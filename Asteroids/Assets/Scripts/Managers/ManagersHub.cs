using System;
using System.Collections.Generic;


namespace Asteroids.Managers
{
    public static class ManagersHub
    {
        #region Fields
    
        private static Dictionary<Type, IManager> managersDictionary = new Dictionary<Type, IManager>();
    
        #endregion


    
        #region Public methods

        public static void Initialize()
        {
            //TODO: move initialization somewhere else to be able to freely modify the managers list 
            managersDictionary.Add(typeof(InputManager), InputManager.Instance);
            managersDictionary.Add(typeof(DataManager), DataManager.Instance);
            managersDictionary.Add(typeof(GameManager), GameManager.Instance);
            managersDictionary.Add(typeof(BoundsManager), BoundsManager.Instance);
            managersDictionary.Add(typeof(AsteroidsManager), AsteroidsManager.Instance);
            managersDictionary.Add(typeof(PlayerShipsManager), PlayerShipsManager.Instance);
            managersDictionary.Add(typeof(EnemiesManager), EnemiesManager.Instance);

            foreach (KeyValuePair<Type, IManager> pair in managersDictionary)
            {
                pair.Value.Initialize();
            }
        }
    

        public static void Deinitialize()
        {
            foreach (KeyValuePair<Type, IManager> pair in managersDictionary)
            {
                pair.Value.Unload();
            }
        }

    
        public static TManagerType GetManager<TManagerType>()
        {
            Type managerType = typeof(TManagerType);

            IManager manager = managersDictionary[managerType];
            if (manager == null)
            {
                throw new NullReferenceException("There is no manager of type " + managerType + "!");
            }
        
            return (TManagerType)manager;
        }

        #endregion
    }
}
