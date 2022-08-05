using System;
using System.Collections.Generic;


namespace Asteroids.Managers
{
    public class ManagersHub : IManagersHub
    {
        #region Fields

        private static ManagersHub instance;

        private Dictionary<Type, IManager> managers = new Dictionary<Type, IManager>();

        private List<IUpdatableManager> updatableManagers = new List<IUpdatableManager>();
        private List<IUnloadableManager> unloadableManagers = new List<IUnloadableManager>();

        #endregion



        #region Properties

        public static ManagersHub Instance => instance ??= new ManagersHub();

        #endregion


    
        #region Public methods

        public void Initialize()
        {
            //Add new services here
            AddManager(new InputManager());
            AddManager(new GameManager());
            AddManager(new BoundsManager());
            AddManager(new AsteroidsManager());
            AddManager(new PlayerShipsManager());
            AddManager(new EnemiesManager());
            AddManager(new SoundManager());
            AddManager(new VFXManager());
            AddManager(new GameObjectsManager());
            AddManager(new UIManager());
            
            foreach (KeyValuePair<Type, IManager> pair in managers)
            {
                pair.Value.Initialize(instance);
            }
        }


        public void Update()
        {
            foreach (IUpdatableManager manager in updatableManagers)
            {
                manager.Update();
            }
        }

        
        public void Unload()
        {
            foreach (IUnloadableManager manager in unloadableManagers)
            {
                manager.Unload();
            }
        }

    
        public TManagerType GetManager<TManagerType>()
        {
            Type managerType = typeof(TManagerType);

            IManager manager = managers[managerType];

            if (manager == null)
            {
                throw new NullReferenceException("There is no manager of type " + managerType + "!");
            }
        
            return (TManagerType)manager;
        }

        #endregion



        #region Private methods

        private void AddManager<TManager>(TManager manager)
        {
            managers.Add(typeof(TManager), manager as IManager);

            if (manager is IUpdatableManager updatableManager)
            {
                updatableManagers.Add(updatableManager);
            }

            if (manager is IUnloadableManager unloadableManager)
            {
                unloadableManagers.Add(unloadableManager);
            }
        }

        #endregion
    }
}
