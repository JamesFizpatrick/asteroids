using System;
using Asteroids.Data;
using Asteroids.UI;


namespace Asteroids.Managers
{
    public class GameManager : IManager
    {
        #region Fields

        public Action OnReset; 
        public Action<int> OnPlayerHealthValueChanged;

        private const float InitialPlayerSafeRadius = 200f;
        
        private PlayerShipsManager playerShipsManager;
        private EnemiesManager enemiesManager;
        private AsteroidsManager asteroidsManager;
        private UIManager uiManager;
        private VFXManager vfxManager;

        private int currentLevelPresetIndex = -1;
        private GamePreset.LevelPreset currentLevelPreset;

        private int currentPlayerHealth;
        
        #endregion

        
        
        #region Protected methods
        
        public void Initialize(ManagersHub hub)
        {
            playerShipsManager = hub.GetManager<PlayerShipsManager>();
            asteroidsManager = hub.GetManager<AsteroidsManager>();
            enemiesManager = hub.GetManager<EnemiesManager>();
            uiManager = hub.GetManager<UIManager>();
            vfxManager = hub.GetManager<VFXManager>();
        }


        public void Update() { }

        
        public void Unload() { }

        
        public void Start() => StartGame();

        #endregion



        #region Private methods

        private void StartGame()
        {
            currentPlayerHealth = DataContainer.PlayerPreset.PlayerLivesQuantity;

            if (TrySwitchToTheNextLevel())
            {
                SubscribeAndSpawn();
            }
        }


        private void StartNextLevel()
        {
            ResetAndUnsubscribe();

            if (TrySwitchToTheNextLevel())
            {
                SubscribeAndSpawn();
            }
            else
            {
                EndGame();
            }
        }


        private void ResetGame()
        {
            OnReset?.Invoke();
            
            currentLevelPresetIndex = -1;
            currentPlayerHealth = DataContainer.PlayerPreset.PlayerLivesQuantity;
            
            StartNextLevel();
        }


        private void EndGame()
        {
            uiManager.ShowScreen(ScreenType.Win, () =>
            {
                currentLevelPresetIndex = 0;
                ResetGame();
            });
        }
        
        
        private bool TrySwitchToTheNextLevel()
        {
            currentLevelPresetIndex++;
            GamePreset gamePreset = DataContainer.GamePreset;
            
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
                playerShipsManager.Player.transform.localPosition,
                InitialPlayerSafeRadius);
            
            playerShipsManager.OnPlayerKilled += PlayerShipsManager_OnPlayerKilled;
            asteroidsManager.OnAllAsteroidsDestroyed += AsteroidsManager_OnAllAsteroidsDestroyed;
            enemiesManager.OnEnemyKilled += EnemiesManager_OnEnemyKilled;
        }


        private void ResetAndUnsubscribe()
        {
            playerShipsManager.Reset();
            asteroidsManager.Reset();
            enemiesManager.Reset();
            vfxManager.Reset();

            playerShipsManager.OnPlayerKilled -= PlayerShipsManager_OnPlayerKilled;
            asteroidsManager.OnAllAsteroidsDestroyed -= AsteroidsManager_OnAllAsteroidsDestroyed;
            enemiesManager.OnEnemyKilled -= EnemiesManager_OnEnemyKilled;
        }
        
        #endregion


        
        #region Event handlers

        private void PlayerShipsManager_OnPlayerKilled()
        {
            currentPlayerHealth--;
            
            OnPlayerHealthValueChanged?.Invoke(currentPlayerHealth);
            
            if (currentPlayerHealth <= 0)
            {
                ResetAndUnsubscribe();
                uiManager.ShowScreen(ScreenType.Lose, ResetGame);
            }
            else if (asteroidsManager.GetActiveAsteroidsCount() == 0 && !enemiesManager.HasActiveEnemy())
            {
                StartNextLevel();
            }
            else
            {
                playerShipsManager.RespawnPlayer(2f, 1f, 2f);
            }
        }
        
        
        private void AsteroidsManager_OnAllAsteroidsDestroyed()
        {
            if (!enemiesManager.HasActiveEnemy())
            {
                StartNextLevel();
            }
        }


        private void EnemiesManager_OnEnemyKilled()
        {
            if (asteroidsManager.GetActiveAsteroidsCount() == 0)
            {
                StartNextLevel();
            }
        }
        
        #endregion
    }
}
