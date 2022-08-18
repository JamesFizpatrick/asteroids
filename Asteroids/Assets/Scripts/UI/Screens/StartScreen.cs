using Asteroids.Managers;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class StartScreen : BaseScreen
    {
        #region Fields

        [SerializeField] private Button startButton;
        [SerializeField] private Button controlsButton;
        [SerializeField] private Button settingsButton;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            ScreenType = ScreenType.Start;

            startButton.onClick.AddListener(StartButton_OnClick);
            controlsButton.onClick.AddListener(ControlsButton_OnClick);
            settingsButton.onClick.AddListener(SettingsButton_OnClick);
        }
       
        #endregion



        #region Protected methods

        protected override void PreClose()
        {
            startButton.onClick.RemoveListener(StartButton_OnClick);
            controlsButton.onClick.RemoveListener(ControlsButton_OnClick);
            settingsButton.onClick.RemoveListener(SettingsButton_OnClick);
        }

        #endregion



        #region Event handlers

        private void StartButton_OnClick()
        {
            GameManager gameManager = ManagersHub.Instance.GetManager<GameManager>();
            gameManager.StartGame();

            UIManager uiManager = ManagersHub.Instance.GetManager<UIManager>();
            uiManager.ShowScreen(ScreenType.Game);
        }

        private void SettingsButton_OnClick()
        {
            UIManager uiManager = ManagersHub.Instance.GetManager<UIManager>();
            uiManager.ShowScreen(ScreenType.Settings);
        }


        private void ControlsButton_OnClick()
        {
            UIManager uiManager = ManagersHub.Instance.GetManager<UIManager>();
            uiManager.ShowScreen(ScreenType.Controls);
        }

        #endregion
    }
}
