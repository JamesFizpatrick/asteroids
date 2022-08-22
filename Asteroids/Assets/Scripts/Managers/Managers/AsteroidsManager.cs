using System;
using System.Collections.Generic;
using Asteroids.Asteroids;
using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.Managers
{
    public class AsteroidsManager : IManager
    {                             
        #region Fields
        
        public Action OnAllAsteroidsDestroyed;
        
        private SoundManager soundManager;
        private VFXManager vfxManager;
        private GameObjectsManager gameObjectsManager;

        private AsteroidsPool asteroidsPool;
        
        #endregion
        

        
        #region Public methods
        
        public void SpawnAsteroids(int quantity, Vector3Int playerPosition, int safeRadius)
        {
            List<Asteroid> asteroids = asteroidsPool.SpawnAsteroids(quantity, playerPosition, safeRadius);

            foreach (Asteroid asteroid in asteroids)
            {
                asteroid.Destroyed += Asteroid_Destroyed;
                asteroid.Init(soundManager, vfxManager);
            }
        }

       
        public int GetActiveAsteroidsCount() => asteroidsPool.GetActiveAsteroidsCount();

        
        public List<SpawnAsteroidData> GetActiveAsteroidsData()
        {
            return asteroidsPool.GetActiveAsteroidsData();
        }

            
        public void Reset()
        {
            asteroidsPool.Reset();

            //TODO: unsubscribe from Destroyed
        }


        public void Initialize(IManagersHub hub)
        {
            soundManager = hub.GetManager<SoundManager>();
            vfxManager = hub.GetManager<VFXManager>();
            gameObjectsManager = hub.GetManager<GameObjectsManager>();
            
            asteroidsPool = new AsteroidsPool(gameObjectsManager);
        }

        #endregion



        #region Private methods

        private bool TrySpawnSubAsteroids(Asteroid asteroid)
        {
            AsteroidType nextType = asteroid.Type.Next();

            if (nextType != AsteroidType.None)
            {
                SpawnSubAsteroids(asteroid.CurrentMoveDirection,
                    nextType,
                    asteroid.transform.localPosition);

                return true;
            }

            return false;
        }


        private void SpawnSubAsteroids(Vector3 direction, AsteroidType nextType, Vector3 parentLocalPosition)
        {
            Asteroid leftAsteroid = CreateChildAsteroid(nextType, parentLocalPosition);
            Asteroid rightAsteroid = CreateChildAsteroid(nextType, parentLocalPosition);

            (Vector3 leftVector, Vector3 rightVector) = direction.GetBreakVectors(30f);

            leftAsteroid.OverrideDirection(leftVector);
            rightAsteroid.OverrideDirection(rightVector);
        }


        private Asteroid CreateChildAsteroid(AsteroidType nextType, Vector3 position)
        {
            Asteroid asteroid = asteroidsPool.SpawnAsteroid(nextType, position);
            asteroid.Destroyed += Asteroid_Destroyed;
            asteroid.Init(soundManager, vfxManager);

            return asteroid;
        }

        #endregion



        #region Event handlers

        private void Asteroid_Destroyed(Asteroid asteroid)
        {
            asteroid.Destroyed -= Asteroid_Destroyed;

            bool spawned = TrySpawnSubAsteroids(asteroid);
            if (!spawned && GetActiveAsteroidsCount() == 0)
            {
                OnAllAsteroidsDestroyed?.Invoke();
            }
        }

        #endregion
    }
}
