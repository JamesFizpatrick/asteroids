using System;
using Asteroids.Game;
using Asteroids.Handlers;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.UFO
{
    [RequireComponent(typeof(UFOMoveController))]
    public class UFO : MonoBehaviour
    {
        #region Fields

        public Action<UFO> Killed;
        
        private UFOMoveController moveController;
        private UFOWeaponController weaponController;
        
        private int weaponLayerMask;

        #endregion



        #region Unity lifecycle
        
        private void OnTriggerEnter2D(Collider2D col) => ProcessOnTriggerEnter(col);


        private void OnDestroy() => weaponController.Dispose();

        #endregion



        #region Public methods

        public void Initialize(Ship player)
        {
            weaponLayerMask = LayerMasksHandler.PlayerProjectiles;

            moveController = GetComponent<UFOMoveController>();
            moveController.Initialize(player);
            
            weaponController = new UFOWeaponController(ManagersHub.Instance.GetManager<SoundManager>(),
                ManagersHub.Instance.GetManager<GameObjectsManager>(),
                gameObject, player);
        }

        #endregion

        
        
        #region Event handlers

        private void ProcessOnTriggerEnter(Collider2D col)
        {
            if (col.gameObject.layer == weaponLayerMask)
            {
                Killed?.Invoke(this);
                gameObject.SetActive(false);
            }
        }

        #endregion
    }
}
