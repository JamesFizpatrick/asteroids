using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Game
{
    public class ShipWeaponController : MonoBehaviour
    {
        private const int MaxBulletsAmount = 10;
        
        private InputManager inputManager;
        
        private GameObject bulletPrefab;
        private List<WeaponsBase> bulletsPool = new List<WeaponsBase>();

        private Coroutine attackCoroutine;
        
        
        private void Awake()
        {
            inputManager = ManagersHub.GetManager<InputManager>();
            bulletPrefab = Resources.Load("Prefabs/Weapons/Weapons_bullet_1") as GameObject;
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
            foreach (WeaponsBase bullet in bulletsPool)
            {
                Destroy(bullet.gameObject);
            }
        }


        private IEnumerator ProcessFire()
        {
            while (true)
            {
                WeaponsBase bullet;
                
                if (bulletsPool.Count < MaxBulletsAmount)
                {
                    bullet = Instantiate(bulletPrefab, transform).GetComponent<WeaponsBase>();
                    bulletsPool.Add(bullet);
                }
                else
                {
                    bullet = bulletsPool.FirstOrDefault(bullet => bullet.gameObject.activeSelf == false);
                }

                if (bullet == null)
                {
                    Debug.LogError("Cannot spawn bullets!");
                    yield return null;
                }
                else
                {
                    bullet.transform.parent = null;
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
    }
}
