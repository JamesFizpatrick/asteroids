using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Handlers;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Game
{
    public class UnitWeaponController : MonoBehaviour
    {
        #region Fields

        protected WeaponType currentWeaponType = WeaponType.None;

        private const int MaxBulletsAmount = 10;
        
        private GameObject bulletPrefab;
        private List<WeaponsBase> bulletsPool = new List<WeaponsBase>();

        private DataManager dataManager;

        private Coroutine attackCoroutine;
        
        #endregion



        #region Unity lifecycle

        protected virtual void Awake()
        {
            dataManager = ManagersHub.GetManager<DataManager>();

            switch (currentWeaponType)
            {
                case WeaponType.Player:
                    bulletPrefab = dataManager.PlayerPreset.PlayerProjectiles;
                    break;
                case WeaponType.Enemy:
                    bulletPrefab = dataManager.PlayerPreset.EnemyProjectiles;
                    break;
                case WeaponType.None:
                    throw new Exception($"Weapon type was no inited for {GetType()}");
            }
        }

        
        private void OnDestroy()
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
            
            foreach (WeaponsBase bullet in bulletsPool)
            {
                Destroy(bullet.gameObject);
            }
        }
        
        #endregion


        
        #region Public methods

        protected void StartFire()
        {
            attackCoroutine = StartCoroutine(ProcessFire());
        }


        protected void StopFire()
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
        }

        #endregion



        #region Private methods

        private IEnumerator ProcessFire()
        {
            while (true)
            {
                WeaponsBase bullet;
                
                if (bulletsPool.Count < MaxBulletsAmount)
                {
                    bullet =
                        Instantiate(bulletPrefab, GameSceneReferences.MainCanvas.transform).GetComponent<WeaponsBase>();
                    bulletsPool.Add(bullet);
                }
                else
                {
                    bullet = bulletsPool.FirstOrDefault(b => b.gameObject.activeSelf == false);
                }

                if (bullet == null)
                {
                    Debug.LogError("Cannot spawn bullets!");
                    yield return null;
                }
                else
                {
                    bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
                    bullet.gameObject.SetActive(true);
            
                    yield return new WaitForSeconds(bullet.FireCooldown);
                }
            }
        }

        #endregion
    }
}
