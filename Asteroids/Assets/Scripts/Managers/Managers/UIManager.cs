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

        public void Initialize(IManagersHub hub) => managersHub = hub;


        public BaseScreen ShowScreen<TScreenType>(object parameter = null)
        {
            if (currentScreen != null)
            {
                currentScreen.CloseScreen();
            }

            BaseScreen screenGO = DataContainer.UiPreset.GetScreen<TScreenType>();
            currentScreen = GameObject.Instantiate(screenGO,
                GameSceneReferences.MainCanvas.transform);

            currentScreen.Init(managersHub, parameter);

            return currentScreen;
        }


        // TODO: Remove callback
        public void ShowScreen<TScreenType>(Action onClose, object parameter = null)
        {
            if (currentScreen != null)
            {
                currentScreen.CloseScreen();
            }

            BaseScreen screenGO = DataContainer.UiPreset.GetScreen<TScreenType>();
            currentScreen = GameObject.Instantiate(screenGO,
                GameSceneReferences.MainCanvas.transform);

            currentScreen.OnClose += () => onClose?.Invoke();
        }

        #endregion
    }
}
