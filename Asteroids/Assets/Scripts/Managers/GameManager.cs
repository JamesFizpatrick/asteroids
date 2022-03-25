using UnityEngine;


namespace Asteroids.Managers
{
    public class GameManager : MonoBehaviour, IManager
    {
        #region Fields

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
            
            asteroidsManager.SpawnAsteroids(4, playerShipsManager.Player.transform.localPosition, 100f);
            asteroidsManager.OnHalfDestroyed += AsteroidsManager_OnHalfDestroyed;
            
            enemiesManager.OnEnemyKilled += EnemiesManager_OnEnemyKilled;
        }

        #endregion


        
        #region Event handlers

        private void PlayerShipsManager_OnPlayerKilled() { }


        private void AsteroidsManager_OnHalfDestroyed()
        {
            enemiesManager.SpawnEnemy(playerShipsManager.Player);
        }


        private void EnemiesManager_OnEnemyKilled() { }
        
        #endregion
    }
}
