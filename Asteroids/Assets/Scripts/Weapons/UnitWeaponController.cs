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

        protected float FireCooldown = 0.0f;
        protected WeaponType currentWeaponType = WeaponType.None;
                
        private SoundManager soundManager;
        private GameObjectsManager gameObjectsManager;
        
        private Coroutine attackCoroutine;

        private List<Weapon> weapons = new List<Weapon>();

        private Weapon currentWeapon;
        private int currentWeaponIndex;

        #endregion



        #region Unity lifecycle

        protected virtual void Awake()
        {
            soundManager = ManagersHub.Instance.GetManager<SoundManager>();
            gameObjectsManager = ManagersHub.Instance.GetManager<GameObjectsManager>();

            switch (currentWeaponType)
            {
                case WeaponType.Player:
                    weapons.Add(new Weapon(DataContainer.GamePreset.PlayerProjectiles));
                    weapons.Add(new Weapon(DataContainer.GamePreset.PlayerAltProjectiles));
                    break;
                case WeaponType.Enemy:
                    weapons.Add(new Weapon(DataContainer.GamePreset.EnemyProjectiles));
                    break;
                case WeaponType.None:
                    throw new Exception($"Weapon type was not initialized for {GetType()}");
            }

            currentWeaponIndex = 0;
            currentWeapon = weapons[currentWeaponIndex];
            FireCooldown = currentWeapon.ShotPrefab.GetComponent<ShotBase>().FireCooldown;
        }

        
        private void OnDestroy()
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }

            foreach (Weapon weapon in weapons)
            {
                foreach (ShotBase bullet in weapon.Pool)
                {
                    Destroy(bullet.gameObject);
                }
            }
        }
        
        #endregion

        
        
        #region Protected methods
        
        protected void StartFire() => attackCoroutine = StartCoroutine(ProcessFire());


        protected void StopFire()
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
        }

        
        protected void SwitchWeaponType()
        {
            currentWeaponIndex++;

            if (currentWeaponIndex > weapons.Count - 1)
            {
                currentWeaponIndex = 0;
            }

            currentWeapon = weapons[currentWeaponIndex];            
            FireCooldown = currentWeapon.ShotPrefab.GetComponent<ShotBase>().FireCooldown;
        }
        

        protected void FireSingleShot(Vector3 direction)
        {
            ShotBase bullet = FireSingleShot();

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
                ShotBase bullet = FireSingleShot();

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


        private ShotBase FireSingleShot()
        {
            ShotBase bullet;

            if (currentWeapon.Pool.Count < PlayerConstants.MaxPoolBulletsAmount)
            {
                bullet = gameObjectsManager.CreateBullet(currentWeapon.ShotPrefab).GetComponent<ShotBase>();
                currentWeapon.Pool.Add(bullet);
            }
            else
            {
                bullet = currentWeapon.Pool.FirstOrDefault(b => b.gameObject.activeSelf == false);
            }

            return bullet;
        }
        
        #endregion
    }
}
