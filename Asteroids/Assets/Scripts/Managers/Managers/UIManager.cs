using System;
using Asteroids.Handlers;
using Asteroids.UI;
using UnityEngine;


namespace Asteroids.Managers
{
    public class UIManager : IManager, IUnloadableManager
    {
        #region Fields

        private HealthBar healthBar;
        private BaseScreen currentScreen;

        private IManagersHub managersHub;
        private GameManager gameManager;

        #endregion



        #region Public methods

        public void Initialize(IManagersHub hub)
        {
            managersHub = hub;
            gameManager = hub.GetManager<GameManager>();

            gameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        }


        public void Unload()
        {
            if (gameManager != null)
            {
                gameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
            }
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



        #region Private methods

        public void ShowHealthBar()
        {
            if (healthBar == null)
            {
                healthBar = GameObject.Instantiate(DataContainer.UiPreset.HealthBar,
                                GameSceneReferences.MainCanvas.transform);
                healthBar.Init(managersHub);
            }

            healthBar.gameObject.SetActive(true);
        }


        public void HideHealthBar()
        {
            if (healthBar != null)
            {
                healthBar.gameObject.SetActive(false);
            }
        }

        #endregion



        #region Event handlers

        private void GameManager_OnGameStateChanged(GameState state)
        {
            if (state == GameState.InGame)
            {
                ShowHealthBar();
            }
            else
            {
                HideHealthBar();
            }
        }

        #endregion
    }
}
