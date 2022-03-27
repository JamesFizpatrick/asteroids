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

        private const int MaxBulletsAmount = 10;

        protected float FireCooldown = 0.0f;
        protected WeaponType currentWeaponType = WeaponType.None;
        
        private GameObject bulletPrefab;
        
        private DataManager dataManager;
        private SoundManager soundManager;
        
        private Coroutine attackCoroutine;

        private List<WeaponsBase> bulletsPool = new List<WeaponsBase>();

        #endregion



        #region Unity lifecycle

        protected virtual void Awake()
        {
            dataManager = ManagersHub.GetManager<DataManager>();
            soundManager = ManagersHub.GetManager<SoundManager>();

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

            FireCooldown = bulletPrefab.GetComponent<WeaponsBase>().FireCooldown;
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


        protected void FireSingleShot(Vector3 direction)
        {
            WeaponsBase bullet = FireShot();

            if (bullet == null)
            {
                Debug.LogError("Cannot spawn bullets!");
            }
            else
            {
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, direction);
                bullet.transform.SetPositionAndRotation(transform.position, rotation);
                bullet.gameObject.SetActive(true);
                
                soundManager.PlaySound(bullet.SoundType);
            }
        }
        
        #endregion



        #region Private methods

        private IEnumerator ProcessFire()
        {
            while (true)
            {
                WeaponsBase bullet = FireShot();

                if (bullet == null)
                {
                    Debug.LogError("Cannot spawn bullets!");
                    yield return null;
                }
                else
                {
                    bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
                    bullet.gameObject.SetActive(true);
                    
                    soundManager.PlaySound(bullet.SoundType);
                    
                    yield return new WaitForSeconds(bullet.FireCooldown);
                }
            }
        }


        private WeaponsBase FireShot()
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

            return bullet;
        }
        
        #endregion
    }
}
