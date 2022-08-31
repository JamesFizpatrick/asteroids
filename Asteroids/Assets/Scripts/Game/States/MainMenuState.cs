using Asteroids.Managers;
using Asteroids.UI;
using System;


namespace Asteroids.Game
{
    public class MainMenuState : IParametricState<GameType>
    {
        #region Fields

        private readonly GameStateMachine gameStateMachine;
        private readonly IUiManager uiManager;
        
        private MenuScreen menuScreen;
        private BaseScreen auxScreen;

        private GameType gameType;
        
        #endregion



        #region Class lifecycle

        public MainMenuState(GameStateMachine gameStateMachine, IUiManager uiManager)
        {
            this.gameStateMachine = gameStateMachine;
            this.uiManager = uiManager;
        }

        #endregion



        #region Public methods

        public void Enter(GameType gameType)
        {
            this.gameType = gameType;
            ShowMenuScreen();
        }


        public void Exit()
        {
            if (menuScreen != null)
            {
                UnsibscribeFromMainMenu();
            }
        }
        
        #endregion


        
        #region Private methods

        private void ShowMenuScreen()
        {
            bool showContinueButton = gameType != GameType.None;
            menuScreen = uiManager.ShowScreen<MenuScreen>(showContinueButton);

            SubscribeOnMainMenu();
        }

        
        private void AuxScreen_OnClose()
        {
            auxScreen.OnClose -= AuxScreen_OnClose;
            ShowMenuScreen();
        }


        private void SubscribeOnMainMenu()
        {
            menuScreen.OnSwitchToScreen += MenuScreen_OnSwitchToScreen;
            menuScreen.OnStartGame += MenuScreen_OnStartGame;
            menuScreen.OnContinueGame += MenuScreen_OnContinueGame; 
        }


        private void UnsibscribeFromMainMenu()
        {
            menuScreen.OnSwitchToScreen -= MenuScreen_OnSwitchToScreen;
            menuScreen.OnStartGame -= MenuScreen_OnStartGame;
            menuScreen.OnContinueGame -= MenuScreen_OnContinueGame; 
        }
        
        #endregion

        
        
        #region Event handlers

        private void MenuScreen_OnSwitchToScreen(Type type)
        {
            UnsibscribeFromMainMenu();

            if (type == typeof(SettingsScreen))
            {
                auxScreen = uiManager.ShowScreen<SettingsScreen>();
            }
            else if (type == typeof(ControlsScreen))
            {
                auxScreen = uiManager.ShowScreen<ControlsScreen>();
            }
            else if (type == typeof(HighscoreScreen))
            {
                auxScreen = uiManager.ShowScreen<HighscoreScreen>();
            }

            auxScreen.OnClose += AuxScreen_OnClose;
        }
        

        private void MenuScreen_OnStartGame(GameType inputType) =>
            gameStateMachine.EnterState<StartGameState, GameType>(inputType);


        private void MenuScreen_OnContinueGame() =>
            gameStateMachine.EnterState<StartGameState, GameType>(gameType);

        #endregion
    }
}
