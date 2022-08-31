using System.Collections.Generic;
using System.Linq;
using Asteroids.Data;
using Asteroids.Handlers;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Asteroids
{
    public class AsteroidsPool
    {
        #region Fields

        private readonly System.Random random;
        private readonly IGameObjectsManager gameObjectsManager;
        
        private Vector2Int screenHalfDimensions;
        private Dictionary<AsteroidType, List<GameObject>> asteroidsPool 
            = new Dictionary<AsteroidType, List<GameObject>>();
        
        #endregion



        #region Class lifecycle

        public AsteroidsPool(IGameObjectsManager gameObjectsManager)
        {
            this.gameObjectsManager = gameObjectsManager;

            random = new System.Random();
            screenHalfDimensions = new Vector2Int(Screen.width / 2, Screen.height / 2);
        }

        #endregion



        #region Public methods

        public List<Asteroid> GetAllAsteroids(bool includeDeactivated)
        {
            List<GameObject> objects = GetAllAsteroidObjects(includeDeactivated);
            return objects.Select(x => x.GetComponent<Asteroid>()).ToList();
        }
        
        
        public List<Asteroid> SpawnAsteroids(int quantity, Vector3Int playerPosition, int safeRadius)
        {
            List<Asteroid> result = new List<Asteroid>();
            List<AsteroidData> data = CreateAsteroidsData(quantity, playerPosition, safeRadius);

            foreach(AsteroidData info in data)
            {
                Asteroid asteroid = SpawnAsteroid(AsteroidType.Huge, info.Position);
                asteroid.OverrideDirection(info.Direction);
                result.Add(asteroid);
            }

            return result;
        }


        public Asteroid SpawnAsteroidOutOfFOV(AsteroidType type)
        {
            AsteroidData data = CreateAsteroidData(GetOutOfFOVCoordinates());
            Asteroid asteroid = SpawnAsteroid(type, data.Position);
            asteroid.OverrideDirection(data.Direction);

            return asteroid;
        }
        

        public int GetActiveAsteroidsCount() => GetAllAsteroidObjects(false).Count;


        public List<SpawnAsteroidData> GetActiveAsteroidsData()
        {
            List<SpawnAsteroidData> result = new List<SpawnAsteroidData>();
            List<GameObject> activeAsteroids = GetAllAsteroidObjects(false);

            foreach (GameObject activeAsteroid in activeAsteroids)
            {
                BoxCollider2D collider = activeAsteroid.GetComponent<BoxCollider2D>();

                Vector2 colliderSize = collider.size;
                colliderSize.x += PlayerConstants.AsteroidBorderGap;
                colliderSize.y += PlayerConstants.AsteroidBorderGap;

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
                    GameObject.Destroy(asteroid);
                }
            }

            asteroidsPool.Clear();
        }


        public Asteroid SpawnAsteroid(AsteroidType type, Vector3 position)
        {
            Asteroid asteroid = TryReuseAsteroid(type);
            if (asteroid == null)
            {
                asteroid = SpawnNewAsteroid(type);
            }

            asteroid.transform.localPosition = position;
            return asteroid;
        }

        #endregion



        #region Private methods

        private Asteroid SpawnNewAsteroid(AsteroidType type)
        {
            Asteroid[] asteroids = DataContainer.GamePreset.Asteroids;
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
                asteroidsPool.Add(type, new List<GameObject>() { asteroid });
            }

            return asteroid.GetComponent<Asteroid>();
        }


        private Asteroid TryReuseAsteroid(AsteroidType type)
        {
            if (!asteroidsPool.ContainsKey(type))
            {
                return null;
            }

            List<GameObject> spawnedAsteroids = asteroidsPool[type];
            GameObject reusableAsteroid = spawnedAsteroids.Find(asteroid => !asteroid.activeSelf);

            if (reusableAsteroid == null)
            {
                return null;
            }

            reusableAsteroid.SetActive(true);
            return reusableAsteroid.GetComponent<Asteroid>();
        }


        private List<GameObject> GetAllAsteroidObjects(bool includeInactive)
        {
            List<GameObject> asteroids = new List<GameObject>();
            foreach (KeyValuePair<AsteroidType, List<GameObject>> pair in asteroidsPool)
            {
                IEnumerable<GameObject> activeAsteroids = includeInactive? pair.Value : pair.Value.Where(x => x.activeSelf);
                asteroids.AddRange(activeAsteroids);
            }

            return asteroids;
        }


        private List<AsteroidData> CreateAsteroidsData(int quantity, Vector3Int playerPosition, int safeRadius)
        {
            // spawn asteroids around the player not crossing safe zone
            
            int minX = playerPosition.x - safeRadius;
            int maxX = playerPosition.x + safeRadius;
            int minY = playerPosition.y - safeRadius;
            int maxY = playerPosition.y + safeRadius;

            List<AsteroidData> result = new List<AsteroidData>();

            for (int i = 0; i < quantity; i++)
            {
                AsteroidData data = CreateAsteroidData(minX, minY, maxX, maxY);
                result.Add(data);
            }

            return result;
        }


        private AsteroidData CreateAsteroidData(int minX, int minY, int maxX, int maxY)
        {
            Vector3 position = GetRandomPosition(minX, minY, maxX, maxY);
            Vector3 direction = GetRandomDirection(position);
            return new AsteroidData(position, direction);
        }


        private AsteroidData CreateAsteroidData(Vector3Int position)
        {
            Vector3 direction = GetRandomDirection(position);
            return new AsteroidData(position, direction);
        }


        private Vector3 GetRandomDirection(Vector3 position)
        {
            float directionX = position.x > 0 ? random.GetRandomFloat(-1f, 0f) : random.GetRandomFloat(0f, 1f);
            float directionY = position.y > 0 ? random.GetRandomFloat(-1f, 0f) : random.GetRandomFloat(0f, 1f);
            return new Vector3(directionX, directionY);
        }


        private Vector3 GetRandomPosition(int minX, int minY, int maxX, int maxY)
        {
            int positionX = random.GetRandomExclude(-screenHalfDimensions.x, screenHalfDimensions.x, minX, maxX);
            int positionY = random.GetRandomExclude(-screenHalfDimensions.y, screenHalfDimensions.y, minY, maxY);
            return new Vector3(positionX, positionY);
        }


        private Vector3Int GetOutOfFOVCoordinates()
        {
            int minX = -screenHalfDimensions.x;
            int minY = -screenHalfDimensions.y;
            int maxX = screenHalfDimensions.x;
            int maxY = screenHalfDimensions.y;
                    
            Vector3Int result = Vector3Int.zero;
            
            int divider = random.Next(0, 2);

            if (divider == 0)
            {
                result.y = random.Next(minY, maxY);
                result.x = random.GetRandomExclude(
                    minX - PlayerConstants.SurvivalAsteroidSpawnDistance,
                    maxX + PlayerConstants.SurvivalAsteroidSpawnDistance,
                    minX,
                    maxX);
            }
            else
            {
                result.x = random.Next(minX, maxX);
                result.y = random.GetRandomExclude(
                    minY - PlayerConstants.SurvivalAsteroidSpawnDistance,
                    maxY + PlayerConstants.SurvivalAsteroidSpawnDistance,
                    minY,
                    maxY);
            }

            return result;
        }
        
        #endregion
    }
}
