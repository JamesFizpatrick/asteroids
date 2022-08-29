using Asteroids.Managers;


namespace Asteroids.Game
{
    public class SurvivalWinState : IState
    {
        #region Fields

        private readonly UIManager uiManager;
        private readonly GameStateMachine gameStateMachine;
        private readonly PlayerProgressManager progressManager;

        private SurvivalWinScreen survivalWinScreen;

        #endregion



        #region Class lyfecycle

        public SurvivalWinState(GameStateMachine gameStateMachine, UIManager uiManager,
            PlayerProgressManager progressManager)
        {
            this.gameStateMachine = gameStateMachine;
            this.uiManager = uiManager;
            this.progressManager = progressManager;
        }

        #endregion


        
        #region Public methods

        public void Enter()
        {
            survivalWinScreen = uiManager.ShowScreen<SurvivalWinScreen>(progressManager);
            survivalWinScreen.OnClose += Screen_OnClose;
        }
        
        
        public void Exit() { }

        #endregion
        
        
       
        #region Events handler

        private void Screen_OnClose()
        {
            survivalWinScreen.OnClose -= Screen_OnClose;
            gameStateMachine.EnterState<MainMenuState, GameType>(GameType.None);
        }

        #endregion
    }
}