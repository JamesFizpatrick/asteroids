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
        
        public void Initialize(ManagersHub hub)
        {
            healthBar = GameObject.Instantiate(DataContainer.PlayerPreset.HealthBar,
                GameSceneReferences.MainCanvas.transform);
            healthBar.Init(hub);
        }

    
        public void Update() { }

    
        public void Unload() { }

        
        
        public void ShowScreen(ScreenType screenType, Action onClose = null)
        {
            BaseScreen screenGO = DataContainer.PlayerPreset.GetScreen(screenType);
            currentScreen = GameObject.Instantiate(screenGO, GameSceneReferences.MainCanvas.transform);

            currentScreen.OnClose += () => onClose?.Invoke();
        }

        
        public void HideScreen()
        {
            if (currentScreen != null)
            {
                GameObject.Destroy(currentScreen.gameObject);
            }
        }
    }
}
