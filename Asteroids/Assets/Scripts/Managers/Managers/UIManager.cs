using System;
using Asteroids.Handlers;
using Asteroids.UI;
using UnityEngine;


namespace Asteroids.Managers
{
    public class UIManager : IManager
    {
        #region Fields

        private BaseScreen currentScreen;
        private IManagersHub managersHub;

        #endregion



        #region Public methods

        public void Initialize(IManagersHub hub)
        {
            managersHub = hub;
        }


        public void ShowScreen(ScreenType screenType)
        {
            if (currentScreen != null)
            {
                currentScreen.CloseScreen();
            }

            BaseScreen screenGO = DataContainer.UiPreset.GetScreen(screenType);
            currentScreen = GameObject.Instantiate(screenGO,
                GameSceneReferences.MainCanvas.transform);
        }


        public void ShowScreen(ScreenType screenType, Action onClose)
        {
            if (currentScreen != null)
            {
                currentScreen.CloseScreen();
            }

            BaseScreen screenGO = DataContainer.UiPreset.GetScreen(screenType);
            currentScreen = GameObject.Instantiate(screenGO,
                GameSceneReferences.MainCanvas.transform);

            currentScreen.OnClose += () => onClose?.Invoke();
        }


        #endregion
    }
}
