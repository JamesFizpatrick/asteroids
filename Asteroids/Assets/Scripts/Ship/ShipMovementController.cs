using System;
using Asteroids.Managers;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


namespace Asteroids.Game
{ 
    public class ShipMovementController : MonoBehaviour
    {
        #region Fields

        public Action<Vector3> OnPositionChanged;

        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float maxInertia = 1;
        [SerializeField] private float rotationSpeed = 5f;
        
        private const float InertiaIncreaseSpeed = 0.02f;
        private const float InertiaDecreaseSpeed = 0.005f;
        
        private float currentInertia;
        private float currentRotationAngle;

        private Vector3 currentAimDirection;
        private Vector3 currentMoveDirection;
        
        private InputRotationType currentRotationType = InputRotationType.None;
        private InputMovementType currentMoveType = InputMovementType.None;

        private InputMovementType previousMovementType = InputMovementType.None;

        private InputManager inputManager;

        private bool canMove = true;
        
        #endregion


        
        #region Properties

        public Vector3 AimDirection => currentAimDirection;

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
            currentAimDirection = Vector3.up;
        }
        

        private void FixedUpdate()
        {
            if (!canMove)
            {
                return;
            }
 
            if (currentRotationType != InputRotationType.None)
            {
                transform.Rotate(-Vector3.back, currentRotationAngle);
            }
            
            ProcessInertia();
            
            float speed = moveSpeed * currentInertia;
            
            Vector3 translateAmount;
            if (currentMoveType == InputMovementType.None)
            {
                translateAmount = currentMoveDirection * speed;
            }
            else
            {
                translateAmount =
                    (transform.TransformDirection(currentAimDirection) + currentMoveDirection).normalized * speed;
            }

            Vector3 prevPos = transform.position;
            
            if (translateAmount != Vector3.zero)
            {
                transform.Translate(translateAmount, Space.World);
                OnPositionChanged?.Invoke(transform.localPosition);
            }
            
            currentMoveDirection = (transform.position - prevPos).normalized;
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


        private void StartMoving(InputMovementType movementType)
        {
            if (movementType == InputMovementType.Forward)
            {
                currentAimDirection = Vector3.up;
            }
            else if (movementType == InputMovementType.Backward)
            {
                currentAimDirection = -Vector3.up;
            }

            if (previousMovementType != movementType)
            {
                currentInertia = -currentInertia;
            }
            
            currentMoveType = movementType;
        }


        private void StopMoving()
        {
            previousMovementType = currentMoveType;
            currentMoveType = InputMovementType.None;
        }


        private void ProcessInertia()
        {
            if (currentMoveType == InputMovementType.Forward || currentMoveType == InputMovementType.Backward)
            {
                if (currentInertia < maxInertia)
                {
                    currentInertia += InertiaIncreaseSpeed * 2;
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
