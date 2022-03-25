using Asteroids.Game;
using UnityEngine;


namespace Asteroids.UFO
{
    public class UFOMoveController : MonoBehaviour
    {
        #region Fields

        private const float Speed = 1.0f;
        private Vector3 currentDirection;

        private Player _playerPlayer;
        
        #endregion


        
        #region Unity lifecycle
        
        private void FixedUpdate()
        {
            transform.Translate(currentDirection * Speed);
        }
        

        private void OnDestroy()
        {
            if (_playerPlayer)
            {
                _playerPlayer.OnPositionChanged -= PlayerShip_OnPositionChanged;
            }
        }

        #endregion


        
        #region Public methods

        public void Initialize(Player player)
        {
            _playerPlayer = player;
            player.OnPositionChanged += PlayerShip_OnPositionChanged;
            currentDirection = (_playerPlayer.transform.localPosition - gameObject.transform.localPosition).normalized;
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
