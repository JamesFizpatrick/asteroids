using System;
using Asteroids.Data;
using Asteroids.Handlers;
using Asteroids.UI;
using UnityEngine;


namespace Asteroids.Managers
{
    public class GameManager : IManager
    {
        #region Fields

        private int currentLevelPresetIndex = -1;

        private LevelsPreset.LevelPreset currentLevelPreset;

        private PlayerShipsManager playerShipsManager;
        private EnemiesManager enemiesManager;
        private AsteroidsManager asteroidsManager;
        private UIManager uiManager;
        private VFXManager vfxManager;

        #endregion



        #region Protected methods

        public void Initialize(IManagersHub hub)
        {
            playerShipsManager = hub.GetManager<PlayerShipsManager>();
            asteroidsManager = hub.GetManager<AsteroidsManager>();
            enemiesManager = hub.GetManager<EnemiesManager>();
            uiManager = hub.GetManager<UIManager>();
            vfxManager = hub.GetManager<VFXManager>();
        }

        #endregion



        #region Public methods

        public void StartGame()
        {
            if (CanSwitchToTheNextLevel())
            {
                SubscribeAndSpawn();
            }
        }


        public void ResetGame()
        {
            OnGameStateChanged?.Invoke(GameState.InGame);

            currentLevelPresetIndex = -1;
            playerShipsManager.Reset();

            TrySwitchToTheNextLevel();
        }

        #endregion



        #region Private methods

        private void TrySwitchToTheNextLevel(bool withInterScreen = false)
        {            
            ResetAndUnsubscribe();

            if (CanSwitchToTheNextLevel())
            {
                if (withInterScreen)
                {
                    OnGameStateChanged?.Invoke(GameState.NotInGame);
                    uiManager.ShowScreen<InterScreen>(SubscribeAndSpawn);
                }
                else
                {
                    SubscribeAndSpawn();
                }
            }
            else
            {
                uiManager.ShowScreen<WinScreen>();
            }
        }

     
        private bool CanSwitchToTheNextLevel()
        {
            currentLevelPresetIndex++;
            LevelsPreset gamePreset = DataContainer.LevelsPreset;
            
            if (currentLevelPresetIndex > gamePreset.GetLevelPresets().Length - 1)
            {
                return false;
            }
            
            currentLevelPreset = gamePreset.GetLevelPreset(currentLevelPresetIndex);

            return true;
        }


        private void SubscribeAndSpawn()
        {
            playerShipsManager.SpawnPlayer();
            enemiesManager.StartSpawnCoroutine(currentLevelPreset.EnemiesDelay);

            asteroidsManager.SpawnAsteroids(currentLevelPreset.AsteroidsCount,
                Vector3Int.FloorToInt(playerShipsManager.Player.transform.localPosition),
                PlayerConstants.InitialPlayerSafeRadius);
            
            playerShipsManager.OnPlayerKilled += PlayerShipsManager_OnPlayerKilled;
            playerShipsManager.OnPlayerHealthValueChanged += PlayerShipManager_OnPlayerHealthValueChanged;

            asteroidsManager.OnAllAsteroidsDestroyed += AsteroidsManager_OnAllAsteroidsDestroyed;

            enemiesManager.OnEnemyKilled += EnemiesManager_OnEnemyKilled;

            OnGameStateChanged?.Invoke(GameState.InGame);
        }


        private void ResetAndUnsubscribe()
        {
            playerShipsManager.Reset();
            asteroidsManager.Reset();
            enemiesManager.Reset();
            vfxManager.Reset();

            playerShipsManager.OnPlayerKilled -= PlayerShipsManager_OnPlayerKilled;
            playerShipsManager.OnPlayerHealthValueChanged -= PlayerShipManager_OnPlayerHealthValueChanged;

            asteroidsManager.OnAllAsteroidsDestroyed -= AsteroidsManager_OnAllAsteroidsDestroyed;

            enemiesManager.OnEnemyKilled -= EnemiesManager_OnEnemyKilled;
        }

        #endregion


        
        #region Event handlers

        private void PlayerShipsManager_OnPlayerKilled()
        {
            ResetAndUnsubscribe();
            uiManager.ShowScreen<LoseScreen>();
            OnGameStateChanged?.Invoke(GameState.NotInGame);
        }


        private void PlayerShipManager_OnPlayerHealthValueChanged(int newValue)
        {
            if (asteroidsManager.GetActiveAsteroidsCount() == 0 &&
                !enemiesManager.HasActiveEnemy())
            {
                TrySwitchToTheNextLevel(true);
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
                TrySwitchToTheNextLevel(true);
            }
        }


        private void EnemiesManager_OnEnemyKilled()
        {
            if (asteroidsManager.GetActiveAsteroidsCount() == 0)
            {
                TrySwitchToTheNextLevel(true);
            }
        }
        
        #endregion
    }
}
