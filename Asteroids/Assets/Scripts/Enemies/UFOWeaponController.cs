using System.Collections;
using Asteroids.Game;
using Asteroids.Handlers;
using Asteroids.Managers;
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



        #region Class lifecycle

        public UFOWeaponController(ISoundManager soundManager, IGameObjectsManager gameObjectsManager, GameObject owner, Ship player) :
            base(soundManager, gameObjectsManager, owner)
        {
            SetCurrentWeaponType(WeaponType.Enemy);
            random = new System.Random();
            this.player = player;
            Fire();
        }

        #endregion

        
        
        #region Protected methods
        
        protected override void ImplicitDispose()
        {
            if (fireCoroutine != null)
            {
                CoroutinesHandler.Instance.StopCoroutine(fireCoroutine);
            }
            
            StopFire();
        }

        #endregion

        
        
        #region Private methods

        private void Fire() => fireCoroutine = CoroutinesHandler.Instance.StartCoroutine(Shoot());
        
        
        private IEnumerator Shoot()
        {
            while (true)
            {
                Vector3 direction = player.transform.localPosition - Owner.transform.localPosition;

                (Vector3 leftVector, Vector3 rightVector) = direction.GetBreakVectors(5f);
                Vector3 newDirection = Vector3.Lerp(leftVector, rightVector, random.GetRandomFloat(0f, 1f));
                
                FireSingleShot(newDirection.normalized);
                
                yield return new WaitForSeconds(FireCooldown);
            }
        }

        #endregion
    }
}
