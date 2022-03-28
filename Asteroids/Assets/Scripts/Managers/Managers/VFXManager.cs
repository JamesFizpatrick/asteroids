using Asteroids.Handlers;
using Asteroids.Managers;
using Asteroids.VFX;
using UnityEngine;


namespace Asteroids.Managers
{
    public class VFXManager : MonoBehaviour, IManager
    {
        #region Fields
    
        private static VFXManager instance;
        private DataManager dataManager;
        
        #endregion


        
        #region Properties

        public static VFXManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject managerGo = new GameObject("VFXManager");
                    VFXManager manager = managerGo.AddComponent<VFXManager>();
                    instance = manager;
                }

                return instance;
            }
        }

        #endregion


    
        #region Public methods

        public void SpawnVFX(VFXType type, Vector3 position)
        {
            VFX.VFX vfxPrefab = dataManager.VFXPreset.GetVFX(type);

            VFX.VFX vfx = Instantiate(vfxPrefab, GameSceneReferences.MainCanvas.transform);
            vfx.transform.localPosition = position;
        }
    
    
        public void Initialize()
        {
            dataManager = ManagersHub.GetManager<DataManager>();
        }

    
        public void Unload() { }

        #endregion
    }
}
