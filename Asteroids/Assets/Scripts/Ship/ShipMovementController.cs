using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Game
{ 
    public class ShipMovementController : MonoBehaviour
    {
        #region Fields
    
        private const float MoveSpeed = 1f;
        private const float RotationSpeed = 5f;
        private const float MaxInertia = 1f;
        private const float InertiaIncreaseSpeed = 0.01f;
        private const float InertiaDecreaseSpeed = 0.01f;
        
        private float currentInertia;
        private float currentRotationAngle;

        private Vector3 currentMoveDirection;
        
        private InputManager.RotationType currentRotationType = InputManager.RotationType.None;
        private InputManager.MovementType currentMoveType = InputManager.MovementType.None;

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
            }

            if (currentRotationType != InputManager.RotationType.None)
            {
                transform.Rotate(-Vector3.back, currentRotationAngle);
            }
        }
        
        #endregion

        
        
        #region Private methods

        private void StartRotating(InputManager.RotationType rotationType)
        {
            switch (rotationType)
            {
                case InputManager.RotationType.Clockwise:
                    currentRotationAngle = -RotationSpeed;
                    break;
                case InputManager.RotationType.CounterClockwise:
                    currentRotationAngle = RotationSpeed;
                    break;
                case InputManager.RotationType.None:
                    currentRotationAngle = 0f;
                    break;
            }
            
            currentRotationType = rotationType;
        }


        private void StopRotating() => currentRotationType = InputManager.RotationType.None;


        private void StartMoving(InputManager.MovementType movementType) => currentMoveType = movementType;


        private void StopMoving() => currentMoveType = InputManager.MovementType.None;
        
        
        private void ProcessInertia()
        {
            if (currentMoveType == InputManager.MovementType.Forward)
            {
                if (currentInertia < MaxInertia)
                {
                    currentInertia += InertiaIncreaseSpeed * 2;
                }
            }
            else if (currentMoveType == InputManager.MovementType.Backward)
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

        private void InputManager_OnStartMoving(InputManager.MovementType movementType) => StartMoving(movementType);


        private void InputManager_OnStopMoving(InputManager.MovementType movementType)
        {
            if (currentMoveType == movementType)
            {
                StopMoving();
            }
        }
        
        
        private void InputManager_OnStartRotating(InputManager.RotationType rotationType) => 
            StartRotating(rotationType);


        private void InputManager_OnStopRotating(InputManager.RotationType rotationType)
        {
            if (currentRotationType == rotationType)
            {
                StopRotating();
            }
        }
        
        #endregion
    }
}

