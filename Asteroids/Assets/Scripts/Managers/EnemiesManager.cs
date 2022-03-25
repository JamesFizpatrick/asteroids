using Asteroids.Game;
using Asteroids.Handlers;
using UnityEngine;
using Random = System.Random;


namespace Asteroids.Managers
{
    public class EnemiesManager : MonoBehaviour, IManager
    {
        #region Fields

        public UFO.UFO Enemy { get; private set; }

        private const int UFOSpawnDistance = 10;
        private static EnemiesManager instance;
        
        #endregion


        
        #region Properties

        public static EnemiesManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject managerGo = new GameObject("EnemiesManager");
                    EnemiesManager manager = managerGo.AddComponent<EnemiesManager>();
                    instance = manager;
                }

                return instance;
            }
        }

        #endregion



        #region Public methods

        public void Initialize() { }

        public void Unload() { }


        public void SpawnEnemy(Ship ship)
        {
            GameObject ufoPrefab = ManagersHub.GetManager<DataManager>().PlayerPreset.Enemy;
            GameObject ufo = Instantiate(ufoPrefab, GameSceneReferences.MainCanvas.transform);

            Random random = new Random();
            
            int maxX = Screen.width / 2;
            int minX = -Screen.width / 2;
            int maxY = Screen.height / 2;
            int minY = -Screen.height / 2;

            int x;
            int y;
            
            int divider = random.Next(0, 2);

            if (divider == 0)
            {
                x = random.GetRandomExclude(minX - UFOSpawnDistance, maxX + UFOSpawnDistance,
                    minX, maxX);
                y = random.Next(minY, maxY);
            }
            else
            {
                x = random.Next(minX, maxX);
                y = random.GetRandomExclude(minY - UFOSpawnDistance, maxY + UFOSpawnDistance,
                    minY, maxY);
            }
            
            ufo.transform.localPosition = new Vector3(x, y);
            
            Enemy = ufo.GetComponent<UFO.UFO>();
            Enemy.Initialze(ship);
        }
        
        #endregion
    }
}
