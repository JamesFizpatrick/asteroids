using Asteroids.Managers;
using Asteroids.UI;


namespace Asteroids.Game
{
    public class InterWinState : IState
    {
        #region Fields

        private readonly UIManager uiManager;
        private readonly GameStateMachine gameStateMachine;
        
        private BaseScreen interScreen;
        
        #endregion
        
        
        #region Class lifecycle

        public InterWinState(GameStateMachine gameStateMachine, UIManager uiManager)
        {
            this.gameStateMachine = gameStateMachine;
            this.uiManager = uiManager;
        }

        #endregion


        
        #region Public methods

        public void Enter()
        {
            interScreen = uiManager.ShowScreen<InterWinScreen>();
            interScreen.OnClose += Screen_OnClose;
        }
        
        
        public void Exit()
        {
        }
        
        #endregion


        #region Events handler

        private void Screen_OnClose()
        {
            interScreen.OnClose -= Screen_OnClose;
            gameStateMachine.EnterState<StartGameState>();
        }

        #endregion
    }
}
