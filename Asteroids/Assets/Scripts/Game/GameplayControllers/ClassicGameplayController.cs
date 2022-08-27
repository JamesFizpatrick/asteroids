using Asteroids.Data;
using Asteroids.Handlers;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Game
{
    public class ClassicGameplayController : BaseGameplayController
    {
        private readonly PlayerShipsManager playerShipsManager;
        private readonly EnemiesManager enemiesManager;
        private readonly AsteroidsManager asteroidsManager;
        private readonly PlayerProgressManager progressManager;
        
        private LevelsPreset.LevelPreset currentLevelPreset;
        
        public ClassicGameplayController(PlayerShipsManager playerShipsManager, EnemiesManager enemiesManager,
            AsteroidsManager asteroidsManager, PlayerProgressManager progressManager)
        {
            this.playerShipsManager = playerShipsManager;
            this.enemiesManager = enemiesManager;
            this.asteroidsManager = asteroidsManager;
            this.progressManager = progressManager;

        }

        public override void StartGame()
        {
            int nexIndex = progressManager.IncreaseLevelIndex(1);
            
            LevelsPreset gamePreset = DataContainer.LevelsPreset;
            currentLevelPreset = gamePreset.GetLevelPreset(nexIndex);
            
            SpawnEntities();
            SubscribeToEvents();
        }


        public override void StopGame()
        {
            ResetManagers();
            UnsubscribeFromEvents();
        }

        
        public override void Reset()
        {
            StopGame();
            progressManager.SetLevelIndex(-1);
        }
        
        
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
