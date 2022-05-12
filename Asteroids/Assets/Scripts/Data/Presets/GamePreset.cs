using System;
using UnityEngine;


namespace Asteroids.Data
{
    [CreateAssetMenu(fileName = "CustomAssets/GamePreset")]
    public class GamePreset : ScriptableObject
    {
        #region Nested types

        [Serializable]
        public struct LevelPreset
        {
            public int AsteroidsCount;
            public int EnemiesCount;
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
