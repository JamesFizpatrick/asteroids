using System;
using Asteroids.Game;
using Asteroids.Handlers;
using Asteroids.Managers;
using Asteroids.VFX;
using UnityEngine;


namespace Asteroids.UFO
{
    [RequireComponent(typeof(UFOMoveController))]
    [RequireComponent(typeof(UFOWeaponController))]

    public class UFO : MonoBehaviour
    {
        #region Fields

        public Action Killed;
        
        private UFOMoveController moveController;
        private UFOWeaponController weaponController;

        private SoundManager soundManager;
        private VFXManager vfxManager;
        
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

        public void Initialize(Player player, IManagersHub hub)
        {
            soundManager = hub.GetManager<SoundManager>();
            vfxManager = hub.GetManager<VFXManager>();
            
            moveController.Initialize(player);
            weaponController.Initialize(player);
            
            weaponController.Fire();
        }

        #endregion

        
        
        #region Event handlers

        private void ProcessOnTriggerEnter(Collider2D col)
        {
            if (col.gameObject.layer == weaponLayerMask)
            {
                Killed?.Invoke();
                gameObject.SetActive(false);
                
                soundManager.PlaySound(SoundType.Explosion);
                vfxManager.SpawnVFX(VFXType.Explosion, transform.localPosition);
            }
        }

        #endregion
    }
}
