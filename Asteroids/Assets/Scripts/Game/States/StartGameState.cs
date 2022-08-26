using Asteroids.Managers;


namespace Asteroids.Game
{
    public class StartGameState : IParametricState<GameType>
    {
        #region Fields

        private GameStateMachine gameStateMachine;
        private GameManager gameManager;

        #endregion



        #region Class lifecycle

        public StartGameState(GameStateMachine gameStateMachine, GameManager gameManager)
        {            
            this.gameStateMachine = gameStateMachine;
            this.gameManager = gameManager;
        }

        #endregion



        #region Public methods

        public void Enter(GameType parameter)
        {
            gameManager.SetGameplayType(parameter);
            gameManager.StartGame();
            
            gameStateMachine.EnterState<GameplayState, GameType>(parameter);
        }

        
        public void Exit() { }

        #endregion
    }
}
