using System;
using UnityEngine;


namespace Asteroids.Managers
{
    public class InputManager : MonoBehaviour, IManager
    {
        #region Nested types

        public enum RotationType
        {
            None              = 0,
            Clockwise         = 1,
            CounterClockwise  = 2
        }


        public enum MovementType
        {
            None     = 0,
            Forward  = 1,
            Backward = 2,
        }

        #endregion


        
        #region Fields

        public Action<RotationType> OnStartRotating;
        public Action<RotationType> OnStopRotating;
        public Action<MovementType> OnStartMoving;
        public Action<MovementType> OnStopMoving;
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
                OnStartMoving?.Invoke(MovementType.Forward);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                OnStartRotating?.Invoke(RotationType.CounterClockwise);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                OnStartMoving?.Invoke(MovementType.Backward);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                OnStartRotating?.Invoke(RotationType.Clockwise);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnStartFiring?.Invoke();
            }
            
            
            if (Input.GetKeyUp(KeyCode.W))
            {
                OnStopMoving?.Invoke(MovementType.Forward);
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                OnStopRotating?.Invoke(RotationType.CounterClockwise);
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                OnStopMoving?.Invoke(MovementType.Backward);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                OnStopRotating?.Invoke(RotationType.Clockwise);
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
