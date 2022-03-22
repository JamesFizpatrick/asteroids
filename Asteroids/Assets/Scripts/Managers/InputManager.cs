using System;
using UnityEngine;


namespace Asteroids.Managers
{
    public class InputManager : MonoBehaviour, IManager
    {
        #region Nested types

        public enum Direction
        {
            None  = 0,
            Up    = 1,
            Left  = 2,
            Down  = 3,
            Right = 4
        }

        #endregion


        
        #region Fields

        public Action<Direction> OnStartDirection;
        public Action<Direction> OnStopDirection;

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
                OnStartDirection?.Invoke(Direction.Up);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                OnStartDirection?.Invoke(Direction.Left);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                OnStartDirection?.Invoke(Direction.Down);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                OnStartDirection?.Invoke(Direction.Right);
            }
            
            
            if (Input.GetKeyUp(KeyCode.W))
            {
                OnStopDirection?.Invoke(Direction.Up);
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                OnStopDirection?.Invoke(Direction.Left);
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                OnStopDirection?.Invoke(Direction.Down);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                OnStopDirection?.Invoke(Direction.Right);
            }
        }

        #endregion


        
        #region Public methods

        public void Initialize() { }

        public void Unload() => Destroy(instance.gameObject);

        #endregion
    }
}
