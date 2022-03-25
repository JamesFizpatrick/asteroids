using Asteroids.Game;
using UnityEngine;


namespace Asteroids.UFO
{
    public class UFOMoveController : MonoBehaviour
    {
        #region Fields

        private const float Speed = 1.0f;
        private Vector3 currentDirection;

        private Ship playerShip;
        
        #endregion


        
        #region Unity lifecycle
        
        private void FixedUpdate()
        {
            transform.Translate(currentDirection * Speed);
        }
        

        private void OnDestroy()
        {
            if (playerShip)
            {
                playerShip.OnPositionChanged -= PlayerShip_OnPositionChanged;
            }
        }

        #endregion


        
        #region Public methods

        public void Initialize(Ship ship)
        {
            playerShip = ship;
            ship.OnPositionChanged += PlayerShip_OnPositionChanged;
            currentDirection = (playerShip.transform.localPosition - gameObject.transform.localPosition).normalized;
        }

        #endregion


        
        #region Event handlers

        private void PlayerShip_OnPositionChanged(Vector3 position)
        {
            currentDirection = (position - transform.localPosition).normalized;
        }

        #endregion
    }
}
