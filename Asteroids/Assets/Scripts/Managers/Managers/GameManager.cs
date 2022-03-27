using UnityEngine;


namespace Asteroids.Managers
{
    public class GameManager : MonoBehaviour, IManager
    {
        #region Fields

        private const int InitialAsteroidsQuantity = 4;
        private const int PlayerRespawnDistanceFromBorders = 100;
        private const float InitialPlayerSafeRadius = 100f;
        
        private static GameManager instance;

        private PlayerShipsManager playerShipsManager;
        private EnemiesManager enemiesManager;
        private AsteroidsManager asteroidsManager;
        
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

            playerShipsManager.SpawnPlayer();
            playerShipsManager.OnPlayerKilled += PlayerShipsManager_OnPlayerKilled;
            
            asteroidsManager.SpawnAsteroids(InitialAsteroidsQuantity,
                playerShipsManager.Player.transform.localPosition,
                InitialPlayerSafeRadius);
            asteroidsManager.OnInitialAsteroidsDestroyed += AsteroidsManager_InitialAsteroidsDestroyed;
            asteroidsManager.OnAllAsteroidsDestroyed += AsteroidsManager_OnAllAsteroidsDestroyed;

            enemiesManager.OnEnemyKilled += EnemiesManager_OnEnemyKilled;
        }


        private void ResetGame()
        {
            playerShipsManager.Reset();
            playerShipsManager.SpawnPlayer();
            
            asteroidsManager.OnInitialAsteroidsDestroyed -= AsteroidsManager_InitialAsteroidsDestroyed;
            asteroidsManager.OnAllAsteroidsDestroyed -= AsteroidsManager_OnAllAsteroidsDestroyed;
            
            asteroidsManager.Reset();
            asteroidsManager.SpawnAsteroids(InitialAsteroidsQuantity,
                playerShipsManager.Player.transform.localPosition,
                InitialPlayerSafeRadius);
            
            asteroidsManager.OnInitialAsteroidsDestroyed += AsteroidsManager_InitialAsteroidsDestroyed;
            asteroidsManager.OnAllAsteroidsDestroyed += AsteroidsManager_OnAllAsteroidsDestroyed;

            enemiesManager.Reset();
        }
        
        #endregion


        
        #region Event handlers

        private void PlayerShipsManager_OnPlayerKilled()
        {
            if (asteroidsManager.GetActiveAsteroidsCount() == 0 && !enemiesManager.HasActiveEnemy())
            {
                ResetGame();
            }
            else
            {
                playerShipsManager.RespawnPlayer(PlayerRespawnDistanceFromBorders, 2f,
                    1f, 2f);
            }
        }


        private void AsteroidsManager_InitialAsteroidsDestroyed(int destroyedQuantity)
        {
            if (destroyedQuantity >= InitialAsteroidsQuantity / 2)
            {
                enemiesManager.SpawnEnemy(playerShipsManager.Player);
                asteroidsManager.OnInitialAsteroidsDestroyed -= AsteroidsManager_InitialAsteroidsDestroyed;
            }
        }


        private void AsteroidsManager_OnAllAsteroidsDestroyed()
        {
            if (!enemiesManager.HasActiveEnemy())
            {
                ResetGame();
            }
        }


        private void EnemiesManager_OnEnemyKilled()
        {
            if (asteroidsManager.GetActiveAsteroidsCount() == 0)
            {
                ResetGame();
            }
        }
        
        #endregion
    }
}
