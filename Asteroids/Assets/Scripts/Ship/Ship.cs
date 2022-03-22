using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Game
{
    public class Ship : MonoBehaviour
    {
        #region Fields
    
        private const float InertiaDeg = 0.01f;
        
        private float inertia;
        private Vector3 directionVector;
        private bool canMove;
    
        #endregion
    
    
        
        #region Unity lifecycle
    
        private void OnEnable() =>
            ManagersHub.GetManager<InputManager>().OnStartDirection += InputManager_OnInputFired;
    
    
        private void OnDisable() =>
            ManagersHub.GetManager<InputManager>().OnStartDirection -= InputManager_OnInputFired;
        
        
        private void Update()
        {
            if (!canMove)
            {
                return;
            }
            
            // TEMP LOGIC, DOTWEEN IS NEEDED
            transform.Translate(directionVector * inertia);
    
            if (inertia > 0f)
            {
                inertia -= InertiaDeg;
                Debug.LogError("Inertia: " + inertia);
            }
            else
            {
                canMove = false;
            }
        }
        
        #endregion
    
    
    
        #region Event handlers
    
        private void InputManager_OnInputFired(InputManager.Direction direction)
        {
            canMove = true;
            inertia = 0.1f;
            
            switch (direction)
            {
                case InputManager.Direction.Down:
                    directionVector = Vector3.down;
                    break;
                case InputManager.Direction.Up:
                    directionVector = Vector3.up;
                    break;
                case InputManager.Direction.Left:
                    directionVector = Vector3.left;
                    break;
                case InputManager.Direction.Right:
                    directionVector = Vector3.right;
                    break;
                case InputManager.Direction.None:
                    canMove = false;
                    return;
            }
        }
    
        #endregion
    }
}
