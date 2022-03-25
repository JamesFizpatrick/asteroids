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
            inputManager = ManagersHub.GetManager<InputManager>();

            base.Awake();
        }
        

        private void OnEnable()
        {
            inputManager.OnStartFiring += InputManager_OnStartFiring;
            inputManager.OnStopFiring += InputManager_OnStopFiring;
        }

        
        private void OnDisable()
        {
            inputManager.OnStartFiring -= InputManager_OnStartFiring;
            inputManager.OnStopFiring -= InputManager_OnStopFiring;
        }
        
        #endregion



        #region Private methods
        
        private void InputManager_OnStartFiring() => StartFire();


        private void InputManager_OnStopFiring() => StopFire();

        #endregion
    }
}
