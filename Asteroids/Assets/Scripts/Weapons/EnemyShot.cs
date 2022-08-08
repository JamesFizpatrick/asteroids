using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.Game
{
    public class EnemyShot : ShotBase
    {
        #region Fields

        private int playerLayer;

        #endregion


        
        #region Unity lifecycle

        private void Awake() => playerLayer = LayerMasksHandler.Player;

        #endregion

        

        #region Protected methods

        protected override void ProcessOnTriggerEnter(Collider2D col)
        {
            if (col.gameObject.layer == playerLayer)
            {
                gameObject.SetActive(false);
            }
        }

        #endregion
    }
}
