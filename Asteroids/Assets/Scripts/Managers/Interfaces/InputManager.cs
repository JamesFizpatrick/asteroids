using System;
using UnityEngine;


namespace Asteroids.Managers
{
    public class InputManager : IInputManager
    {
        #region Fields

        public Action<InputRotationType> OnStartRotating { get; set; }
        public Action<InputRotationType> OnStopRotating { get; set; }
        
        public Action OnStartMoving { get; set; }
        public Action OnStopMoving { get; set; }
        
        public Action OnStartFiring { get; set; }
        public Action OnStopFiring { get; set; }
        public Action OnSwitchWeapon { get; set; }
        
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
