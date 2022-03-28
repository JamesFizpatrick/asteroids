using Asteroids.Data;
using UnityEngine;


namespace Asteroids.Managers
{
    public class GameManager : MonoBehaviour, IManager
    {
        #region Fields

        private const int PlayerRespawnDistanceFromBorders = 100;
        private const float InitialPlayerSafeRadius = 100f;
        
        private static GameManager instance;

        private PlayerShipsManager playerShipsManager;
        private EnemiesManager enemiesManager;
        private AsteroidsManager asteroidsManager;
        private DataManager dataManager;

        private int currentLevelPresetIndex = -1;
        private GamePreset.LevelPreset currentLevelPreset;

        private int currentPlayerHealth;
        
        #endregion


        
        #region Properties

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject managerGo = new GameObject("GameManager");
                    GameManager manager = managerGo.AddComponent<GameManager>();
                    instance = manager;
                }

                return instance;
            }
        }

        #endregion


        
        #region Unity lifecycle

        public void Start() => StartGame();

        #endregion



        #region Public methods
        
        public void Initialize() { }

        
        public void Unload() { }
        
        #endregion



        #region Private methods

        private void StartGame()
        {
            playerShipsManager = ManagersHub.GetManager<PlayerShipsManager>();
            asteroidsManager = ManagersHub.GetManager<AsteroidsManager>();
            enemiesManager = ManagersHub.GetManager<EnemiesManager>();
            dataManager = ManagersHub.GetManager<DataManager>();

            SwitchToTheNextLevel();

            currentPlayerHealth = dataManager.PlayerPreset.PlayerLivesQuantity;
            
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
            currentPlayerHealth = dataManager.PlayerPreset.PlayerLivesQuantity;
            StartNextLevel();
        }
        
        
        private void SwitchToTheNextLevel()
        {
            currentLevelPresetIndex++;

            GamePreset gamePreset = dataManager.GamePreset;

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
