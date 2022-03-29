using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.Managers
{
    public class BoundsManager : BaseManager<BoundsManager>
    {
        #region Fields
        
        private BoxCollider boundCollider;

        private int playerProjectilesLayer;
        private int enemyProjectilesLayer;
        private int playerLayer;
        private int asteroidLayer;
        private int enemyLayer;
        
        #endregion


        
        #region Unity lifecycle

        private void Awake()
        {
            playerProjectilesLayer = LayerMasksHandler.PlayerProjectiles;
            enemyProjectilesLayer = LayerMasksHandler.EnemyProjectiles;
            playerLayer = LayerMasksHandler.Player;
            asteroidLayer = LayerMasksHandler.Asteroid;
            enemyLayer = LayerMasksHandler.Enemy;
        }
        
        
        private void OnTriggerExit2D(Collider2D entity)
        {
            int layer = entity.gameObject.layer;
            
            if (layer == playerProjectilesLayer || layer == enemyProjectilesLayer)
            {
                entity.gameObject.SetActive(false);
            }
            else if (layer == playerLayer || layer == asteroidLayer || layer == enemyLayer)
            {
                TeleportEntity(entity.gameObject);
            }
        }

        #endregion



        #region Protected methods

        protected override void Initialize()
        {
            transform.parent = GameSceneReferences.MainCanvas.transform;
            gameObject.AddComponent<RectTransform>();
            
            BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;

            Vector2 size = GameSceneReferences.MainCanvas.pixelRect.max;
            Vector2 scale = GameSceneReferences.MainCanvas.GetComponent<RectTransform>().localScale;
            boxCollider.size = size * scale + new Vector2(20f, 20f);
            boxCollider.offset = Vector3.zero;
                    
            Rigidbody2D rigidBody = gameObject.AddComponent<Rigidbody2D>();
            rigidBody.bodyType = RigidbodyType2D.Kinematic;
        }

        
        protected override void Deinitialize() { }

        #endregion


        
        #region Private methods

        private void TeleportEntity(GameObject entity)
        {
            Vector3 localPosition = entity.transform.localPosition;

            float maxX = Screen.width / 2f;
            float maxY = Screen.height / 2f;
            float minX = -Screen.width / 2f;
            float minY = -Screen.height / 2f;
                
            // Move the entity to the opposite side of the screen
            
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
