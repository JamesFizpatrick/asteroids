using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.Weapons
{
    public class PlayerShot : ShotBase
    {
        #region Fields

        private int asteroidLayer;
        private int enemyLayer;

        #endregion


        
        #region Unity lifecycle

        private void Awake()
        {
            asteroidLayer = LayerMasksHandler.Asteroid;
            enemyLayer = LayerMasksHandler.Enemy;
        }

        #endregion

        

        #region Protected methods

        protected override void ProcessOnTriggerEnter(Collider2D col)
        {
            int colliderLayer = col.gameObject.layer;
            if (colliderLayer == asteroidLayer || colliderLayer == enemyLayer)
            {
                gameObject.SetActive(false);
            }
        }

        #endregion
    } 
}
