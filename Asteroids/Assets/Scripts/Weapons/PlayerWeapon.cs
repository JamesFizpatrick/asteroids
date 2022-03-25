using UnityEngine;


namespace Asteroids.Game
{
    public class PlayerWeapon : WeaponsBase
    {
        #region Fields

        private int asteroidLayer;
        private int enemyLayer;

        #endregion


        
        #region Unity lifecycle

        private void Awake()
        {
            asteroidLayer = LayerMask.NameToLayer("Asteroid");
            enemyLayer = LayerMask.NameToLayer("Enemy");
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
