using Asteroids.Managers;
using Asteroids.UI;


namespace Asteroids.Game
{
    public class InterState : IState
    {
        #region Fields

        private UIManager uiManager;
        private GameStateMachine gameStateMachine;

        #endregion
        
        
        #region Class lifecycle

        public InterState(GameStateMachine gameStateMachine, UIManager uiManager)
        {
            this.gameStateMachine = gameStateMachine;
            this.uiManager = uiManager;
        }

        #endregion


        
        #region Public methods

        public void Enter()
        {
            BaseScreen screen = uiManager.ShowScreen<InterScreen>();
            screen.OnClose += () => gameStateMachine.EnterState<StartGameState>();
        }
        
        
        public void Exit()
        {
            
        }
        
        #endregion
    }
}

