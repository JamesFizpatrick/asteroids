using System;
using System.Collections;
using Asteroids.Handlers;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;


namespace Asteroids.Managers
{
    public class EnemiesManager : IManager, IUnloadableManager
    {
        #region Fields

        public Action OnEnemyKilled;

        private const int UFOSpawnDistance = 10;
        
        private float currentSpawnDelay = 10f;

        private GameObjectsManager gameObjectsManager;
        private PlayerShipsManager playerShipsManager;
        
        private Random random;
        private Coroutine spawnCoroutine;
        
        private IManagersHub hub;
        private UFO.UFO enemy;
        
        #endregion
        
        
        
        #region Public methods

        public void StartSpawnCoroutine(float delay)
        {
            currentSpawnDelay = delay;
            spawnCoroutine = CoroutinesHandler.Instance.StartCoroutine(SpawnWithDelay(currentSpawnDelay));
        }

        
        public bool HasActiveEnemy() => enemy != null && enemy.gameObject.activeSelf;

        
        public void Reset()
        {
            if (enemy)
            {
                enemy.Killed -= Enemy_Killed;
                Object.Destroy(enemy.gameObject);
            }

            if (spawnCoroutine != null)
            {
                CoroutinesHandler.Instance.StopCoroutine(spawnCoroutine);
            }
        }
        
        
        public void Initialize(IManagersHub hub)
        {
            this.hub = hub;
            
            gameObjectsManager = hub.GetManager<GameObjectsManager>();
            playerShipsManager = hub.GetManager<PlayerShipsManager>();
            
            random = new Random();
        }

       
        public void Unload()
        {
            if (enemy)
            {
                enemy.Killed -= Enemy_Killed;
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
            
            enemy = ufo.GetComponent<UFO.UFO>();
            enemy.Initialize(playerShipsManager.Player, hub);
            enemy.Killed += Enemy_Killed;
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
