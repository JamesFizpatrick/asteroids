using System;
using Asteroids.Data;
using Asteroids.Handlers;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Game
{
    public class ClassicGameplayController : BaseGameplayController
    {
        #region Fields

        public Action<int> OnLevelIndexChanged;
        
        private readonly PlayerShipsManager playerShipsManager;
        private readonly EnemiesManager enemiesManager;
        private readonly AsteroidsManager asteroidsManager;
        private readonly PlayerProgressManager progressManager;
        
        private LevelsPreset.LevelPreset currentLevelPreset;

        private int currentLevelIndex = 0;

        #endregion



        #region Class lifecycle

        public ClassicGameplayController(PlayerShipsManager playerShipsManager, EnemiesManager enemiesManager,
            AsteroidsManager asteroidsManager, PlayerProgressManager progressManager)
        {
            this.playerShipsManager = playerShipsManager;
            this.enemiesManager = enemiesManager;
            this.asteroidsManager = asteroidsManager;
            this.progressManager = progressManager;

        }

        #endregion



        #region Public methods

        public override void StartGame()
        {
            currentLevelIndex = progressManager.GetLevelIndex() + 1;
            
            LevelsPreset gamePreset = DataContainer.LevelsPreset;
            currentLevelPreset = gamePreset.GetLevelPreset(currentLevelIndex);
            
            SpawnEntities();
            SubscribeToEvents();
        }


        public int GetCurrentLevelIndex() => currentLevelIndex;

        
        public override void StopGame()
        {
            ResetManagers();
            UnsubscribeFromEvents();
        }

        
        public override void Reset()
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

            UnsubscribeFromEvents();
        }

        
        private void SubscribeToEvents()
        {
            playerShipsManager.OnPlayerKilled += PlayerShipsManager_OnPlayerKilled;
            playerShipsManager.OnPlayerHealthValueChanged += PlayerShipManager_OnPlayerHealthValueChanged;
            asteroidsManager.OnAllAsteroidsDestroyed += AsteroidsManager_OnAllAsteroidsDestroyed;
            enemiesManager.OnEnemyKilled += EnemiesManager_OnEnemyKilled;
        }
        
        
        private void UnsubscribeFromEvents()
        {
            playerShipsManager.OnPlayerKilled -= PlayerShipsManager_OnPlayerKilled;
            playerShipsManager.OnPlayerHealthValueChanged -= PlayerShipManager_OnPlayerHealthValueChanged;
            asteroidsManager.OnAllAsteroidsDestroyed -= AsteroidsManager_OnAllAsteroidsDestroyed;
            enemiesManager.OnEnemyKilled -= EnemiesManager_OnEnemyKilled;
        }


        private void ProcessPlayerWin()
        {
            OnPlayerWin?.Invoke();
            progressManager.SetLevelIndex(currentLevelIndex);
        }
        
        #endregion


        
        #region Event handlers

        private void PlayerShipsManager_OnPlayerKilled()
        {
            ResetManagers();
            progressManager.SetLevelIndex(-1);
            OnPlayerLose?.Invoke();
        }


        private void PlayerShipManager_OnPlayerHealthValueChanged(int newValue)
        {
            if (asteroidsManager.GetActiveAsteroidsCount() == 0 &&
                !enemiesManager.HasActiveEnemy())
            {
                ProcessPlayerWin();
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
                ProcessPlayerWin();
            }
        }


        private void EnemiesManager_OnEnemyKilled()
        {
            if (asteroidsManager.GetActiveAsteroidsCount() == 0)
            {
                ResetManagers();
                ProcessPlayerWin();
            }
        }
        
        #endregion
    }
}
