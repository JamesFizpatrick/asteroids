using System;
using System.Collections;
using System.Collections.Generic;
using Asteroids.Data;
using Asteroids.Handlers;
using Asteroids.Ships;
using UnityEngine;


namespace Asteroids.Managers
{
    public class PlayerShipsManager : IPlayerShipsManager
    {
        #region Fields

        public Action OnPlayerKilled { get; set; }
        public Action<int> OnPlayerHealthValueChanged { get; set; }
        
        private Coroutine respawnCoroutine;

        private int currentPlayerHealth;

        private ISoundManager soundManager;
        private IGameObjectsManager gameObjectsManager;
        private IAsteroidsManager asteroidsManager;
        private FieldSegmentsController fieldSegmentsController;
        private Ship player;

        #endregion



        #region Properties

        

        #endregion



        #region Public methods

        public void Initialize(IManagersHub hub)
        {
            soundManager = hub.GetManager<ISoundManager>();
            gameObjectsManager = hub.GetManager<IGameObjectsManager>();
            asteroidsManager = hub.GetManager<IAsteroidsManager>();

            fieldSegmentsController = new FieldSegmentsController();
        }

       
        public void Unload()
        {
            if (player)
            {
                player.OnPlayerDamaged -= Player_OnPlayerDamaged;
            }

            if (respawnCoroutine != null)
            { 
                CoroutinesHandler.Instance.StopCoroutine(respawnCoroutine);
            }
        }


        public void SpawnPlayer()
        {
            player = gameObjectsManager.CreatePlayerShip().GetComponent<Ship>();
            player.OnPlayerDamaged += Player_OnPlayerDamaged;

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
            
            player.transform.localPosition = newCoordinates;
            player.gameObject.SetActive(true);
        }


        public void Reset()
        {
            if (respawnCoroutine != null)
            {
                CoroutinesHandler.Instance.StopCoroutine(respawnCoroutine);
            }

            if (player)
            {
                player.OnPlayerDamaged -= Player_OnPlayerDamaged;
                UnityEngine.Object.Destroy(player.gameObject);
            }
        }


        public Ship GetPlayer() => player;

        #endregion



        #region Private methods

        private IEnumerator RespawnCoroutine(float preDelay, float respawnDelay, float iFramesDelay)
        {
            player.EnableIFrames(false);
            player.SetWeaponActivity(false);

            yield return new WaitForSeconds(preDelay);
            
            player.gameObject.SetActive(false);

            yield return new WaitForSeconds(respawnDelay);
            
            RespawnPlayer();
            player.EnableIFrames(true);
            player.SetWeaponActivity(true);

            yield return new WaitForSeconds(iFramesDelay);
            
            player.DisableIFrames();
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
