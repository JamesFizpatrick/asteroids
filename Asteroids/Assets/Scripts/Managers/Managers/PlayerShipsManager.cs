using System;
using System.Collections;
using System.Collections.Generic;
using Asteroids.Game;
using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.Managers
{
    public class PlayerShipsManager : IManager, IUnloadableManager
    {
        #region Fields

        public Action OnPlayerKilled;
        public Action<int> OnPlayerHealthValueChanged;
        
        private Coroutine respawnCoroutine;

        private int currentPlayerHealth;

        private SoundManager soundManager;
        private GameObjectsManager gameObjectsManager;
        private AsteroidsManager asteroidsManager;
        private FieldSegmentsController fieldSegmentsController;

        #endregion



        #region Properties

        public Ship Player { get; private set; }

        #endregion



        #region Public methods

        public void Initialize(IManagersHub hub)
        {
            soundManager = hub.GetManager<SoundManager>();
            gameObjectsManager = hub.GetManager<GameObjectsManager>();
            asteroidsManager = hub.GetManager<AsteroidsManager>();

            fieldSegmentsController = new FieldSegmentsController();
        }

       
        public void Unload()
        {
            if (Player)
            {
                Player.OnPlayerDamaged -= Player_OnPlayerDamaged;
            }

            if (respawnCoroutine != null)
            { 
                CoroutinesHandler.Instance.StopCoroutine(respawnCoroutine);
            }
        }


        public void SpawnPlayer()
        {
            GameObject shipPrefab = DataContainer.GamePreset.Ship;
            Player = gameObjectsManager.CreatePlayerShip(shipPrefab).GetComponent<Ship>();
            Player.OnPlayerDamaged += Player_OnPlayerDamaged;

            currentPlayerHealth = DataContainer.GamePreset.PlayerLivesQuantity;
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
            List<SpawnAsteroidData> asteroidsData = asteroidsManager.GetActiveAsteroidsData();

            fieldSegmentsController.Reset();
            fieldSegmentsController.BlockSegments(asteroidsData);            
            FieldSegment segment = fieldSegmentsController.GetRandomOpenSegment();

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
                Player.OnPlayerDamaged -= Player_OnPlayerDamaged;
                UnityEngine.Object.Destroy(Player.gameObject);
            }
        }

        #endregion



        #region Private methods

        private IEnumerator RespawnCoroutine(float preDelay, float respawnDelay, float iFramesDelay)
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
                     
        #endregion

        
        
        #region Event handlers

        private void Player_OnPlayerDamaged()
        {
            currentPlayerHealth--;

            soundManager.PlaySound(SoundType.Death);

            if (currentPlayerHealth > 0)
            {
                OnPlayerHealthValueChanged?.Invoke(currentPlayerHealth);
            }
            else
            {
                OnPlayerKilled?.Invoke();
            }
        }

        #endregion
    }
}
