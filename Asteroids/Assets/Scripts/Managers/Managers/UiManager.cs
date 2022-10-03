using Asteroids.Data;
using Asteroids.Handlers;
using Asteroids.UI;
using UnityEngine;


namespace Asteroids.Managers
{
    public class UiManager : IUiManager
    {
        #region Fields

        private BaseScreen currentScreen;

        #endregion



        #region Public methods

        public void Initialize(IManagersHub hub) { }


        public TScreenType ShowScreen<TScreenType>(object parameter = null) where TScreenType : BaseScreen
        {
            if (currentScreen != null)
            {
                currentScreen.CloseScreen();
            }

            BaseScreen screenGO = DataContainer.UiPreset.GetScreen<TScreenType>();
            currentScreen = GameObject.Instantiate(screenGO, GameSceneReferences.MainCanvas.transform);

            currentScreen.Init(parameter);

            return (TScreenType)currentScreen;
        }
        
        #endregion
    }
}
