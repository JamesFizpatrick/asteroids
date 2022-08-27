using Asteroids.Game;
using Asteroids.Managers;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class MenuScreen : BaseScreen
    {
        #region Fields

        [SerializeField] private Button continueButton;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button controlsButton;
        [SerializeField] private Button settingsButton;

        private UIManager uiManager;
        private PlayerProgressManager progressManager;
        private GameStateMachine stateMachine;

        #endregion



        #region Unity lifecycle

        private void OnEnable()
        {
            newGameButton.onClick.AddListener(NewGameButton_OnClick);
            controlsButton.onClick.AddListener(ControlsButton_OnClick);
            settingsButton.onClick.AddListener(SettingsButton_OnClick);
            continueButton.onClick.AddListener(ContinueButton_OnClick);
        }
        
        
        private void OnDisable()
        {
            newGameButton.onClick.RemoveListener(NewGameButton_OnClick);
            controlsButton.onClick.RemoveListener(ControlsButton_OnClick);
            settingsButton.onClick.RemoveListener(SettingsButton_OnClick);
            continueButton.onClick.RemoveListener(ContinueButton_OnClick);
        }


        private void Start()
        {
            uiManager = ManagersHub.GetManager<UIManager>();
            progressManager = ManagersHub.GetManager<PlayerProgressManager>();

            if (Parameter != null)
            {
                // TODO: Should be more clear
                stateMachine = Parameter as GameStateMachine;
            }

            continueButton.gameObject.SetActive(progressManager.HasPreviousProgress());
        }

        #endregion



        #region Event handlers

        private void NewGameButton_OnClick()
        {
            //TODO: Reset progress
            progressManager.ResetProgress();
            stateMachine?.EnterState<StartGameState, GameType>(GameType.Classic);
        }


        private void SettingsButton_OnClick() => uiManager.ShowScreen<SettingsScreen>();


        private void ControlsButton_OnClick() => uiManager.ShowScreen<ControlsScreen>();

        
        private void ContinueButton_OnClick() => stateMachine?.EnterState<StartGameState, GameType>(GameType.Classic);

        #endregion
    }
}
