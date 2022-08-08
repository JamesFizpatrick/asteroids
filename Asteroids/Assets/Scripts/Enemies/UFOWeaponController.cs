using System.Collections;
using Asteroids.Game;
using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.UFO
{
    public class UFOWeaponController : UnitWeaponController
    {
        #region Fields

        private Ship player;
        private System.Random random;

        private Coroutine fireCoroutine;
        
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
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
            }
            
            StopFire();
        }

        #endregion



        #region Public methods

        public void Initialize(Ship player) => this.player = player;


        public void Fire() => fireCoroutine = StartCoroutine(Shoot());

        #endregion



        #region Private methods

        private IEnumerator Shoot()
        {
            while (true)
            {
                Vector3 direction = player.transform.localPosition - transform.localPosition;

                (Vector3 leftVector, Vector3 rightVector) = direction.GetBreakVectors(5f);
                Vector3 newDirection = Vector3.Lerp(leftVector, rightVector, random.GetRandomFloat(0f, 1f));
                
                FireSingleShot(newDirection.normalized);
                
                yield return new WaitForSeconds(FireCooldown);
            }
        }

        #endregion
    }
}
