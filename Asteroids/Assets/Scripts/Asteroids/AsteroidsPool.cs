using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Handlers;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Asteroids
{
    public class AsteroidsPool
    {
        #region Nested types

        private struct AsteroidData
        {
            public Vector3 position;
            public Vector3 direction;


            public AsteroidData(Vector3 position, Vector3 direction)
            {
                this.position = position;
                this.direction = direction;
            }
        }

        #endregion



        #region Fields

        public Action OnAllAsteroidsDestroyed;

        public Dictionary<AsteroidType, List<GameObject>> asteroidsPool =
            new Dictionary<AsteroidType, List<GameObject>>();

        private System.Random random;
        private GameObjectsManager gameObjectsManager;

        #endregion



        #region Class lifecycle

        public AsteroidsPool(GameObjectsManager gameObjectsManager)
        {
            random = new System.Random();
            this.gameObjectsManager = gameObjectsManager;
        }

        #endregion



        #region Public methods

        public List<Asteroid> SpawnAsteroids(int quantity, Vector3Int playerPosition, int safeRadius)
        {
            List<Asteroid> result = new List<Asteroid>();
            List<AsteroidData> data = CreateAsteroidsData(quantity, playerPosition, safeRadius);

            foreach(AsteroidData info in data)
            {
                Asteroid asteroid = SpawnAsteroid(AsteroidType.Huge, info.position);
                asteroid.OverrideDirection(info.direction);
                result.Add(asteroid);
            }

            return result;
        }

      
        public int GetActiveAsteroidsCount() => GetActiveAsteroids().Count;


        public List<SpawnAsteroidData> GetActiveAsteroidsData()
        {
            List<SpawnAsteroidData> result = new List<SpawnAsteroidData>();

            List<GameObject> activeAsteroids = GetActiveAsteroids();

            foreach (GameObject activeAsteroid in activeAsteroids)
            {
                BoxCollider2D collider = activeAsteroid.GetComponent<BoxCollider2D>();
                Vector2 colliderSize = new Vector2(collider.size.x + PlayerConstants.AsteroidBorderGap,
                    collider.size.y + PlayerConstants.AsteroidBorderGap);

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


        private List<AsteroidData> CreateAsteroidsData(int quantity, Vector3Int playerPosition, int safeRadius)
        {
            int minX = playerPosition.x - safeRadius;
            int maxX = playerPosition.x + safeRadius;
            int minY = playerPosition.y - safeRadius;
            int maxY = playerPosition.y + safeRadius;

            List<AsteroidData> result = new List<AsteroidData>();

            // spawn asteroids around the player not crossing safe zone
            for (int i = 0; i < quantity; i++)
            {
                AsteroidData data = CreateAsteroidData(minX, minY, maxX, maxY);
                result.Add(data);
            }

            return result;
        }


        private AsteroidData CreateAsteroidData(int minX, int minY, int maxX, int maxY)
        {
            int positionX = random.GetRandomExclude(
                -Screen.width / 2,
                Screen.width / 2,
                minX,
                maxX);

            int positionY = random.GetRandomExclude(
                -Screen.height / 2,
                Screen.height / 2,
                minY,
                maxY);

            Vector3 position = new Vector3(positionX, positionY);

            float directionX = positionX > 0 ?
                random.GetRandomFloat(-1f, 0f) :
                random.GetRandomFloat(0f, 1f);

            float directionY = positionY > 0 ?
                random.GetRandomFloat(-1f, 0f) :
                random.GetRandomFloat(0f, 1f);

            var direction = new Vector3(directionX, directionY);

            return new AsteroidData(position, direction);
        }
      
        #endregion
    }
}
