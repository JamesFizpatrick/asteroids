using System.Collections;
using Asteroids.Game;
using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.UFO
{
    public class UFOWeaponController : UnitWeaponController
    {
        #region Fields

        private Ship ship;
        private System.Random random;

        #endregion


        
        #region Unity lifecycle

        protected override void Awake()
        {
            currentWeaponType = WeaponType.Enemy;
            random = new System.Random();
            base.Awake();
        }

        
        private void OnDisable()
        {
            StopFire();
        }
        
        #endregion



        #region Public methods

        public void Initialize(Ship ship)
        {
            this.ship = ship;
        }
        
        
        public void StartFire()
        {
            StartCoroutine(Shoot());
        }

        #endregion



        #region Private methods

        private IEnumerator Shoot()
        {
            while (true)
            {
                Vector3 direction = ship.transform.localPosition - transform.localPosition;

                (Vector3 leftVector, Vector3 rightVector) = direction.GetBreakVectors(5f);
                Vector3 newDirection = Vector3.Lerp(leftVector, rightVector, random.GetRandomFloat(0f, 1f));
                
                FireSingleShot(newDirection.normalized);
                
                yield return new WaitForSeconds(FireCooldown);
            }
        }

        #endregion
    }
}
