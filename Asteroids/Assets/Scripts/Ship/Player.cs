using System;
using UnityEngine;


namespace Asteroids.Game
{
    public class Player : MonoBehaviour
    {
        #region Fields

        public Action<Vector3> OnPositionChanged;
        public Action Killed;
        
        private ShipMovementController shipMovementController;
        private ShipVisualAppearanceController shipVisualAppearanceController;
        private ShipWeaponController shipWeaponController;

        private int enemyProjectilesLayer;
        private int asteroidsLayer;
        private int enemyLayer;
        
        #endregion



        #region Unity lifecycle

        public void Awake()
        {
            enemyProjectilesLayer = LayerMask.NameToLayer("EnemyProjectiles");
            asteroidsLayer = LayerMask.NameToLayer("Asteroid");
            enemyLayer = LayerMask.NameToLayer("Enemy");

            InitControllers();
            shipMovementController.OnPositionChanged += ShipMovementController_OnPositionChanged;
        }


        private void OnTriggerEnter2D(Collider2D col)
        {
            int layer = col.gameObject.layer;
            if (layer == enemyProjectilesLayer || layer == asteroidsLayer || layer == enemyLayer)
            {
                Killed?.Invoke();
                gameObject.SetActive(false);
            }
        }

        
        public void OnDestroy()
        {
            shipMovementController.OnPositionChanged -= ShipMovementController_OnPositionChanged;
        }

        #endregion



        #region Private methods

        private void InitControllers()
        {
            shipMovementController = GetComponent<ShipMovementController>();
            shipVisualAppearanceController = GetComponent<ShipVisualAppearanceController>();
            shipWeaponController = GetComponent<ShipWeaponController>();
        }

        #endregion


        
        #region Event handlers

        private void ShipMovementController_OnPositionChanged(Vector3 position)
        {
            OnPositionChanged?.Invoke(position);
        }

        #endregion
    }
}
