using System;
using UnityEngine;


namespace Asteroids.Managers
{
    public class InputManager : IManager
    {
        #region Fields

        public Action<InputRotationType> OnStartRotating;
        public Action<InputRotationType> OnStopRotating;
        
        public Action OnStartMoving;
        public Action OnStopMoving;
        
        public Action OnStartFiring;
        public Action OnStopFiring;
        public Action OnSwitchWeapon;
        
        #endregion

        
        
        #region Public methods

        public void Initialize(ManagersHub hub) { }
        

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

        
        public void Unload() { }

        #endregion
    }
}
