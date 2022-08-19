using Asteroids.Game;
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

        private UIManager uiManager;

        private GameStateMachine stateMachine;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            ScreenType = ScreenType.Start;

            startButton.onClick.AddListener(StartButton_OnClick);
            controlsButton.onClick.AddListener(ControlsButton_OnClick);
            settingsButton.onClick.AddListener(SettingsButton_OnClick);
        }


        private void Start() =>
            uiManager = ManagersHub.Instance.GetManager<UIManager>();

        #endregion



        #region Public methods

        public override void Init(object parameter)
        {
            stateMachine = parameter as GameStateMachine;
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

        private void StartButton_OnClick() => stateMachine.EnterState<StartGameState>();


        private void SettingsButton_OnClick() => uiManager.ShowScreen(ScreenType.Settings);


        private void ControlsButton_OnClick() => uiManager.ShowScreen(ScreenType.Controls);

        #endregion
    }
}
