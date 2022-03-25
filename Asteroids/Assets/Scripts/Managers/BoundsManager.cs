using Asteroids.Game;
using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.Managers
{
    public class BoundsManager : MonoBehaviour, IManager
    {
        #region Fields
        
        private static BoundsManager instance;
        private BoxCollider boundCollider;

        private int playerProjectilesLayer;
        private int enemyProjectilesLayer;
        private int PlayerLayer;
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
            playerProjectilesLayer = LayerMask.NameToLayer("PlayerProjectiles");
            playerProjectilesLayer = LayerMask.NameToLayer("EnemyProjectiles");
            PlayerLayer = LayerMask.NameToLayer("Player");
            asteroidLayer = LayerMask.NameToLayer("Asteroid");
            enemyLayer = LayerMask.NameToLayer("Enemy");
        }
        
        
        private void OnTriggerExit2D(Collider2D entity)
        {
            int layer = entity.gameObject.layer;
            
            if (layer == playerProjectilesLayer || layer == enemyProjectilesLayer)
            {
                entity.gameObject.SetActive(false);
            }
            else if (layer == PlayerLayer || layer == asteroidLayer || layer == enemyLayer)
            {
                TeleportEntity(entity.gameObject);
            }
        }

        #endregion



        #region Public methods

        public void Initialize()
        {
            transform.parent = GameSceneReferences.MainCanvas.transform;
            gameObject.AddComponent<RectTransform>();
            
            BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;

            Vector2 size = GameSceneReferences.MainCanvas.pixelRect.max;
            boxCollider.size = size;
            boxCollider.offset = size / 2f;
                    
            Rigidbody2D rigidBody = gameObject.AddComponent<Rigidbody2D>();
            rigidBody.bodyType = RigidbodyType2D.Kinematic;
        }

        
        public void Unload() { }

        #endregion


        
        #region Private methods

        private void TeleportEntity(GameObject entity)
        {
            Vector3 localPosition = entity.transform.localPosition;

            float maxX = Screen.width / 2f;
            float maxY = Screen.height / 2f;
            float minX = -Screen.width / 2f;
            float minY = -Screen.height / 2f;
                
            if (localPosition.x > maxX)
            {
                localPosition.x = minX;
            }
            else if (localPosition.x < minX)
            {
                localPosition.x = maxX;
            }
            else if (localPosition.y > maxY)
            {
                localPosition.y = minY;
            }
            else if (localPosition.y < minY)
            {
                localPosition.y = maxY;
            }

            entity.gameObject.transform.localPosition = localPosition;
        }

        #endregion
    }
}
