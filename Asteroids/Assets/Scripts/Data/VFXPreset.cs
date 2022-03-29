using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.VFX;
using UnityEngine;


namespace Asteroids.Data
{
    [CreateAssetMenu(fileName = "CustomAssets/VFXPreset")]
    public class VFXPreset : ScriptableObject
    {
        #region Nested types

        [Serializable]
        private struct Preset
        {
            public VFXType Type;
            public GameObject Prefab;
        }

        #endregion


        
        #region Fields

        [SerializeField] private Preset[] presets;

        #endregion


        
        #region Public methods

        public GameObject GetVFX(VFXType vfxType, int index = -1)
        {
            List<Preset> presetList = presets.Where(x => x.Type == vfxType).ToList();

            if (presetList.Count == 0)
            {
                return null;
            }
            
            Preset preset = index >= 0 ? presetList[index] : presetList[0];

            return preset.Prefab;
        }

        #endregion
    }
}
