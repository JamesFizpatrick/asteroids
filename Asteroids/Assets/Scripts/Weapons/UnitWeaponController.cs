using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Handlers;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Game
{
    public class UnitWeaponController
    {
        #region Fields

        protected float FireCooldown;
        protected WeaponType CurrentWeaponType { get; private set; }
                
        private SoundManager soundManager;
        private GameObjectsManager gameObjectsManager;
        
        private Coroutine attackCoroutine;

        private List<Weapon> weapons = new List<Weapon>();

        private Weapon currentWeapon;
        private int currentWeaponIndex;

        protected GameObject Owner;

        #endregion


        
        #region Class lifecycle

        public UnitWeaponController(SoundManager soundManager, GameObjectsManager gameObjectsManager, GameObject owner)
        {
            this.soundManager = soundManager;
            this.gameObjectsManager = gameObjectsManager;
            Owner = owner;
        }

        #endregion
        
        
        
        #region Public methods
        
        public void Dispose()
        {
            if (attackCoroutine != null)
            {
                CoroutinesHandler.Instance.StopCoroutine(attackCoroutine);
            }

            foreach (Weapon weapon in weapons)
            {
                foreach (ShotBase bullet in weapon.Pool)
                {
                    GameObject.Destroy(bullet.gameObject);
                }
            }

            ImplicitDispose();
        }
        
        #endregion
        
        
        
        #region Protected methods


        protected virtual void ImplicitDispose() { }
        
        
        protected void StartFire() => attackCoroutine = CoroutinesHandler.Instance.StartCoroutine(ProcessFire());


        protected void StopFire()
        {
            if (attackCoroutine != null)
            {
                CoroutinesHandler.Instance.StopCoroutine(attackCoroutine);
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
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, direction);
            FireSingleShot(Owner.transform.position, rotation);
        }


        protected void SetCurrentWeaponType(WeaponType weaponType)
        {
            CurrentWeaponType = weaponType;
            PrepareWeapons();
        }
        
        #endregion



        #region Private methods

        private void PrepareWeapons()
        {
            switch (CurrentWeaponType)
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
        
        
        private IEnumerator ProcessFire()
        {
            while (true)
            {
                ShotBase bullet = FireSingleShot(Owner.transform.position, Owner.transform.rotation);
                
                if (bullet)
                {
                    yield return new WaitForSeconds(bullet.FireCooldown);
                }
                else
                {
                    yield return null;
                }
            }
        }


        private ShotBase FireSingleShot(Vector3 fromPosition, Quaternion fromRotation)
        {
            ShotBase bullet = TryCreateSingleShot();

            if (bullet == null)
            {
                Debug.LogError("Cannot spawn bullet!");
                return null;
            }
            
            bullet.transform.SetPositionAndRotation(fromPosition, fromRotation);
            bullet.gameObject.SetActive(true);
            
            soundManager.PlaySound(bullet.SoundType);

            return bullet;
        }
        
        
        private ShotBase TryCreateSingleShot()
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
