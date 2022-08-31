using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Weapons
{
    public class ShipWeaponController : UnitWeaponController
    {
        #region Fields

        private readonly IInputManager inputManager;

        #endregion



        #region Class lifecycle

        public ShipWeaponController(ISoundManager soundManager, IGameObjectsManager gameObjectsManager, GameObject owner, IInputManager inputManager) :
            base(soundManager, gameObjectsManager, owner)
        {
            this.inputManager = inputManager;
            SetCurrentWeaponType(WeaponType.Player);
            SubscribeToFireInputs();
            inputManager.OnSwitchWeapon += InputManager_OnSwitchWeapon;
        }

        #endregion
        
        
        
        #region Protected methods
        
        protected override void ImplicitDispose()
        {
            UnsubscribeFromFireInputs();
            inputManager.OnSwitchWeapon -= InputManager_OnSwitchWeapon;
        }
        
        #endregion


        
        #region Public methods

        public void SetWeaponActivity(bool isActive)
        {
            if (isActive)
            {
                SubscribeToFireInputs();
            }
            else
            {
                UnsubscribeFromFireInputs();
            }
        }

        #endregion



        #region Private methods

        private void SubscribeToFireInputs()
        {
            inputManager.OnStartFiring += InputManager_OnStartFiring;
            inputManager.OnStopFiring += InputManager_OnStopFiring;
        }

        
        private void UnsubscribeFromFireInputs()
        {
            StopFire();
            
            inputManager.OnStartFiring -= InputManager_OnStartFiring;
            inputManager.OnStopFiring -= InputManager_OnStopFiring;
        }

        #endregion
        
        

        #region Event handlers
        
        private void InputManager_OnStartFiring() => StartFire();


        private void InputManager_OnStopFiring() => StopFire();

        
        private void InputManager_OnSwitchWeapon() => SwitchWeaponType();

        #endregion
    }
}
