using Asteroids.Managers;


namespace Asteroids.Game
{
    public class StartGameState : IParametricState<GameType>
    {
        #region Fields

        private readonly GameStateMachine gameStateMachine;
        private readonly IGameManager gameManager;

        #endregion



        #region Class lifecycle

        public StartGameState(GameStateMachine gameStateMachine, IGameManager gameManager)
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
