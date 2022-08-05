using System;
using System.Collections.Generic;
using System.Linq;


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
            IEnumerable<Type> managerEntites = GetAllManagerEntities();
          
            foreach (Type managerType in managerEntites)
            {
                AddManager(managerType);
            }
            
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

        private void AddManager(Type managerType)
        {
            object newManager = Activator.CreateInstance(managerType);

            managers.Add(managerType, newManager as IManager);

            if (newManager is IUpdatableManager updatableManager)
            {
                updatableManagers.Add(updatableManager);
            }

            if (newManager is IUnloadableManager unloadableManager)
            {
                unloadableManagers.Add(unloadableManager);
            }
        }


        private IEnumerable<Type> GetAllManagerEntities()
        {
            System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            IEnumerable<Type> types = assemblies.SelectMany(x => x.GetTypes());
            IEnumerable<Type> selectedTypes = types.Where(x => typeof(IManager).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
            
            return selectedTypes.ToList();                        
        }

        #endregion
    }
}
