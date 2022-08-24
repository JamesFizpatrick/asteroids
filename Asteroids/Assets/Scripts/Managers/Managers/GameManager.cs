using System;
using Asteroids.Data;
using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.Managers
{
    public class GameManager : IManager
    {
        #region Fields

        public Action OnPlayerWin;
        public Action OnPlayerLose;
        
        private int currentLevelIndex = -1;

        private LevelsPreset.LevelPreset currentLevelPreset;

        private PlayerShipsManager playerShipsManager;
        private EnemiesManager enemiesManager;
        private AsteroidsManager asteroidsManager;
        private VFXManager vfxManager;

        #endregion



        #region Protected methods

        public void Initialize(IManagersHub hub)
        {
            playerShipsManager = hub.GetManager<PlayerShipsManager>();
            asteroidsManager = hub.GetManager<AsteroidsManager>();
            enemiesManager = hub.GetManager<EnemiesManager>();
            vfxManager = hub.GetManager<VFXManager>();
        }

        #endregion



        #region Public methods

        public void StartGame()
        {
            currentLevelIndex++;
            
            LevelsPreset gamePreset = DataContainer.LevelsPreset;
            currentLevelPreset = gamePreset.GetLevelPreset(currentLevelIndex);
            
            SpawnEntities();
            SubscribeToEvents();
        }


        public void StopGame()
        {
            ResetManagers();
            UnsubscribeFromEvents();
        }

        
        public void Reset()
        {
            StopGame();
            currentLevelIndex = -1;
        }

        #endregion



        #region Private methods
        
        private void SpawnEntities()
        {
            playerShipsManager.SpawnPlayer();
            enemiesManager.StartSpawnCoroutine(currentLevelPreset.EnemiesDelay);

            asteroidsManager.SpawnAsteroids(currentLevelPreset.AsteroidsCount,
                Vector3Int.FloorToInt(playerShipsManager.Player.transform.localPosition),
                PlayerConstants.InitialPlayerSafeRadius);
        }


        private void ResetManagers()
        {
            playerShipsManager.Reset();
            asteroidsManager.Reset();
            enemiesManager.Reset();
            vfxManager.Reset();

            UnsubscribeFromEvents();
        }

        
        private void SubscribeToEvents()
        {
            playerShipsManager.OnPlayerKilled += PlayerShipsManager_OnPlayerKilled;
            playerShipsManager.OnPlayerHealthValueChanged += PlayerShipManager_OnPlayerHealthValueChanged;
            asteroidsManager.OnAllAsteroidsDestroyed += AsteroidsManager_OnAllAsteroidsDestroyed;
        }
        
        
        private void UnsubscribeFromEvents()
        {
            playerShipsManager.OnPlayerKilled -= PlayerShipsManager_OnPlayerKilled;
            playerShipsManager.OnPlayerHealthValueChanged -= PlayerShipManager_OnPlayerHealthValueChanged;
            asteroidsManager.OnAllAsteroidsDestroyed -= AsteroidsManager_OnAllAsteroidsDestroyed;
            enemiesManager.OnEnemyKilled -= EnemiesManager_OnEnemyKilled;
        }

        #endregion


        
        #region Event handlers

        private void PlayerShipsManager_OnPlayerKilled()
        {
            ResetManagers();
            OnPlayerLose?.Invoke();
        }


        private void PlayerShipManager_OnPlayerHealthValueChanged(int newValue)
        {
            if (asteroidsManager.GetActiveAsteroidsCount() == 0 &&
                !enemiesManager.HasActiveEnemy())
            {
                OnPlayerWin?.Invoke();
            }
            else
            {
                playerShipsManager.RespawnPlayer(1f, 1f, 2f);
            }
        }


        private void AsteroidsManager_OnAllAsteroidsDestroyed()
        {
            if (!enemiesManager.HasActiveEnemy())
            {
                ResetManagers();
                OnPlayerWin?.Invoke();
            }
        }


        private void EnemiesManager_OnEnemyKilled()
        {
            if (asteroidsManager.GetActiveAsteroidsCount() == 0)
            {
                ResetManagers();
                OnPlayerWin?.Invoke();
            }
        }
        
        #endregion
    }
}
