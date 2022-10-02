using System;


namespace Asteroids.Managers
{
    public interface IInputManager : IManager, IUpdatableManager
    {
        event Action<InputRotationType> OnStartRotating;
        event Action<InputRotationType> OnStopRotating;
        event Action OnStartMoving;
        event Action OnStopMoving;
        event Action OnStartFiring;
        event Action OnStopFiring;
        event Action OnSwitchWeapon;

        void Update();
    }
}
