using System;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Game
{ 
    public class ShipMovementController : MonoBehaviour
    {
        #region Fields

        public Action<Vector3> OnPositionChanged;

        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float maxInertia = 1;
        [SerializeField] private float rotationSpeed = 5f;
        
        private const float InertiaIncreaseSpeed = 0.01f;
        private const float InertiaDecreaseSpeed = 0.01f;
        
        private float currentInertia;
        private float currentRotationAngle;

        private Vector3 currentMoveDirection;
        
        private InputRotationType currentRotationType = InputRotationType.None;
        private InputMovementType currentMoveType = InputMovementType.None;

        private InputManager inputManager;

        private bool canMove = true;
        
        #endregion


        
        #region Properties

        public Vector3 MoveDirection => currentMoveDirection;

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
            inputManager = ManagersHub.Instance.GetManager<InputManager>();
            currentMoveDirection = Vector3.up * moveSpeed;
        }
        

        private void FixedUpdate()
        {
            if (!canMove)
            {
                return;
            }
            
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


        
        #region Public methods

        public void Move() => canMove = true;
        
        
        public void Stop() => canMove = false;


        public void Reset()
        {
            currentInertia = 0;
            currentRotationAngle = 0;
            currentMoveType = InputMovementType.None;
            currentRotationType = InputRotationType.None;
        }
        
        #endregion
        
        
        
        #region Private methods

        private void StartRotating(InputRotationType rotationType)
        {
            switch (rotationType)
            {
                case InputRotationType.Clockwise:
                    currentRotationAngle = -rotationSpeed;
                    break;
                case InputRotationType.CounterClockwise:
                    currentRotationAngle = rotationSpeed;
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
                if (currentInertia < maxInertia)
                {
                    currentInertia += InertiaIncreaseSpeed * 2;
                }
            }
            else if (currentMoveType == InputMovementType.Backward)
            {
                if (currentInertia > -maxInertia)
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
