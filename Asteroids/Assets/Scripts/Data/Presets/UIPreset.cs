using System;
using Asteroids.UI;
using UnityEngine;
using System.Linq;


namespace Asteroids.Data
{
    [CreateAssetMenu(fileName = "CustomAssets/UiPreset")]
    public class UiPreset : ScriptableObject
    {
        #region Nested types

        [Serializable]
        private struct Screen
        {
            public ScreenType type;
            public BaseScreen screen;
        }

        #endregion



        #region Fields

        [Header("UI")]
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private Screen[] screens;

        #endregion



        #region Properties
  
        public HealthBar HealthBar => healthBar;


        public BaseScreen GetScreen(ScreenType type) => screens.FirstOrDefault(x => x.type == type).screen;

        #endregion
    }
}
