using System;
using UnityEngine;


namespace Asteroids.Managers
{
    public class InputManager : MonoBehaviour, IManager
    {
        #region Fields

        public Action<InputRotationType> OnStartRotating;
        public Action<InputRotationType> OnStopRotating;
        public Action<InputMovementType> OnStartMoving;
        public Action<InputMovementType> OnStopMoving;
        public Action OnStartFiring;
        public Action OnStopFiring;


        private static InputManager instance;

        #endregion



        #region Properties

        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject managerGo = new GameObject("InputManager");
                    InputManager manager = managerGo.AddComponent<InputManager>();
                    instance = manager;
                }

                return instance;
            }
        }

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


        
        #region Public methods

        public void Initialize() { }

        public void Unload() { }

        #endregion
    }
}
