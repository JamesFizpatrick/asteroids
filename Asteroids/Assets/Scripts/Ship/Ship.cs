using UnityEngine;


namespace Asteroids.Game
{
    public class Ship : MonoBehaviour
    {
        #region Fields

        private ShipMovementController shipMovementController;
        private ShipVisualAppearanceController shipVisualAppearanceController;
        private ShipWeaponController shipWeaponController;

        #endregion



        #region Unity lifecycle

        public void Awake() => InitControllers();

        #endregion



        #region Private methods

        private void InitControllers()
        {
            shipMovementController = GetComponent<ShipMovementController>();
            shipVisualAppearanceController = GetComponent<ShipVisualAppearanceController>();
            shipWeaponController = GetComponent<ShipWeaponController>();
        }

        #endregion
    }
}
