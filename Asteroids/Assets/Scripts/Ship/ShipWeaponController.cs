using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Handlers;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Game
{
    public class ShipWeaponController : MonoBehaviour
    {
        #region Fields

        private const int MaxBulletsAmount = 10;
        
        private InputManager inputManager;
        private DataManager dataManager;
        
        private GameObject bulletPrefab;
        private List<WeaponsBase> bulletsPool = new List<WeaponsBase>();

        private Coroutine attackCoroutine;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            inputManager = ManagersHub.GetManager<InputManager>();
            dataManager = ManagersHub.GetManager<DataManager>();
            
            bulletPrefab = dataManager.PlayerPreset.Weapon;
        }
        

        private void OnEnable()
        {
            inputManager.OnStartFiring += InputManager_OnStartFiring;
            inputManager.OnStopFiring += InputManager_OnStopFiring;
        }

        
        private void OnDisable()
        {
            inputManager.OnStartFiring -= InputManager_OnStartFiring;
            inputManager.OnStopFiring -= InputManager_OnStopFiring;
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



        #region Private methods
        
        private IEnumerator ProcessFire()
        {
            while (true)
            {
                WeaponsBase bullet;
                
                if (bulletsPool.Count < MaxBulletsAmount)
                {
                    bullet = Instantiate(bulletPrefab, GameSceneReferences.MainCanvas.transform).GetComponent<WeaponsBase>();
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
                    // bullet.transform.parent = GameSceneReferences.MainCanvas.transform;
                    bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
                    bullet.gameObject.SetActive(true);
            
                    yield return new WaitForSeconds(bullet.FireCooldown);
                }
            }
        }
        
        
        private void InputManager_OnStartFiring()
        {
            attackCoroutine = StartCoroutine(ProcessFire());
        }

        
        private void InputManager_OnStopFiring()
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
        }
        
        #endregion
    }
}
