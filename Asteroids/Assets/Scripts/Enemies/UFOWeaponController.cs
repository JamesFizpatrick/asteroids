using System.Collections;
using Asteroids.Game;
using Asteroids.Handlers;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.UFO
{
    public class UFOWeaponController : UnitWeaponController
    {
        private GameManager gameManager;

        private System.Random random;
        
        protected override void Awake()
        {
            gameManager = ManagersHub.GetManager<GameManager>();
            currentWeaponType = WeaponType.Enemy;
            random = new System.Random();
            base.Awake();
        }

        
        private void OnEnable()
        {
            StartCoroutine(Shoot());
        }


        private void OnDisable()
        {
            StopFire();
        }


        private IEnumerator Shoot()
        {
            while (true)
            {
                Vector3 direction = gameManager.GetPlayerShipLocalPosition() - transform.localPosition;

                (Vector3 leftVector, Vector3 rightVector) = direction.GetBreakVectors(5f);
                Vector3 newDirection = Vector3.Lerp(leftVector, rightVector, random.GetRandomFloat(0f, 1f));
                
                FireSingleShot(newDirection.normalized);
                
                yield return new WaitForSeconds(FireCooldown);
            }
        }
    }
}
