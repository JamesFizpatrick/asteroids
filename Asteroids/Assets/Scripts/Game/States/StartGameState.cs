using Asteroids.Managers;
using Asteroids.UI;


namespace Asteroids.Game
{
    public class StartGameState : IParametricState<GameType>
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

        public void Enter(GameType parameter)
        {
            uiManager.ShowScreen<GameScreen>();
            
            gameManager.SetGameplayType(parameter);
            gameManager.StartGame();
            
            gameStateMachine.EnterState<GameplayState, GameType>(parameter);
        }

        
        public void Exit() { }

        #endregion
    }
}
