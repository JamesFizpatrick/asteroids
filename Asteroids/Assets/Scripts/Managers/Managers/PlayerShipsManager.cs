using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private AsteroidsManager asteroidsManager;
        private System.Random random;

        #endregion

        
        
        #region Public methods
        
        public void Initialize(ManagersHub hub)
        {
            soundManager = hub.GetManager<SoundManager>();
            gameObjectsManager = hub.GetManager<GameObjectsManager>();
            asteroidsManager = hub.GetManager<AsteroidsManager>();
            
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
            List<AsteroidsManager.SpawnAsteroidData> asteroidsData = asteroidsManager.GetActiveAsteroidsData();
            
            int minX = -Screen.width / 2 + distanceFromBorders;
            int maxX = Screen.width / 2 - distanceFromBorders;
            int minY = -Screen.height / 2 + distanceFromBorders;
            int maxY = Screen.height / 2 - distanceFromBorders;
            
            List<int> possibleXCoordinates = Enumerable.Range(minX, Screen.width - distanceFromBorders * 2).ToList();
            List<int> possibleYCoordinates = Enumerable.Range(minY, Screen.height - distanceFromBorders * 2).ToList();

            Dictionary<int, List<int>> possibleCoordinates = new Dictionary<int, List<int>>();
            
            foreach (int x in possibleXCoordinates)
            {
                possibleCoordinates.Add(x, possibleYCoordinates);
            }
            
            foreach (AsteroidsManager.SpawnAsteroidData data in asteroidsData)
            {
                int asteroidMinX = (int)(data.LocalPosition.x - data.ColliderSize.x / 2);
                int asteroidMaxX = (int)(asteroidMinX + data.ColliderSize.x);
                
                int asteroidMinY = (int)(data.LocalPosition.y - data.ColliderSize.y / 2);
                int asteroidMaxY = (int)(asteroidMinY + data.ColliderSize.y);

                for (int x = asteroidMinX; x <= asteroidMaxX; x++)
                {
                    if (possibleCoordinates.TryGetValue(x, out List<int> yCoordinates))
                    {
                        for (int y = asteroidMinY; y <= asteroidMaxY; y++)
                        {
                            if (yCoordinates.Contains(y))
                            {
                                yCoordinates.Remove(y);
                            }
                        }

                        possibleCoordinates.Remove(x);
                        possibleCoordinates.Add(x, yCoordinates);
                    }
                }
            }
            
            KeyValuePair<int, List<int>> pair = possibleCoordinates.ElementAt(random.Next(0, possibleCoordinates.Keys.Count));

            int newX = pair.Key;
            int newY = pair.Value.ElementAt(random.Next(0, pair.Value.Count));

            Vector3 newCoordinates = new Vector3(newX, newY);
            
            Player.transform.localPosition = newCoordinates;
            Player.gameObject.SetActive(true);
        }


        public void Reset()
        {
            if (respawnCoroutine != null)
            {
                CoroutinesHandler.Instance.StopCoroutine(respawnCoroutine);
            }

            if (Player)
            {
                Player.Killed -= Player_Killed;
                UnityEngine.Object.Destroy(Player.gameObject);
            }
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
