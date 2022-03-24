using Asteroids.Asteroids;
using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.Managers
{
    public class GameManager : MonoBehaviour, IManager
    {
        #region Fields

        private static GameManager instance;

        private GameObject playerShip;
        
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

        public Vector3 GetPlayerShipLocalPosition() =>
            playerShip != null ? playerShip.transform.localPosition : Vector3.zero;

        
        public void Initialize() { }

        
        public void Unload() { }

        #endregion



        #region Private methods

        private void StartGame()
        {
            GameObject shipPrefab = ManagersHub.GetManager<DataManager>().PlayerPreset.Ship;
            playerShip = Instantiate(shipPrefab, GameSceneReferences.MainCanvas.transform);
            
            ManagersHub.GetManager<AsteroidsManager>()
                .SpawnAsteroids(4, GetPlayerShipLocalPosition(), 100f);
        }

        #endregion
    }
}
