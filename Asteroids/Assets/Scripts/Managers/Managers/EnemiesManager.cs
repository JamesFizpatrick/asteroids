using System;
using Asteroids.Game;
using Asteroids.Handlers;
using Asteroids.VFX;
using UnityEngine;
using Random = System.Random;


namespace Asteroids.Managers
{
    public class EnemiesManager : BaseManager<EnemiesManager>
    {
        #region Fields

        public Action OnEnemyKilled;
        
        public UFO.UFO Enemy { get; private set; }
        private const int UFOSpawnDistance = 10;

        private SoundManager soundManager;
        private VFXManager vfxManager;
        private GameObjectsManager gameObjectsManager;
        private DataManager dataManager;

        #endregion
        
        
        
        #region Public methods
        
        public bool HasActiveEnemy() => Enemy != null && Enemy.gameObject.activeSelf;


        public void SpawnEnemy(Player player)
        {
            GameObject ufoPrefab = dataManager.PlayerPreset.Enemy;
            GameObject ufo = gameObjectsManager.CreateEnemy(ufoPrefab);

            Random random = new Random();
            
            int maxX = Screen.width / 2;
            int minX = -Screen.width / 2;
            int maxY = Screen.height / 2;
            int minY = -Screen.height / 2;

            int x;
            int y;
            
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
            Enemy.Initialze(player);
            Enemy.Killed += Enemy_Killed;
        }


        public void Reset()
        {
            if (Enemy)
            {
                Enemy.Killed -= Enemy_Killed;
                Destroy(Enemy.gameObject);
            }
        }
        
        #endregion



        #region Protected methods

        protected override void Initialize()
        {
            soundManager = ManagersHub.GetManager<SoundManager>();
            vfxManager = ManagersHub.GetManager<VFXManager>();
            gameObjectsManager = ManagersHub.GetManager<GameObjectsManager>();
            dataManager = ManagersHub.GetManager<DataManager>();
        }

        
        protected override void Deinitialize()
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
