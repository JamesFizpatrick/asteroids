using Asteroids.Handlers;
using Asteroids.VFX;
using UnityEngine;


namespace Asteroids.Managers
{
    public class VFXManager : BaseManager<VFXManager>
    {
        #region Fields
    
        private DataManager dataManager;
        private GameObjectsManager gameObjectsManager;
        
        #endregion

        
        
        #region Public methods

        public void SpawnVFX(VFXType type, Vector3 position)
        {
            GameObject vfxPrefab = dataManager.VFXPreset.GetVFX(type);

            VFX.VFX vfx = gameObjectsManager.CreateVFX(vfxPrefab).GetComponent<VFX.VFX>();
            vfx.transform.localPosition = position;
        }
    
    
        protected override void Initialize()
        {
            dataManager = ManagersHub.GetManager<DataManager>();
            gameObjectsManager = ManagersHub.GetManager<GameObjectsManager>();
        }

    
        protected override void Deinitialize() { }

        #endregion
    }
}
