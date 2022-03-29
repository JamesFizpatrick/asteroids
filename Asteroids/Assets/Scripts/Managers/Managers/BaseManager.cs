using UnityEngine;


namespace Asteroids.Managers
{
    public abstract class BaseManager<TManagerType> : MonoBehaviour, IManager where TManagerType : Component
    {
        #region Fields

        private static TManagerType instance;
        private static GameObject managerGo;

        #endregion



        #region Properties

        public static TManagerType Instance
        {
            get
            {
                if (instance == null)
                {
                    managerGo = new GameObject(typeof(TManagerType).ToString());
                    TManagerType manager = managerGo.AddComponent<TManagerType>();
                    instance = manager;
                }

                return instance;
            }
        }

        #endregion



        #region Public methods
        
        public void Initialize(GameObject root)
        {
            managerGo.transform.parent = root.transform;
            Initialize();
            
        }

        
        public void Unload()
        {
            Deinitialize();
            if (managerGo != null)
            {
                Destroy(managerGo);
            }
        }
        
        #endregion


        
        #region Protected methods

        protected abstract void Initialize();
        
        protected abstract void Deinitialize();

        #endregion
    }
}
