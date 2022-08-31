using System;
using Asteroids.Handlers;
using Asteroids.Managers;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


namespace Asteroids.Ships
{ 
    public class ShipMovementController
    {
        #region Fields

        public Action<Vector3> OnPositionChanged;

        private readonly IInputManager inputManager;
        private readonly GameObject owner;
        private readonly float moveSpeed;
        private readonly float maxInertia;
        private readonly float rotationSpeed;
        
        private float currentInertia;
        private float currentRotationAngle;

        private Vector3 currentAimDirection;
        private Vector3 currentMoveDirection;
        
        private InputRotationType currentRotationType = InputRotationType.None;
        private InputMovementType currentMoveType = InputMovementType.None;
        
        private bool canMove = true;
        
        #endregion


        
        #region Class lifecycle

        public ShipMovementController(IInputManager inputManager, GameObject owner, float moveSpeed, float maxInertia,
            float rotationSpeed)
        {
            this.inputManager = inputManager;
            this.owner = owner;
            this.moveSpeed = moveSpeed;
            this.maxInertia = maxInertia;
            this.rotationSpeed = rotationSpeed;
            
            currentAimDirection = Vector3.up;

            Subscribe();
        }

        #endregion
        
        
        
        #region Public methods

        public void Update()
        {
            if (!canMove)
            {
                return;
            }

            ProcessRotation();                        
            ProcessInertia();
            ProcessMovement();
        }

        
        public void Dispose() => Unsubscribe();


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


        private void ProcessRotation()
        {
            if (currentRotationType != InputRotationType.None)
            {
                owner.transform.Rotate(-Vector3.back, currentRotationAngle);
            }
        }


        private void ProcessMovement()
        {
            float speed = moveSpeed * currentInertia;

            Vector3 translateAmount;

            if (currentMoveType == InputMovementType.None)
            {
                translateAmount = currentMoveDirection * speed;
            }
            else
            {
                translateAmount = (owner.transform.TransformDirection(currentAimDirection) + currentMoveDirection).normalized * speed;
            }

            Vector3 prevPos = owner.transform.position;

            if (translateAmount != Vector3.zero)
            {
                owner.transform.Translate(translateAmount, Space.World);
                OnPositionChanged?.Invoke(owner.transform.localPosition);
            }

            currentMoveDirection = (owner.transform.position - prevPos).normalized;
        }


        private void Subscribe()
        {
            inputManager.OnStartMoving += InputManager_OnStartMoving;
            inputManager.OnStopMoving += InputManager_OnStopMoving;
            
            inputManager.OnStartRotating += InputManager_OnStartRotating;
            inputManager.OnStopRotating += InputManager_OnStopRotating;
        }


        private void Unsubscribe()
        {
            inputManager.OnStartMoving -= InputManager_OnStartMoving;
            inputManager.OnStopMoving -= InputManager_OnStopMoving;
            
            inputManager.OnStartRotating -= InputManager_OnStartRotating;
            inputManager.OnStopRotating -= InputManager_OnStopRotating;
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
