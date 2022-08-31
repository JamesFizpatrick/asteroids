using System;
using Asteroids.Game;
using Asteroids.Handlers;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.UFO
{
    public class UFO : MonoBehaviour
    {
        #region Fields

        [SerializeField] private float speed = 0.5f;
        
        public Action<UFO> Killed;
        
        private UFOMoveController moveController;
        private UFOWeaponController weaponController;
        
        private int weaponLayerMask;

        #endregion



        #region Unity lifecycle
        
        private void OnTriggerEnter2D(Collider2D col) => ProcessOnTriggerEnter(col);


        private void FixedUpdate() => moveController.Update();


        private void OnDestroy()
        {
            moveController.Dispose();
            weaponController.Dispose();
        }

        #endregion



        #region Public methods

        public void Initialize(Ship player)
        {
            weaponLayerMask = LayerMasksHandler.PlayerProjectiles;

            moveController = new UFOMoveController(gameObject, player, speed);
            
            weaponController = new UFOWeaponController(
                ManagersHub.Instance.GetManager<ISoundManager>(),
                ManagersHub.Instance.GetManager<IGameObjectsManager>(),
                gameObject, 
                player);
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
