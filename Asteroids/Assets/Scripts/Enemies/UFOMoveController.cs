using Asteroids.Game;
using UnityEngine;


namespace Asteroids.UFO
{
    public class UFOMoveController : MonoBehaviour
    {
        #region Fields

        [SerializeField] private float Speed = 0.5f;
        
        private Vector3 currentDirection;
        private Player playerPlayer;
        
        #endregion


        
        #region Unity lifecycle
        
        private void FixedUpdate() => transform.Translate(currentDirection * Speed);


        private void OnDestroy()
        {
            if (playerPlayer)
            {
                playerPlayer.OnPositionChanged -= PlayerShip_OnPositionChanged;
            }
        }

        #endregion


        
        #region Public methods

        public void Initialize(Player player)
        {
            playerPlayer = player;
            player.OnPositionChanged += PlayerShip_OnPositionChanged;
            currentDirection = (playerPlayer.transform.localPosition - gameObject.transform.localPosition).normalized;
        }

        #endregion


        
        #region Event handlers

        private void PlayerShip_OnPositionChanged(Vector3 localPosition) =>
            currentDirection = (localPosition - transform.localPosition).normalized;

        #endregion
    }
}
