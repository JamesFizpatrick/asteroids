using System;
using System.Collections;
using Asteroids.Handlers;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;


namespace Asteroids.Managers
{
    public class EnemiesManager : IManager
    {
        #region Fields

        public Action OnEnemyKilled;

        private const int UFOSpawnDistance = 10;

        public UFO.UFO Enemy { get; private set; }

        
        private GameObjectsManager gameObjectsManager;
        private PlayerShipsManager playerShipsManager;
        private Random random;

        private Coroutine spawnCoroutine;

        private float currentSpawnDelay = 10f;

        private ManagersHub hub;
        
        #endregion
        
        
        
        #region Public methods

        public void StartSpawnCoroutine(float delay)
        {
            currentSpawnDelay = delay;
            spawnCoroutine = CoroutinesHandler.Instance.StartCoroutine(SpawnWithDelay(currentSpawnDelay));
        }

        
        public bool HasActiveEnemy() => Enemy != null && Enemy.gameObject.activeSelf;

        
        public void Reset()
        {
            if (Enemy)
            {
                Enemy.Killed -= Enemy_Killed;
                Object.Destroy(Enemy.gameObject);
            }

            if (spawnCoroutine != null)
            {
                CoroutinesHandler.Instance.StopCoroutine(spawnCoroutine);
            }
        }
        
        
        public void Initialize(ManagersHub hub)
        {
            this.hub = hub;
            
            gameObjectsManager = hub.GetManager<GameObjectsManager>();
            playerShipsManager = hub.GetManager<PlayerShipsManager>();
            
            random = new Random();
        }

        
        public void Update() { }


        public void Unload()
        {
            if (Enemy)
            {
                Enemy.Killed -= Enemy_Killed;
            }
        }
        
        #endregion


        
        #region Private methods

        private void SpawnEnemy()
        {
            GameObject ufoPrefab = DataContainer.PlayerPreset.Enemy;
            GameObject ufo = gameObjectsManager.CreateEnemy(ufoPrefab);
            
            int maxX = Screen.width / 2;
            int minX = -Screen.width / 2;
            int maxY = Screen.height / 2;
            int minY = -Screen.height / 2;

            int x;
            int y;
            
            // spawn enemy behind the screen
            
            int divider = random.Next(0, 2);

            if (divider == 0)
            {
                x = random.GetRandomExclude(minX - UFOSpawnDistance, maxX + UFOSpawnDistance,
                    minX, maxX);
                y = random.Next(minY, maxY);
            }
            else
            {
                x = random.Next(minX, maxX);
                y = random.GetRandomExclude(minY - UFOSpawnDistance, maxY + UFOSpawnDistance,
                    minY, maxY);
            }
            
            ufo.transform.localPosition = new Vector3(x, y);
            
            Enemy = ufo.GetComponent<UFO.UFO>();
            Enemy.Initialize(playerShipsManager.Player, hub);
            Enemy.Killed += Enemy_Killed;
        }
        
        
        private IEnumerator SpawnWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            SpawnEnemy();
        }

        #endregion
        
        
        
        #region Event handlers

        private void Enemy_Killed()
        {
            OnEnemyKilled?.Invoke();
            
            spawnCoroutine = CoroutinesHandler.Instance.StartCoroutine(SpawnWithDelay(currentSpawnDelay));
        }

        #endregion
    }
}
