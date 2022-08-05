using System.Collections.Generic;
using Asteroids.VFX;
using UnityEngine;


namespace Asteroids.Managers
{
    public class VFXManager : IManager
    {
        #region Fields
    
        private GameObjectsManager gameObjectsManager;
        private List<VFX.VFX> currentVFXes = new List<VFX.VFX>();
        
        #endregion

        
        
        #region Public methods

        public void SpawnVFX(VFXType type, Vector3 position)
        {
            GameObject vfxPrefab = DataContainer.VFXPreset.GetVFX(type);

            VFX.VFX vfx =
                gameObjectsManager.CreateVFX(vfxPrefab).GetComponent<VFX.VFX>();

            vfx.transform.localPosition = position;
            vfx.Destroyed += VFX_Destroyed;
            
            currentVFXes.Add(vfx);
        }
    
    
        public void Initialize(IManagersHub hub) =>
            gameObjectsManager = hub.GetManager<GameObjectsManager>();


        public void Reset()
        {
            foreach (VFX.VFX vfx in currentVFXes)
            {
                Object.Destroy(vfx.gameObject);
            }
        }
        
        #endregion


        
        #region Private methods

        private void VFX_Destroyed(VFX.VFX vfx)
        {
            currentVFXes.Remove(vfx);
            vfx.Destroyed -= VFX_Destroyed;
        }

        #endregion
    }
}
