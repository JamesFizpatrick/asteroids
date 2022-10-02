using System;
using System.Collections;
using Asteroids.Enemies;
using Asteroids.Handlers;
using Asteroids.VFX;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;


namespace Asteroids.Managers
{
    public class EnemiesManager : IEnemiesManager
    {
        #region Fields

        public event Action OnEnemyKilled;
        
        private float currentSpawnDelay;

        private IGameObjectsManager gameObjectsManager;
        private IPlayerShipsManager playerShipsManager;
        private ISoundManager soundManager;
        private IVfxManager vfxManager;
        
        private Random random;
        private Coroutine spawnCoroutine;
        private UFO enemy;
        
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
            gameObjectsManager = hub.GetManager<IGameObjectsManager>();
            playerShipsManager = hub.GetManager<IPlayerShipsManager>();
            soundManager = hub.GetManager<ISoundManager>();
            vfxManager = hub.GetManager<IVfxManager>();
            
            random = new Random();
        }


        public void Unload() => Reset();

        #endregion



        #region Private methods

        private void SpawnEnemy()
        {
            GameObject ufo = gameObjectsManager.CreateEnemy();
            
            // spawn the enemy behind the screen         
            ufo.transform.localPosition = GetSpawnCoordinates(
                -Screen.width / 2,
                -Screen.height / 2,
                Screen.width / 2,
                Screen.height / 2);

            enemy = ufo.GetComponent<UFO>();
            enemy.Initialize(playerShipsManager.GetPlayer());
            enemy.Killed += Enemy_Killed;
        }
        
        
        private IEnumerator SpawnWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            SpawnEnemy();
        }


        private Vector3 GetSpawnCoordinates(int minX, int minY, int maxX, int maxY)
        {
            Vector3 result = Vector3.zero;

            int divider = random.Next(0, 2);

            if (divider == 0)
            {
                result.x = random.GetRandomExclude(
                    minX - PlayerConstants.UFOSpawnDistance,
                    maxX + PlayerConstants.UFOSpawnDistance,
                    minX,
                    maxX);

                result.y = random.Next(minY, maxY);
            }
            else
            {
                result.x = random.Next(minX, maxX);

                result.y = random.GetRandomExclude(
                    minY - PlayerConstants.UFOSpawnDistance,
                    maxY + PlayerConstants.UFOSpawnDistance,
                    minY,
                    maxY);
            }

            return result;
        }


        #endregion
        
        
        
        #region Event handlers

        private void Enemy_Killed(UFO ufo)
        {
            OnEnemyKilled?.Invoke();
            
            soundManager.PlaySound(SoundType.Explosion);
            vfxManager.SpawnVFX(VFXType.Explosion, ufo.transform.localPosition);
            
            spawnCoroutine = CoroutinesHandler.Instance.StartCoroutine(SpawnWithDelay(currentSpawnDelay));
        }

        #endregion
    }
}
