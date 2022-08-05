using System;
using Asteroids.Handlers;
using Asteroids.UI;
using UnityEngine;


namespace Asteroids.Managers
{
    public class UIManager : IManager
    {
        private HealthBar healthBar;
        private BaseScreen currentScreen;
        
        public void Initialize(IManagersHub hub)
        {
            healthBar = GameObject.Instantiate(DataContainer.PlayerPreset.HealthBar,
                GameSceneReferences.MainCanvas.transform);
            healthBar.Init(hub);
        }
  
                   
        public void ShowScreen(ScreenType screenType, Action onClose = null)
        {
            BaseScreen screenGO = DataContainer.PlayerPreset.GetScreen(screenType);
            currentScreen = GameObject.Instantiate(screenGO,
                GameSceneReferences.MainCanvas.transform);

            currentScreen.OnClose += () => onClose?.Invoke();
        }
    }
}
