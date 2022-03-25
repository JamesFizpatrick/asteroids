using UnityEngine;


namespace Asteroids.Game
{
    public class EnemyWeapon : WeaponsBase
    {
        #region Fields

        private int playerLayer;

        #endregion


        
        #region Unity lifecycle

        private void Awake() => playerLayer = LayerMask.NameToLayer("Player");

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
