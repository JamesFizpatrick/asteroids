using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Game;
using Asteroids.Handlers;
using UnityEngine;
using Random = System.Random;


namespace Asteroids.Managers
{
    public class PlayerShipsManager : IManager, IUnloadableManager
    {
        #region Fields

        public Action OnPlayerKilled;

        private const int DistanceFromBorders = 100;
        private const int FieldSegmentsGridModule = 10;
        
        public Player Player { get; private set; }
        
        private Coroutine respawnCoroutine;
        
        private SoundManager soundManager;
        private GameObjectsManager gameObjectsManager;
        private AsteroidsManager asteroidsManager;
        private Random random;

        List<FieldSegment> fieldSegments = new List<FieldSegment>();
        
        #endregion

        
        
        #region Public methods
        
        public void Initialize(IManagersHub hub)
        {
            soundManager = hub.GetManager<SoundManager>();
            gameObjectsManager = hub.GetManager<GameObjectsManager>();
            asteroidsManager = hub.GetManager<AsteroidsManager>();
            
            InitFieldSegments();

            random = new Random();
        }

       
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
            GameObject shipPrefab = DataContainer.GamePreset.Ship;
            Player = gameObjectsManager.CreatePlayerShip(shipPrefab).GetComponent<Player>();
            Player.Killed += Player_Killed;
        }


        public void RespawnPlayer(float preDelay, float respawnDelay, float iFramesDelay)
        {
            // Respawn player and give them a coupe of invincibility frames
            if (respawnCoroutine != null)
            {
                CoroutinesHandler.Instance.StopCoroutine(respawnCoroutine);
            }
            
            respawnCoroutine = 
                CoroutinesHandler.Instance.StartCoroutine(RespawnCoroutine(preDelay, respawnDelay, iFramesDelay));
        }
        
        
        public void RespawnPlayer()
        {
            ResetPossibleCoordinatesDictionary();

            List<AsteroidsManager.SpawnAsteroidData> asteroidsData = asteroidsManager.GetActiveAsteroidsData();

            BlockCoordinateSegments(asteroidsData);
            
            FieldSegment segment = GetRandomOpenSegment();
            Vector2Int intCoordinates = segment?.GetRandomCoordinate() ?? Vector2Int.zero;
            Vector3 newCoordinates = new Vector3(intCoordinates.x, intCoordinates.y);
            
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

        private IEnumerator RespawnCoroutine(float preDelay,
            float respawnDelay, float iFramesDelay)
        {
            Player.EnableIFrames(false);

            yield return new WaitForSeconds(preDelay);
            Player.gameObject.SetActive(false);

            yield return new WaitForSeconds(respawnDelay);
            RespawnPlayer();
            Player.EnableIFrames(true);

            yield return new WaitForSeconds(iFramesDelay);
            Player.DisableIFrames();
        }


        // Methods for dividing the field into several segments to find coordinates for safe respawn 
        private void InitFieldSegments()
        {
            int minX = -Screen.width / 2 + DistanceFromBorders;
            int minY = -Screen.height / 2 + DistanceFromBorders;
            int maxX = Screen.width / 2 - DistanceFromBorders;
            int maxY = Screen.height / 2 - DistanceFromBorders;

            int xStep = Mathf.Abs(maxX - minX) / FieldSegmentsGridModule;
            int yStep = Mathf.Abs(maxY - minY) / FieldSegmentsGridModule;

            int x = minX;
            int y = minY;
            
            while (x <= maxX)
            {
                while (y <= maxY)
                {
                    Vector2Int xRange = new Vector2Int(x, x + xStep);
                    Vector2Int yRange = new Vector2Int(y, y + yStep);;
                    
                    FieldSegment segment = new FieldSegment(xRange, yRange, false);
                    
                    fieldSegments.Add(segment);

                    y += yStep;
                }

                x += xStep;
            }
        }
        
        
        private void ResetPossibleCoordinatesDictionary()
        {
            foreach (FieldSegment segment in fieldSegments)
            {
                segment.Unblock();
            }
        }


        private void BlockCoordinateSegments(List<AsteroidsManager.SpawnAsteroidData> asteroidsData)
        {
            foreach (AsteroidsManager.SpawnAsteroidData data in asteroidsData)
            {
                int minX = data.LocalPosition.x - data.ColliderSize.x / 2;
                int minY = data.LocalPosition.y - data.ColliderSize.y / 2;
                int maxX = data.LocalPosition.x + data.ColliderSize.x / 2;
                int maxY = data.LocalPosition.y + data.ColliderSize.y / 2;
                
                Vector2Int minRange = new Vector2Int(minX, minY);
                Vector2Int maxRange = new Vector2Int(maxX, maxY);
                
                foreach (FieldSegment fieldSegment in fieldSegments)
                {
                    if (fieldSegment.Contains(minRange, maxRange))
                    {
                        fieldSegment.Block();
                    }
                }
            }
        }


        private FieldSegment GetRandomOpenSegment()
        {
            List<FieldSegment> openSegments = fieldSegments.Where(x => !x.Blocked).ToList();
            int index = random.Next(0, openSegments.Count);
            return openSegments[index];
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
