using System;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Game
{ 
    public class ShipMovementController : MonoBehaviour
    {
        #region Fields

        public Action<Vector3> OnPositionChanged;
        
        private const float MoveSpeed = 3f;
        private const float RotationSpeed = 5f;
        private const float MaxInertia = 1f;
        private const float InertiaIncreaseSpeed = 0.01f;
        private const float InertiaDecreaseSpeed = 0.01f;
        
        private float currentInertia;
        private float currentRotationAngle;

        private Vector3 currentMoveDirection;
        
        private InputRotationType currentRotationType = InputRotationType.None;
        private InputMovementType currentMoveType = InputMovementType.None;

        private InputManager inputManager;
        
        #endregion
    
    
        
        #region Unity lifecycle
        
        private void OnEnable()
        {
            inputManager.OnStartMoving += InputManager_OnStartMoving;
            inputManager.OnStopMoving += InputManager_OnStopMoving;
            
            inputManager.OnStartRotating += InputManager_OnStartRotating;
            inputManager.OnStopRotating += InputManager_OnStopRotating;
        }

        
        private void OnDisable()
        {
            inputManager.OnStartMoving -= InputManager_OnStartMoving;
            inputManager.OnStopMoving -= InputManager_OnStopMoving;
            
            inputManager.OnStartRotating -= InputManager_OnStartRotating;
            inputManager.OnStopRotating -= InputManager_OnStopRotating;
        }


        private void Awake()
        {
            inputManager = ManagersHub.GetManager<InputManager>();
            currentMoveDirection = Vector3.up * MoveSpeed;
        }
        

        private void FixedUpdate()
        {
            ProcessInertia();

            Vector3 translateAmount = currentMoveDirection * currentInertia;
            if (translateAmount != Vector3.zero)
            {
                transform.Translate(translateAmount);
                OnPositionChanged?.Invoke(transform.localPosition);
            }

            if (currentRotationType != InputRotationType.None)
            {
                transform.Rotate(-Vector3.back, currentRotationAngle);
            }
        }
        
        #endregion

        
        
        #region Private methods

        private void StartRotating(InputRotationType rotationType)
        {
            switch (rotationType)
            {
                case InputRotationType.Clockwise:
                    currentRotationAngle = -RotationSpeed;
                    break;
                case InputRotationType.CounterClockwise:
                    currentRotationAngle = RotationSpeed;
                    break;
                case InputRotationType.None:
                    currentRotationAngle = 0f;
                    break;
            }
            
            currentRotationType = rotationType;
        }


        private void StopRotating() => currentRotationType = InputRotationType.None;


        private void StartMoving(InputMovementType movementType) => currentMoveType = movementType;


        private void StopMoving() => currentMoveType = InputMovementType.None;
        
        
        private void ProcessInertia()
        {
            if (currentMoveType == InputMovementType.Forward)
            {
                if (currentInertia < MaxInertia)
                {
                    currentInertia += InertiaIncreaseSpeed * 2;
                }
            }
            else if (currentMoveType == InputMovementType.Backward)
            {
                if (currentInertia > -MaxInertia)
                {
                    currentInertia -= InertiaIncreaseSpeed * 2;
                }
            }
            else
            {
                if (currentInertia == 0f)
                {
                    return;
                }
                
                if (currentInertia > 0f)
                {
                    currentInertia -= InertiaDecreaseSpeed;
                }
                else
                {
                    currentInertia += InertiaDecreaseSpeed;
                }
            }
        }
        
        #endregion
    
    
        
        #region Event handlers

        private void InputManager_OnStartMoving(InputMovementType movementType) => StartMoving(movementType);


        private void InputManager_OnStopMoving(InputMovementType movementType)
        {
            if (currentMoveType == movementType)
            {
                StopMoving();
            }
        }
        
        
        private void InputManager_OnStartRotating(InputRotationType rotationType) => StartRotating(rotationType);


        private void InputManager_OnStopRotating(InputRotationType rotationType)
        {
            if (currentRotationType == rotationType)
            {
                StopRotating();
            }
        }
        
        #endregion
    }
}
