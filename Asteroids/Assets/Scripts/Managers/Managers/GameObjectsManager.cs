using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.Managers
{
    public class GameObjectsManager : BaseManager<GameObjectsManager>
    {
        #region Fields

        private GameObject bulletsRoot;
        private GameObject asteroidsRoot;
        private GameObject playerShipsRoot;
        private GameObject enemiesRoot;

        #endregion



        #region Public methods

        public GameObject CreateBullet(GameObject bulletPrefab) => CreateObject(bulletPrefab, bulletsRoot);


        public GameObject CreatePlayerShip(GameObject shipPrefab) => CreateObject(shipPrefab, playerShipsRoot);


        public GameObject CreateAsteroid(GameObject asteroidPrefab) => CreateObject(asteroidPrefab, asteroidsRoot);

        
        public GameObject CreateEnemy(GameObject enemyPrefab) => CreateObject(enemyPrefab, enemiesRoot);


        public GameObject CreateVFX(GameObject vfxPrefab) => CreateObject(vfxPrefab, asteroidsRoot);
        
        #endregion



        #region Protected methods

        protected override void Initialize()
        {
            bulletsRoot = CreateRootObject("===BULLETS===");
            asteroidsRoot = CreateRootObject("===ASTEROIDS===");
            playerShipsRoot = CreateRootObject("===SHIPS===");
            enemiesRoot = CreateRootObject("===ENEMIES===");
        }
        
        
        protected override void Deinitialize() { }
        
        #endregion


        
        #region Private methods

        private GameObject CreateObject(GameObject prefab, GameObject root) => Instantiate(prefab, root.transform);


        private GameObject CreateRootObject(string name)
        {
            GameObject result = new GameObject(name);
            result.transform.parent = GameSceneReferences.MainCanvas.transform;
            
            RectTransform rectTransform = result.AddComponent<RectTransform>();
            
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            
            rectTransform.localScale = Vector3.one;

            return result;
        }
        
        #endregion
    }
}
