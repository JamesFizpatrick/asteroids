using System.Collections.Generic;
using Asteroids.Data;
using Asteroids.VFX;
using UnityEngine;


namespace Asteroids.Managers
{
    public class VfxManager : IVfxManager
    {
        #region Fields
    
        private IGameObjectsManager gameObjectsManager;
        private IGameManager gameManager;
        
        private List<VFX.VFX> currentVFXes = new List<VFX.VFX>();

        #endregion

        
        
        #region Public methods

        public void SpawnVFX(VFXType type, Vector3 position)
        {
            GameObject vfxPrefab = DataContainer.VFXPreset.GetVFX(type);

            VFX.VFX vfx = gameObjectsManager.CreateVFX(vfxPrefab).GetComponent<VFX.VFX>();

            vfx.transform.localPosition = position;
            vfx.Destroyed += VFX_Destroyed;
            
            currentVFXes.Add(vfx);
        }


        public void Initialize(IManagersHub hub)
        {
            gameObjectsManager = hub.GetManager<IGameObjectsManager>();
            gameManager = hub.GetManager<IGameManager>();

            gameManager.OnPlayerLose += GameManager_OnPlayerLose;
            gameManager.OnPlayerWin += GameManager_OnPlayerWin;
        }

        
        public void Reset()
        {
            foreach (VFX.VFX vfx in currentVFXes)
            {
                Object.Destroy(vfx.gameObject);
            }
        }
        
        
        public void Unload()
        {
            Reset();

            if (gameManager != null)
            {
                gameManager.OnPlayerLose += GameManager_OnPlayerLose;
                gameManager.OnPlayerWin += GameManager_OnPlayerWin;
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


        
        #region Event handlers

        private void GameManager_OnPlayerWin() => Reset();

        
        private void GameManager_OnPlayerLose() => Reset();

        #endregion
    }
}
