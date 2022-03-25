using Asteroids.Game;
using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.Managers
{
    public class EnemiesManager : MonoBehaviour, IManager
    {
        #region Fields

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


        public void SpawnEnemy(Vector3 position, Ship ship)
        {
            GameObject ufoPrefab = ManagersHub.GetManager<DataManager>().PlayerPreset.Enemy;
            GameObject ufo = Instantiate(ufoPrefab, GameSceneReferences.MainCanvas.transform);
            ufo.transform.localPosition = position;
            ufo.GetComponent<UFO.UFO>().Initialze(ship);
        }
        
        #endregion
    }
}
