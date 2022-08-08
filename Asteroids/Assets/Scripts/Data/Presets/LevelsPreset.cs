using System;
using UnityEngine;


namespace Asteroids.Data
{
    [CreateAssetMenu(fileName = "CustomAssets/LevelsPreset")]
    public class LevelsPreset : ScriptableObject
    {
        #region Nested types

        [Serializable]
        public struct LevelPreset
        {
            public int AsteroidsCount;
            public int EnemiesDelay;
        }

        #endregion


        
        #region Fields

        [SerializeField] private LevelPreset[] levelPresets;
        
        #endregion


        
        #region Public methods
        
        public LevelPreset GetLevelPreset(int index) => levelPresets[index];

        
        public LevelPreset[] GetLevelPresets() => levelPresets;

        #endregion
    }
}
