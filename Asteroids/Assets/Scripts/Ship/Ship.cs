using System;
using UnityEngine;


namespace Asteroids.Game
{
    public class Ship : MonoBehaviour
    {
        #region Fields

        public Action<Vector3> OnPositionChanged;
        
        private ShipMovementController shipMovementController;
        private ShipVisualAppearanceController shipVisualAppearanceController;
        private ShipWeaponController shipWeaponController;

        #endregion



        #region Unity lifecycle

        public void Awake()
        {
            InitControllers();
            shipMovementController.OnPositionChanged += ShipMovementController_OnPositionChanged;
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
