using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Asteroids;
using Asteroids.Handlers;
using Asteroids.VFX;
using UnityEngine;
using Random = System.Random;


namespace Asteroids.Managers
{
    public class AsteroidsManager : IManager
    {
        #region Nested types

        public struct SpawnAsteroidData
        {
            public Vector3Int LocalPosition;
            public Vector2Int ColliderSize;

            public SpawnAsteroidData(Vector3 localPosition, Vector2 colliderSize)
            {
                LocalPosition = Vector3Int.FloorToInt(localPosition);
                ColliderSize = Vector2Int.FloorToInt(colliderSize);
            }
        }

        #endregion
        
        
        
        #region Fields
        
        public Action OnAllAsteroidsDestroyed;
        
        private const int AsteroidBorderGap = 60;

        private SoundManager soundManager;
        private VFXManager vfxManager;
        private GameObjectsManager gameObjectsManager;
        
        private Dictionary<AsteroidType, List<GameObject>> asteroidsPool =
            new Dictionary<AsteroidType, List<GameObject>>();

        private Random random;
        
        #endregion
        

        
        #region Public methods
        
        public void SpawnAsteroids(int quantity, Vector3Int playerPosition, int safeRadius)
        {
            int minX = playerPosition.x - safeRadius;
            int maxX = playerPosition.x + safeRadius;
            int minY = playerPosition.y - safeRadius;
            int maxY = playerPosition.y + safeRadius;

            // spawn asteroids around the player not crossing safe zone
            for (int i = 0; i < quantity; i++)
            {
                var positionX = random.GetRandomExclude(-Screen.width / 2,
                    Screen.width / 2,
                    minX,
                    maxX);
                var positionY = random.GetRandomExclude(-Screen.height / 2,
                    Screen.height / 2,
                    minY,
                    maxY);
                var position = new Vector3(positionX, positionY);

                var directionX = positionX > 0 ? random.GetRandomFloat(-1f, 0f) : random.GetRandomFloat(0f, 1f);
                var directionY = positionY > 0 ? random.GetRandomFloat(-1f, 0f) : random.GetRandomFloat(0f, 1f);
                
                var direction = new Vector3(directionX, directionY);
                
                Asteroid asteroid = SpawnAsteroid(AsteroidType.Huge, position);
                asteroid.OverrideDirection(direction);
            }
        }


        public int GetActiveAsteroidsCount() => GetActiveAsteroids().Count;

        
        public List<SpawnAsteroidData> GetActiveAsteroidsData()
        {
            List<SpawnAsteroidData> result = new List<SpawnAsteroidData>();
            
            List<GameObject> activeAsteroids = GetActiveAsteroids();

            foreach (GameObject activeAsteroid in activeAsteroids)
            {
                BoxCollider2D collider = activeAsteroid.GetComponent<BoxCollider2D>();
                Vector2 colliderSize = new Vector2(collider.size.x + AsteroidBorderGap,
                    collider.size.y + AsteroidBorderGap);
                
                SpawnAsteroidData data = new SpawnAsteroidData(activeAsteroid.transform.localPosition, colliderSize);
                result.Add(data);
            }

            return result;
        }

            
        public void Reset()
        {
            foreach (KeyValuePair<AsteroidType, List<GameObject>> pair in asteroidsPool)
            {
                foreach (GameObject asteroid in pair.Value)
                {
                    Asteroid asteroidComponent = asteroid.GetComponent<Asteroid>();
                    asteroidComponent.Destroyed -= Asteroid_Destroyed;
                    UnityEngine.Object.Destroy(asteroid);
                }
            }

            asteroidsPool.Clear();
        }


        public void Initialize(IManagersHub hub)
        {
            soundManager = hub.GetManager<SoundManager>();
            vfxManager = hub.GetManager<VFXManager>();
            gameObjectsManager = hub.GetManager<GameObjectsManager>();
            
            random = new Random();
        }
               
        #endregion



        #region Private methods
        
        private Asteroid SpawnAsteroid(AsteroidType type, Vector3 position)
        {
            Asteroid asteroid = TryReuseAsteroid(type);
            if (asteroid == null)
            {
                asteroid = SpawnNewAsteroid(type);
            }
            
            asteroid.transform.localPosition = position;
            return asteroid;
        }
        
        
        private Asteroid SpawnNewAsteroid(AsteroidType type)
        {
            Asteroid[] asteroids = DataContainer.PlayerPreset.Asteroids;
            List<Asteroid> selectedAsteroid = asteroids.Where(a => a.Type == type).ToList();

            int index = random.Next(0, selectedAsteroid.Count);

            GameObject asteroid = gameObjectsManager.CreateAsteroid(selectedAsteroid[index].gameObject);

            if (asteroidsPool.ContainsKey(type))
            {
                List<GameObject> asteroidsList = asteroidsPool[type];
                asteroidsList.Add(asteroid);
                asteroidsPool[type] = asteroidsList;
            }
            else
            {
                asteroidsPool.Add(type, new List<GameObject>(){asteroid});
            }

            Asteroid asteroidComponent = asteroid.GetComponent<Asteroid>();
            asteroidComponent.Destroyed += Asteroid_Destroyed;
            
            return asteroidComponent;
        }

        
        private Asteroid TryReuseAsteroid(AsteroidType type)
        {
            if (!asteroidsPool.ContainsKey(type))
            {
                return null;
            }
            
            List<GameObject> spawnedAsteroids = asteroidsPool[type];
            GameObject reusableAsteroid = spawnedAsteroids.Find(asteroid => !asteroid.gameObject.activeSelf);

            if (reusableAsteroid == null)
            {
                return null;
            }
            
            reusableAsteroid.SetActive(true);
            return reusableAsteroid.GetComponent<Asteroid>();
        }
        
        
        private List<GameObject> GetActiveAsteroids()
        {
            List<GameObject> asteroids = new List<GameObject>();
            foreach (KeyValuePair<AsteroidType, List<GameObject>> pair in asteroidsPool)
            {
                IEnumerable<GameObject> activeAsteroids = pair.Value.Where(x => x.activeSelf);
                asteroids.AddRange(activeAsteroids);
            }

            return asteroids;
        }
        
        #endregion



        #region Event handlers

        private void Asteroid_Destroyed(Asteroid asteroid)
        {
            asteroid.Destroyed -= Asteroid_Destroyed;
            
            soundManager.PlaySound(SoundType.Explosion);
            vfxManager.SpawnVFX(VFXType.Explosion, asteroid.transform.localPosition);
            
            // Spawn two smaller asteroids
            Vector3 direction = asteroid.CurrentMoveDirection;
            AsteroidType nextType = asteroid.Type.Next();
            Vector3 parentLocalPosition = asteroid.transform.localPosition;
            
            if (nextType != AsteroidType.None)
            {
                Asteroid leftAsteroid = CreateChildAsteroid(nextType, parentLocalPosition);
                Asteroid rightAsteroid = CreateChildAsteroid(nextType, parentLocalPosition);
                
                (Vector3 leftVector, Vector3 rightVector) = direction.GetBreakVectors(30f);
                
                leftAsteroid.OverrideDirection(leftVector);
                rightAsteroid.OverrideDirection(rightVector);
            }
            else if (GetActiveAsteroidsCount() == 0)
            {
                OnAllAsteroidsDestroyed?.Invoke();
            }
        }


        private Asteroid CreateChildAsteroid(AsteroidType nextType, Vector3 position)
        {
            Asteroid asteroid = SpawnAsteroid(nextType, position);
            asteroid.Destroyed += Asteroid_Destroyed;

            return asteroid;
        }
        
        #endregion
    }
}
