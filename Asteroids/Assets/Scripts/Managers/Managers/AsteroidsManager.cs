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

        public Action OnFracturesDestroyed;
        public Action OnAllAsteroidsDestroyed;
        public Action<Asteroid> OnAsteroidDestroyed;
        
        private const int FracturesPerAsteroid = 4;
        
        private SoundManager soundManager;
        private VFXManager vfxManager;
        private GameObjectsManager gameObjectsManager;

        private AsteroidsPool asteroidsPool;

        private int currentFracturesCount = 0;
        
        #endregion
        

        
        #region Public methods
        
        public void Initialize(IManagersHub hub)
        {
            soundManager = hub.GetManager<SoundManager>();
            vfxManager = hub.GetManager<VFXManager>();
            gameObjectsManager = hub.GetManager<GameObjectsManager>();
            
            asteroidsPool = new AsteroidsPool(gameObjectsManager);
        }
        
        
        public void SpawnAsteroids(int quantity, Vector3Int playerPosition, int safeRadius)
        {
            List<Asteroid> asteroids = asteroidsPool.SpawnAsteroids(quantity, playerPosition, safeRadius);
            foreach (Asteroid asteroid in asteroids)
            {
                InitAsteroid(asteroid);
            }
        }


        public void SpawnNewAsteroidOutOfFOV()
        {
            Asteroid asteroid = asteroidsPool.SpawnAsteroidOutOfFOV(AsteroidType.Huge);
            InitAsteroid(asteroid);
        }
        
        
        public int GetActiveAsteroidsCount() => asteroidsPool.GetActiveAsteroidsCount();

        
        public List<SpawnAsteroidData> GetActiveAsteroidsData() => asteroidsPool.GetActiveAsteroidsData();


        public void Reset()
        {
            foreach (Asteroid asteroid in asteroidsPool.GetAllAsteroids(false))
            {
                asteroid.Destroyed -= Asteroid_Destroyed;
            }
            
            asteroidsPool.Reset();
        }
        
        #endregion



        #region Private methods

        private void InitAsteroid(Asteroid asteroid)
        {
            asteroid.Destroyed += Asteroid_Destroyed;
            asteroid.Init(soundManager, vfxManager);
        }
        
        
        private bool TrySpawnSubAsteroids(Asteroid asteroid)
        {
            AsteroidType nextType = asteroid.Type.Next();

            if (nextType != AsteroidType.None)
            {
                SpawnSubAsteroids(asteroid.CurrentMoveDirection, nextType, asteroid.transform.localPosition);
                return true;
            }

            CheckFractures();
            return false;
        }

        
        private void CheckFractures()
        {
            currentFracturesCount++;
            
            if (currentFracturesCount >= FracturesPerAsteroid)
            {
                OnFracturesDestroyed?.Invoke();
                currentFracturesCount = 0;
            }
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
            OnAsteroidDestroyed?.Invoke(asteroid);
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
