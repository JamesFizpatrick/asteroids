using Asteroids.Data;


namespace Asteroids.Managers
{
    public class GameManager : IManager
    {
        #region Fields

        private const int PlayerRespawnDistanceFromBorders = 100;
        private const float InitialPlayerSafeRadius = 100f;
        
        private PlayerShipsManager playerShipsManager;
        private EnemiesManager enemiesManager;
        private AsteroidsManager asteroidsManager;

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
        }


        public void Update() { }

        
        public void Unload() { }

        
        public void Start() => StartGame();

        #endregion



        #region Private methods

        private void StartGame()
        {
            SwitchToTheNextLevel();

            currentPlayerHealth = DataContainer.PlayerPreset.PlayerLivesQuantity;
            
            playerShipsManager.SpawnPlayer();
            playerShipsManager.OnPlayerKilled += PlayerShipsManager_OnPlayerKilled;
            
            asteroidsManager.SpawnAsteroids(currentLevelPreset.AsteroidsCount,
                playerShipsManager.Player.transform.localPosition,
                InitialPlayerSafeRadius);
            
            asteroidsManager.OnInitialAsteroidsDestroyed += AsteroidsManager_InitialAsteroidsDestroyed;
            asteroidsManager.OnAllAsteroidsDestroyed += AsteroidsManager_OnAllAsteroidsDestroyed;

            enemiesManager.OnEnemyKilled += EnemiesManager_OnEnemyKilled;
        }


        private void StartNextLevel()
        {
            SwitchToTheNextLevel();
            
            playerShipsManager.Reset();
            playerShipsManager.SpawnPlayer();
            
            asteroidsManager.OnInitialAsteroidsDestroyed -= AsteroidsManager_InitialAsteroidsDestroyed;
            asteroidsManager.OnAllAsteroidsDestroyed -= AsteroidsManager_OnAllAsteroidsDestroyed;
            
            asteroidsManager.Reset();
            asteroidsManager.SpawnAsteroids(currentLevelPreset.AsteroidsCount,
                playerShipsManager.Player.transform.localPosition,
                InitialPlayerSafeRadius);
            
            asteroidsManager.OnInitialAsteroidsDestroyed += AsteroidsManager_InitialAsteroidsDestroyed;
            asteroidsManager.OnAllAsteroidsDestroyed += AsteroidsManager_OnAllAsteroidsDestroyed;

            enemiesManager.Reset();
        }


        private void ResetGame()
        {
            currentLevelPresetIndex = -1;
            currentPlayerHealth = DataContainer.PlayerPreset.PlayerLivesQuantity;
            StartNextLevel();
        }
        
        
        private void SwitchToTheNextLevel()
        {
            currentLevelPresetIndex++;

            GamePreset gamePreset = DataContainer.GamePreset;

            if (currentLevelPresetIndex > gamePreset.GetLevelPresets().Length - 1)
            {
                currentLevelPresetIndex = 0;
            }
            
            currentLevelPreset = gamePreset.GetLevelPreset(currentLevelPresetIndex);
        }
        
        #endregion


        
        #region Event handlers

        private void PlayerShipsManager_OnPlayerKilled()
        {
            currentPlayerHealth--;
            
            if (currentPlayerHealth < 0)
            {
                ResetGame();
            }
            else if (asteroidsManager.GetActiveAsteroidsCount() == 0 && !enemiesManager.HasActiveEnemy())
            {
                StartNextLevel();
            }
            else
            {
                playerShipsManager.RespawnPlayer(PlayerRespawnDistanceFromBorders, 2f, 1f, 2f);
            }
        }


        private void AsteroidsManager_InitialAsteroidsDestroyed(int destroyedQuantity)
        {
            if (destroyedQuantity >= currentLevelPreset.AsteroidsCount / 2)
            {
                for (int i = 0; i < currentLevelPreset.EnemiesCount; i++)
                {
                    enemiesManager.SpawnEnemy(playerShipsManager.Player);
                }
                
                asteroidsManager.OnInitialAsteroidsDestroyed -= AsteroidsManager_InitialAsteroidsDestroyed;
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
