using System;
using System.Collections;
using Asteroids.Game;
using UnityEngine;


namespace Asteroids.Managers
{
    public class PlayerShipsManager : IManager
    {
        #region Fields

        public Action OnPlayerKilled;
        
        public Player Player { get; private set; }
        
        private Coroutine respawnCoroutine;
        
        private SoundManager soundManager;
        private GameObjectsManager gameObjectsManager;
        private System.Random random;

        #endregion

        
        
        #region Public methods
        
        public void Initialize(ManagersHub hub)
        {
            soundManager = hub.GetManager<SoundManager>();
            gameObjectsManager = hub.GetManager<GameObjectsManager>();
            
            random = new System.Random();
        }

        
        public void Update() { }


        public void Unload()
        {
            if (Player)
            {
                Player.Killed -= Player_Killed;
            }

            if (respawnCoroutine != null)
            { 
                CoroutinesHandler.Instance.StopCoroutine(respawnCoroutine);
            }
        }


        public void SpawnPlayer()
        {
            GameObject shipPrefab = DataContainer.PlayerPreset.Ship;
            Player = gameObjectsManager.CreatePlayerShip(shipPrefab).GetComponent<Player>();
            Player.Killed += Player_Killed;
        }


        public void RespawnPlayer(int distanceFromBorders, float preDelay, float respawnDelay, float iFramesDelay)
        {
            // Respawn player and give them a coupe of invincibility frames
            if (respawnCoroutine != null)
            {
                CoroutinesHandler.Instance.StopCoroutine(respawnCoroutine);
            }
            CoroutinesHandler.Instance.StartCoroutine(RespawnCoroutine(distanceFromBorders, preDelay, respawnDelay, iFramesDelay));
        }
        
        
        public void RespawnPlayer(int distanceFromBorders)
        {
            int minX = -Screen.width / 2 + distanceFromBorders;
            int maxX = Screen.width / 2 - distanceFromBorders;
            int minY = -Screen.height / 2 + distanceFromBorders;
            int maxY = Screen.height / 2 - distanceFromBorders;
            
            int x = random.Next(minX, maxX);
            int y = random.Next(minY, maxY);

            Player.transform.localPosition = new Vector3(x, y);
            Player.gameObject.SetActive(true);
        }


        public void Reset()
        {
            if (respawnCoroutine != null)
            {
                CoroutinesHandler.Instance.StopCoroutine(respawnCoroutine);
            }
            
            Player.Killed -= Player_Killed;
            UnityEngine.Object.Destroy(Player.gameObject);
        }
        
        #endregion


        
        #region Private methods

        private IEnumerator RespawnCoroutine(int distanceFromBorders, float preDelay,
            float respawnDelay, float iFramesDelay)
        {
            Player.EnableIFrames(false);

            yield return new WaitForSeconds(preDelay);
            Player.gameObject.SetActive(false);

            yield return new WaitForSeconds(respawnDelay);
            RespawnPlayer(distanceFromBorders);
            Player.EnableIFrames(true);

            yield return new WaitForSeconds(iFramesDelay);
            Player.DisableIFrames();
        }

        #endregion

        
        
        #region Event handlers

        private void Player_Killed()
        {
            soundManager.PlaySound(SoundType.Death);
            OnPlayerKilled?.Invoke();
        }

        #endregion
    }
}
