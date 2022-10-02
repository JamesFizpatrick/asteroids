using System;
using UnityEngine;


namespace Asteroids.Managers
{
    public class InputManager : IInputManager
    {
        #region Fields

        public event Action<InputRotationType> OnStartRotating;
        public event Action<InputRotationType> OnStopRotating;
        
        public event Action OnStartMoving;
        public event Action OnStopMoving;
        
        public event Action OnStartFiring;
        public event Action OnStopFiring;
        public event Action OnSwitchWeapon;
        
        #endregion

        
        
        #region Public methods

        public void Initialize(IManagersHub hub) { }
        

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                OnStartMoving?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                OnStartRotating?.Invoke(InputRotationType.CounterClockwise);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                OnStartRotating?.Invoke(InputRotationType.Clockwise);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnStartFiring?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnSwitchWeapon?.Invoke();
            }
            
            
            if (Input.GetKeyUp(KeyCode.W))
            {
                OnStopMoving?.Invoke();
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                OnStopRotating?.Invoke(InputRotationType.CounterClockwise);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                OnStopRotating?.Invoke(InputRotationType.Clockwise);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                OnStopFiring?.Invoke();
            }
        }
        
        #endregion
    }
}
