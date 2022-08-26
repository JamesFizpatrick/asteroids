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


        public TScreenType ShowScreen<TScreenType>(object parameter = null) where TScreenType : BaseScreen
        {
            if (currentScreen != null)
            {
                currentScreen.CloseScreen();
            }

            BaseScreen screenGO = DataContainer.UiPreset.GetScreen<TScreenType>();
            currentScreen = GameObject.Instantiate(screenGO,
                GameSceneReferences.MainCanvas.transform);

            currentScreen.Init(managersHub, parameter);

            return (TScreenType)currentScreen;
        }
        
        #endregion
    }
}
