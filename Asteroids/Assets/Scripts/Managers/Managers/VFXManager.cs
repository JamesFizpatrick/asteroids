using Asteroids.VFX;
using UnityEngine;


namespace Asteroids.Managers
{
    public class VFXManager : IManager
    {
        #region Fields
    
        private GameObjectsManager gameObjectsManager;
        
        #endregion

        
        
        #region Public methods

        public void SpawnVFX(VFXType type, Vector3 position)
        {
            GameObject vfxPrefab = DataContainer.VFXPreset.GetVFX(type);

            VFX.VFX vfx = gameObjectsManager.CreateVFX(vfxPrefab).GetComponent<VFX.VFX>();
            vfx.transform.localPosition = position;
        }
    
    
        public void Initialize(ManagersHub hub)
        {
            gameObjectsManager = hub.GetManager<GameObjectsManager>();
        }

        
        public void Update() { }


        public void Unload() { }

        #endregion
    }
}
