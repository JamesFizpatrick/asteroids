using Asteroids.UI;
using UnityEngine;
using System.Linq;


namespace Asteroids.Data
{
    [CreateAssetMenu(fileName = "CustomAssets/UiPreset")]
    public class UiPreset : ScriptableObject
    {
        #region Fields

        [Header("UI elements")]
        [SerializeField] private HealthBar healthBar;

        [Header("Screens")]
        [SerializeField] private BaseScreen[] screenPrefabs;

        #endregion



        #region Properties

        public HealthBar HealthBar => healthBar;


        public BaseScreen GetScreen<TScreenType>() =>
            screenPrefabs.FirstOrDefault(x => x.GetType() == typeof(TScreenType));

        #endregion
    }
}
