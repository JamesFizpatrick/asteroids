using System;


namespace Asteroids.Managers
{
    public interface IInputManager : IManager, IUpdatableManager
    {
        Action<InputRotationType> OnStartRotating { get; set; }
        Action<InputRotationType> OnStopRotating { get; set; }
        Action OnStartMoving { get; set; }
        Action OnStopMoving { get; set; }
        Action OnStartFiring { get; set; }
        Action OnStopFiring { get; set; }
        Action OnSwitchWeapon { get; set; }

        void Update();
    }
}
