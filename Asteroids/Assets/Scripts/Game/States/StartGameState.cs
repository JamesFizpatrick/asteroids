using Asteroids.Managers;
using Asteroids.UI;


namespace Asteroids.Game
{
    public class StartGameState : IState
    {
        #region Fields

        private GameStateMachine gameStateMachine;
        private UIManager uiManager;
        private GameManager gameManager;

        #endregion



        #region Class lifecycle

        public StartGameState(GameStateMachine gameStateMachine, UIManager uiManager, GameManager gameManager)
        {            
            this.gameStateMachine = gameStateMachine;
            this.uiManager = uiManager;
            this.gameManager = gameManager;
        }

        #endregion



        #region Public methods

        public void Enter()
        {
            uiManager.ShowScreen<GameScreen>();
            gameManager.StartGame();
            gameStateMachine.EnterState<GameplayState>();
        }


        public void Exit() { }

        #endregion
    }
}
