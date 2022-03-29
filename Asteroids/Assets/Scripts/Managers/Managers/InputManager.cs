using System;
using UnityEngine;


namespace Asteroids.Managers
{
    public class InputManager : BaseManager<InputManager>
    {
        #region Fields

        public Action<InputRotationType> OnStartRotating;
        public Action<InputRotationType> OnStopRotating;
        public Action<InputMovementType> OnStartMoving;
        public Action<InputMovementType> OnStopMoving;
        public Action OnStartFiring;
        public Action OnStopFiring;
        public Action OnSwitchWeapon;
        
        #endregion

        
        
        #region Unity lifecycle

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                OnStartMoving?.Invoke(InputMovementType.Forward);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                OnStartRotating?.Invoke(InputRotationType.CounterClockwise);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                OnStartMoving?.Invoke(InputMovementType.Backward);
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
                OnStopMoving?.Invoke(InputMovementType.Forward);
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                OnStopRotating?.Invoke(InputRotationType.CounterClockwise);
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                OnStopMoving?.Invoke(InputMovementType.Backward);
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


        
        #region Protected methods

        protected override void Initialize() { }

        protected override void Deinitialize() { }

        #endregion
    }
}
