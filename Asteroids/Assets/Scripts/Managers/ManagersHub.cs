using System;
using System.Collections.Generic;


namespace Asteroids.Managers
{
    public class ManagersHub
    {
        #region Fields

        private static ManagersHub instance;

        private Dictionary<Type, IManager> managersDictionary = new Dictionary<Type, IManager>();
    
        #endregion


        
        #region Properties

        public static ManagersHub Instance => instance ??= new ManagersHub();

        #endregion


    
        #region Public methods

        public void Initialize()
        {
            //Add new services here
            managersDictionary.Add(typeof(InputManager), new InputManager());
            managersDictionary.Add(typeof(GameManager), new GameManager());
            managersDictionary.Add(typeof(BoundsManager), new BoundsManager());
            managersDictionary.Add(typeof(AsteroidsManager), new AsteroidsManager());
            managersDictionary.Add(typeof(PlayerShipsManager), new PlayerShipsManager());
            managersDictionary.Add(typeof(EnemiesManager), new EnemiesManager());
            managersDictionary.Add(typeof(SoundManager), new SoundManager());
            managersDictionary.Add(typeof(VFXManager), new VFXManager());
            managersDictionary.Add(typeof(GameObjectsManager), new GameObjectsManager());
            
            
            foreach (KeyValuePair<Type, IManager> pair in managersDictionary)
            {
                pair.Value.Initialize(instance);
            }
        }


        public void Update()
        {
            foreach (KeyValuePair<Type, IManager> pair in managersDictionary)
            {
                pair.Value.Update();
            }
        }

        
        public void Unload()
        {
            foreach (KeyValuePair<Type, IManager> pair in managersDictionary)
            {
                pair.Value.Unload();
            }
        }

    
        public TManagerType GetManager<TManagerType>()
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
