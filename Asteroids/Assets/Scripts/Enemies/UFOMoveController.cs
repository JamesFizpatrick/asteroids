using Asteroids.Game;
using UnityEngine;


namespace Asteroids.UFO
{
    public class UFOMoveController
    {
        #region Fields

        private readonly GameObject owner;
        private readonly Ship player;
        private readonly float speed;
        
        private Vector3 currentDirection;
        
        #endregion


        #region Class lifecycle

        public UFOMoveController(GameObject owner, Ship player, float speed)
        {
            this.owner = owner;
            this.speed = speed;
            this.player = player;
            
            player.OnPositionChanged += PlayerShip_OnPositionChanged;
            currentDirection = (this.player.transform.localPosition - owner.transform.localPosition).normalized;
        }

        #endregion


        
        #region Public methods
        
        public void Update() => owner.transform.Translate(currentDirection * speed);


        public void Dispose()
        {
            if (player)
            {
                player.OnPositionChanged -= PlayerShip_OnPositionChanged;
            }
        }
        
        #endregion


        
        #region Event handlers

        private void PlayerShip_OnPositionChanged(Vector3 localPosition) =>
            currentDirection = (localPosition - owner.transform.localPosition).normalized;

        #endregion
    }
}
