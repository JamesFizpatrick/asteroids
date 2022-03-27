using System;
using Asteroids.Game;
using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.UFO
{
    public class UFO : MonoBehaviour
    {
        #region Fields

        public Action Killed;
        
        private UFOMoveController moveController;
        private UFOWeaponController weaponController;

        private int weaponLayerMask;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            weaponLayerMask = LayerMasksHandler.PlayerProjectiles;

            moveController = GetComponent<UFOMoveController>();
            weaponController = GetComponent<UFOWeaponController>();
        }
        
        
        private void OnTriggerEnter2D(Collider2D col) => ProcessOnTriggerEnter(col);

        #endregion



        #region Public methods

        public void Initialze(Player player)
        {
            moveController.Initialize(player);
            weaponController.Initialize(player);
            
            weaponController.StartFire();
        }

        #endregion

        
        
        #region Event handlers

        private void ProcessOnTriggerEnter(Collider2D col)
        {
            if (col.gameObject.layer == weaponLayerMask)
            {
                Killed?.Invoke();
                gameObject.SetActive(false);
            }
        }

        #endregion
    }
}
