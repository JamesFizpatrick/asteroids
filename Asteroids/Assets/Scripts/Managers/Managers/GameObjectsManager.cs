using Asteroids.Data;
using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.Managers
{
    public class GameObjectsManager : IGameObjectsManager
    {
        #region Fields

        private const string BulletsRootObjectName = "===BULLETS===";
        private const string AsteroidsRootObjectName = "===ASTEROIDS===";
        private const string PlayerShipsRootObjectName = "===SHIPS===";
        private const string EnemiesRootObjectName = "===ENEMIES===";
        
        private GameObject bulletsRoot;
        private GameObject asteroidsRoot;
        private GameObject playerShipsRoot;
        private GameObject enemiesRoot;

        #endregion



        #region Public methods

        public GameObject CreateBullet(GameObject bulletPrefab) => CreateObject(bulletPrefab, bulletsRoot);


        public GameObject CreatePlayerShip() => CreateObject(DataContainer.GamePreset.Ship, playerShipsRoot);


        public GameObject CreateAsteroid(GameObject asteroidPrefab) => CreateObject(asteroidPrefab, asteroidsRoot);

        
        public GameObject CreateEnemy() => CreateObject(DataContainer.GamePreset.Enemy, enemiesRoot);


        public GameObject CreateVFX(GameObject vfxPrefab) => CreateObject(vfxPrefab, asteroidsRoot);
        
        
        public void Initialize(IManagersHub hub)
        {
            bulletsRoot = CreateRootObject(BulletsRootObjectName);
            asteroidsRoot = CreateRootObject(AsteroidsRootObjectName);
            playerShipsRoot = CreateRootObject(PlayerShipsRootObjectName);
            enemiesRoot = CreateRootObject(EnemiesRootObjectName);
        }
              
        #endregion


        
        #region Private methods

        private GameObject CreateObject(GameObject prefab, GameObject root) =>
            Object.Instantiate(prefab, root.transform);


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
