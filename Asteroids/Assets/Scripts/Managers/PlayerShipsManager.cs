using System;
using Asteroids.Game;
using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.Managers
{
    public class PlayerShipsManager : MonoBehaviour, IManager
    {
        #region Fields

        public Action OnPlayerKilled;
        public Player Player { get; private set; }

        private static PlayerShipsManager instance;

        private AsteroidsManager asteroidsManager;
        
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

        public void Initialize() => asteroidsManager = ManagersHub.GetManager<AsteroidsManager>();


        public void Unload()
        {
            if (Player)
            {
                Player.Killed -= Player_Killed;
            }
        }


        public void SpawnPlayer()
        {
            GameObject shipPrefab = ManagersHub.GetManager<DataManager>().PlayerPreset.Ship;
            Player = Instantiate(shipPrefab, GameSceneReferences.MainCanvas.transform).GetComponent<Player>();
            Player.Killed += Player_Killed;
        }


        public void RespawnPlayer(int distanceFromBorders)
        {
            if (Player.gameObject.activeSelf)
            {
                Player.gameObject.SetActive(false);
            }

            int minX = -Screen.width / 2 + distanceFromBorders;
            int maxX = Screen.width / 2 - distanceFromBorders;
            int minY = -Screen.height / 2 + distanceFromBorders;
            int maxY = Screen.height / 2 - distanceFromBorders;
            
            System.Random random = new System.Random();

            int x = random.Next(minX, maxX);
            int y = random.Next(minY, maxY);

            Player.transform.localPosition = new Vector3(x, y);
            Player.gameObject.SetActive(true);
        }


        public void Reset()
        {
            Player.Killed -= Player_Killed;
            Destroy(Player.gameObject);
        }
        
        #endregion


        
        #region Event handlers

        private void Player_Killed() => OnPlayerKilled?.Invoke();

        #endregion
    }
}
