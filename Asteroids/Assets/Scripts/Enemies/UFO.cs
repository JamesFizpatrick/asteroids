using Asteroids.Game;
using UnityEngine;


namespace Asteroids.UFO
{
    public class UFO : MonoBehaviour
    {
        #region Fields

        private UFOMoveController moveController;
        private UFOWeaponController weaponController;

        private int weaponLayerMask;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            weaponLayerMask = LayerMask.NameToLayer("PlayerProjectiles");

            moveController = GetComponent<UFOMoveController>();
            weaponController = GetComponent<UFOWeaponController>();
        }
        
        
        private void OnTriggerEnter2D(Collider2D col) => ProcessOnTriggerEnter(col);

        #endregion



        #region Public methods

        public void Initialze(Ship ship)
        {
            moveController.Initialize(ship);
            weaponController.StartFire();
        }

        #endregion

        
        
        #region Event handlers

        private void ProcessOnTriggerEnter(Collider2D col)
        {
            if (col.gameObject.layer == weaponLayerMask)
            {
                gameObject.SetActive(false);
            }
        }

        #endregion
    }
}
