using System;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class PauseScreen : BaseScreen
    {
        #region Fields

        public Action OnResumeButtonPressed;
        public Action OnMainMenuButtonPressed;
        
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button mainMenuButton;
        
        #endregion



        #region Unity lifecycle

        private void OnEnable()
        {
            resumeButton.onClick.AddListener(ResumeButton_OnClick);
            mainMenuButton.onClick.AddListener(MainMenuButton_OnClick);
        }


        private void OnDisable()
        {
            resumeButton.onClick.RemoveListener(ResumeButton_OnClick);
            mainMenuButton.onClick.RemoveListener(MainMenuButton_OnClick);
        }

        #endregion



        #region Event handlers

        private void ResumeButton_OnClick() => OnResumeButtonPressed?.Invoke();


        private void MainMenuButton_OnClick() => OnMainMenuButtonPressed?.Invoke();

        #endregion
    }
}
