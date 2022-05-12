using System;
using Asteroids.Game;
using Asteroids.Handlers;
using Asteroids.VFX;
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

        private SoundManager soundManager;
        private VFXManager vfxManager;
        private GameObjectsManager gameObjectsManager;
        private Random random;
        
        #endregion
        
        
        
        #region Public methods
        
        public bool HasActiveEnemy() => Enemy != null && Enemy.gameObject.activeSelf;


        public void SpawnEnemy(Player player)
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
            Enemy.Initialize(player);
            Enemy.Killed += Enemy_Killed;
        }


        public void Reset()
        {
            if (Enemy)
            {
                Enemy.Killed -= Enemy_Killed;
                Object.Destroy(Enemy.gameObject);
            }
        }
        
        
        public void Initialize(ManagersHub hub)
        {
            soundManager = hub.GetManager<SoundManager>();
            vfxManager = hub.GetManager<VFXManager>();
            gameObjectsManager = hub.GetManager<GameObjectsManager>();
            
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

        
        
        #region Event handlers

        private void Enemy_Killed()
        {
            soundManager.PlaySound(SoundType.Explosion);
            vfxManager.SpawnVFX(VFXType.Explosion, Enemy.transform.localPosition);
            OnEnemyKilled?.Invoke();
        }

        #endregion
    }
}
