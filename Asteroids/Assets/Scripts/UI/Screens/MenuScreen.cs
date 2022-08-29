using System;
using Asteroids.Game;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class MenuScreen : BaseScreen
    {
        #region Fields

        public Action<Type> OnSwitchToScreen;
        public Action<GameType> OnStartGame;
        public Action OnContinueGame;
        
        [SerializeField] private Button continueButton;
        [SerializeField] private NewGameButton newGameButton;
        [SerializeField] private Button controlsButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button leaderboardButton;
        
        #endregion



        #region Unity lifecycle

        private void OnEnable()
        {
            newGameButton.OnStartNewGame += NewGameButton_OnStartNewGame;
            
            controlsButton.onClick.AddListener(ControlsButton_OnClick);
            settingsButton.onClick.AddListener(SettingsButton_OnClick);
            continueButton.onClick.AddListener(ContinueButton_OnClick);
            leaderboardButton.onClick.AddListener(LeaderboardButton_OnClick);
        }

        
        private void OnDisable()
        {
            newGameButton.OnStartNewGame -= NewGameButton_OnStartNewGame;
            
            controlsButton.onClick.RemoveListener(ControlsButton_OnClick);
            settingsButton.onClick.RemoveListener(SettingsButton_OnClick);
            continueButton.onClick.RemoveListener(ContinueButton_OnClick);
            leaderboardButton.onClick.RemoveListener(LeaderboardButton_OnClick);
        }
        
        #endregion


        
        #region Protected methods

        protected override void Init()
        {
            bool showContinueButton = (bool)Parameter;
            continueButton.gameObject.SetActive(showContinueButton);
        }

        #endregion



        #region Event handlers

        private void NewGameButton_OnStartNewGame(GameType gameType) => OnStartGame?.Invoke(gameType);

        
        private void ContinueButton_OnClick() => OnContinueGame?.Invoke();

        
        private void SettingsButton_OnClick() => OnSwitchToScreen?.Invoke(typeof(SettingsScreen));


        private void ControlsButton_OnClick() => OnSwitchToScreen?.Invoke(typeof(ControlsScreen));

        
        private void LeaderboardButton_OnClick() => OnSwitchToScreen?.Invoke(typeof(HighscoreScreen));

        #endregion
    }
}
