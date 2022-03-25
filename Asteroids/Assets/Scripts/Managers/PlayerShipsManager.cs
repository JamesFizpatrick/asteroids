using Asteroids.Game;
using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.Managers
{
    public class PlayerShipsManager : MonoBehaviour, IManager
    {
        #region Fields

        public Ship Player { get; private set; }

        private static PlayerShipsManager instance;
        
        #endregion


        
        #region Properties

        public static PlayerShipsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject managerGo = new GameObject("PlayerShipsManager");
                    PlayerShipsManager manager = managerGo.AddComponent<PlayerShipsManager>();
                    instance = manager;
                }

                return instance;
            }
        }

        #endregion


        
        #region Public methods

        public void Initialize() { }

        
        public void Unload() { }


        public void SpawnPlayer()
        {
            GameObject shipPrefab = ManagersHub.GetManager<DataManager>().PlayerPreset.Ship;
            Player = Instantiate(shipPrefab, GameSceneReferences.MainCanvas.transform).GetComponent<Ship>();
        }

        #endregion
    }
}
