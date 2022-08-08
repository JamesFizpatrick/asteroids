using System;
using Asteroids.Handlers;
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
                
        private float currentInertia;
        private float currentRotationAngle;

        private Vector3 currentAimDirection;
        private Vector3 currentMoveDirection;
        
        private InputRotationType currentRotationType = InputRotationType.None;
        private InputMovementType currentMoveType = InputMovementType.None;
        
        private InputManager inputManager;

        private bool canMove = true;
        
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


        private void StartMoving() => currentMoveType = InputMovementType.Forward;


        private void StopMoving() => currentMoveType = InputMovementType.None;


        private void ProcessInertia()
        {
            if (currentMoveType == InputMovementType.Forward)
            {
                if (currentInertia < maxInertia)
                {
                    currentInertia += PlayerConstants.InertiaIncreaseSpeed * 2;
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
                    currentInertia -= PlayerConstants.InertiaDecreaseSpeed;
                }
            }
        }
        
        #endregion
    
    
        
        #region Event handlers

        private void InputManager_OnStartMoving() => StartMoving();


        private void InputManager_OnStopMoving() => StopMoving();
        
        
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
