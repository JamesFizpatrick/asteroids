using Asteroids.Managers;


namespace Asteroids.Game
{
    public class ShipWeaponController : UnitWeaponController
    {
        #region Fields

        private InputManager inputManager;

        #endregion
        
        
        
        #region Unity lifecycle

        protected override void Awake()
        {
            currentWeaponType = WeaponType.Player;
            inputManager = ManagersHub.Instance.GetManager<InputManager>();

            base.Awake();
        }
        

        private void OnEnable()
        {
            SubscribeToFireInputs();
            inputManager.OnSwitchWeapon += InputManager_OnSwitchWeapon;
        }

        
        private void OnDisable()
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
