using Asteroids.Managers;


namespace Asteroids.Game
{
    public class GameplayState : IParametricState<GameType>
    {
        #region Fields

        private GameStateMachine gameStateMachine;
        private GameManager gameManager;
        
        private GameType gameType;

        #endregion



        #region Class lifecycle

        public GameplayState(GameStateMachine gameStateMachine, GameManager gameManager)
        {
            this.gameStateMachine = gameStateMachine;
            this.gameManager = gameManager;
        }

        #endregion



        #region Public methods

        public void Enter()
        {
            gameManager.OnPlayerWin += GameManager_OnPLayerWin;
            gameManager.OnPlayerLose += GameManager_OnPlayerLose;
        }


        public void Enter(GameType parameter)
        {
            gameType = parameter;
        }

        public void Exit()
        {
            gameManager.OnPlayerWin -= GameManager_OnPLayerWin;
            gameManager.OnPlayerLose -= GameManager_OnPlayerLose;
        }

        #endregion


        #region Event handlers

        private void GameManager_OnPLayerWin()
        {
            gameManager.StopGame();
            gameStateMachine.EnterState<InterWinState, GameType>(gameType);
        }
        
        
        private void GameManager_OnPlayerLose()
        {
            gameManager.StopGame();
            gameStateMachine.EnterState<InterLoseState, GameType>(gameType);
        }
        
        #endregion
    }
}
