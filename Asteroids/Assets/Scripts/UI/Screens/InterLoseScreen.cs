using System;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class InterLoseScreen : BaseScreen
    {
        #region Fields

        public Action OnReset;
        public Action OnExitToMainMenu;
        
        [SerializeField] private Button resetButton;
        [SerializeField] private Button mainMenuButton;
        
        #endregion



        #region Unity lifecycle
        
        private void OnEnable()
        {           
            resetButton.onClick.AddListener(ResetButton_OnClick);
            mainMenuButton.onClick.AddListener(MainMenuButton_OnClick);
        }


        private void OnDisable()
        {
            resetButton.onClick.RemoveListener(ResetButton_OnClick);
            mainMenuButton.onClick.RemoveListener(MainMenuButton_OnClick);
        }

        #endregion



        #region Event handlers

        private void ResetButton_OnClick()
        {
            OnReset?.Invoke();
            CloseScreen();
        }
    
        
        private void MainMenuButton_OnClick()
        {
            OnExitToMainMenu?.Invoke();
            CloseScreen();
        }

        #endregion
    }
}
