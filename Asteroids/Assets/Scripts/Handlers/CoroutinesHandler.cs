using UnityEngine;


namespace Asteroids.Handlers
{
    public class CoroutinesHandler : MonoBehaviour
    {
        private static CoroutinesHandler instance;

        public static CoroutinesHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject helperGo = new GameObject(typeof(CoroutinesHandler).ToString());
                    CoroutinesHandler manager = helperGo.AddComponent<CoroutinesHandler>();
                    instance = manager;
                }

                return instance;
            }
        }

        private void Awake() => DontDestroyOnLoad(this);
    }
}
