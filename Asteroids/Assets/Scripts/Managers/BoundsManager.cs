using UnityEngine;


namespace Asteroids.Managers
{
    public class BoundsManager : MonoBehaviour, IManager
    {
        #region Fields
        
        private static BoundsManager instance;
        private BoxCollider boundCollider;

        private int weaponLayer;
        private int shipLayer;
        private int asteroidLayer;
        private int enemyLayer;
        
        #endregion



        #region Properties

        public static BoundsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject managerGo = new GameObject("BoundsManager");
                    BoundsManager manager = managerGo.AddComponent<BoundsManager>();
                    instance = manager;
                }

                return instance;
            }
        }

        #endregion


        
        #region Unity lifecycle

        private void Awake()
        {
            weaponLayer = LayerMask.NameToLayer("Weapon");
            shipLayer = LayerMask.NameToLayer("Ship");
            asteroidLayer = LayerMask.NameToLayer("Asteroid");
            enemyLayer = LayerMask.NameToLayer("Enemy");
        }
        
        
        private void OnTriggerExit(Collider entity)
        {
            if (entity.gameObject.layer == weaponLayer)
            {
                entity.gameObject.SetActive(false);
            }
        }

        #endregion



        #region Public methods

        public void Initialize()
        {
            Camera mainCamera = Camera.main;
            
            transform.SetPositionAndRotation(mainCamera.transform.position, mainCamera.transform.rotation);
            
            boundCollider = gameObject.AddComponent<BoxCollider>();
            boundCollider.size = new Vector3(340f, 200f, mainCamera.farClipPlane);
            boundCollider.isTrigger = true;
            
            Vector3 center = boundCollider.center;
            center.z += mainCamera.farClipPlane / 2f;
            boundCollider.center = center;
        }

        
        public void Unload() { }

        #endregion
    }
}
