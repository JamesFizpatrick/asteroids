using Asteroids.Managers;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class PauseScreen : BaseScreen
    {
        #region Fields

        [SerializeField] private Button resumeButton;
        [SerializeField] private Button mainMenuButton;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            ScreenType = ScreenType.Pause;

            resumeButton.onClick.AddListener(ResumeButton_OnClick);
            mainMenuButton.onClick.AddListener(MainMenuButton_OnClick);
        }

        #endregion



        #region Protected methods

        protected override void PreClose()
        {
            resumeButton.onClick.RemoveListener(ResumeButton_OnClick);
            mainMenuButton.onClick.RemoveListener(MainMenuButton_OnClick);
        }

        #endregion



        #region Event handlers

        private void ResumeButton_OnClick()
        {
            UIManager uiManager = ManagersHub.Instance.GetManager<UIManager>();
            uiManager.ShowScreen(ScreenType.Game);
            //TODO: Set GameStateMachine to InGameState
        }



        private void MainMenuButton_OnClick()
        {
            UIManager uiManager = ManagersHub.Instance.GetManager<UIManager>();
            uiManager.ShowScreen(ScreenType.Start);
        }

        #endregion
    }
}

