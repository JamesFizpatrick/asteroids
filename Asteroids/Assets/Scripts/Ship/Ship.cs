using UnityEngine;


namespace Asteroids.Game
{
    public class Ship : MonoBehaviour
    {
        private ShipMovementController shipMovementController;
        private ShipVisualAppearanceController shipVisualAppearanceController;
        private ShipWeaponController shipWeaponController;

        
        public void Awake()
        {
            InitControllers();
        }


        private void InitControllers()
        {
            shipMovementController = GetComponent<ShipMovementController>();
            shipVisualAppearanceController = GetComponent<ShipVisualAppearanceController>();
            shipWeaponController = GetComponent<ShipWeaponController>();
        }
    }
}
