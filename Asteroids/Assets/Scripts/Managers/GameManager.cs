using Asteroids.Game;
using UnityEngine;


namespace Asteroids.Managers
{
    public class GameManager : MonoBehaviour, IManager
    {
        #region Fields

        private static GameManager instance;

        #endregion


        
        #region Properties

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject managerGo = new GameObject("GameManager");
                    GameManager manager = managerGo.AddComponent<GameManager>();
                    instance = manager;
                }

                return instance;
            }
        }

        #endregion


        
        #region Unity lifecycle

        public void Start() => StartGame();

        #endregion



        #region Public methods

        public void Initialize() { }

        
        public void Unload() { }

        #endregion



        #region Private methods

        private void StartGame()
        {
            GameObject ship = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ship.name = "Ship";
            ship.AddComponent<Ship>();
        }

        #endregion
    }
}
