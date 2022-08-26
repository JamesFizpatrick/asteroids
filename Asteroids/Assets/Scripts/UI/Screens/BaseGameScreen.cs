using System;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class BaseGameScreen : BaseScreen
    {
        #region Fields
        
        public Action OnPauseButtonClick;
        [SerializeField] private Button pauseButton;
        
        #endregion


        
        #region Unity lifecycle

        private void OnEnable() => pauseButton.onClick.AddListener(PauseButton_OnClick);


        private void OnDisable() => pauseButton.onClick.RemoveListener(PauseButton_OnClick);

        #endregion



        #region Event handlers

        private void PauseButton_OnClick() => OnPauseButtonClick?.Invoke();

        #endregion
    }
}
