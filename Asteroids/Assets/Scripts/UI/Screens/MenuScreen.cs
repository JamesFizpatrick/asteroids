using Asteroids.Game;
using Asteroids.Managers;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class MenuScreen : BaseScreen
    {
        #region Fields

        [SerializeField] private Button startButton;
        [SerializeField] private Button controlsButton;
        [SerializeField] private Button settingsButton;

        private UIManager uiManager;
        private GameStateMachine stateMachine;

        #endregion



        #region Unity lifecycle

        private void OnEnable()
        {
            startButton.onClick.AddListener(StartButton_OnClick);
            controlsButton.onClick.AddListener(ControlsButton_OnClick);
            settingsButton.onClick.AddListener(SettingsButton_OnClick);
        }


        private void OnDisable()
        {
            startButton.onClick.RemoveListener(StartButton_OnClick);
            controlsButton.onClick.RemoveListener(ControlsButton_OnClick);
            settingsButton.onClick.RemoveListener(SettingsButton_OnClick);
        }


        private void Start()
        {
            uiManager = managersHub.GetManager<UIManager>();

            if (parameter != null)
            {
                // TODO: Should be more clear
                stateMachine = parameter as GameStateMachine;
            }
        }

        #endregion



        #region Event handlers

        private void StartButton_OnClick() => stateMachine?.EnterState<StartGameState, GameType>(GameType.Classic);


        private void SettingsButton_OnClick() => uiManager.ShowScreen<SettingsScreen>();


        private void ControlsButton_OnClick() => uiManager.ShowScreen<ControlsScreen>();

        #endregion
    }
}
