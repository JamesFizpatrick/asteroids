using Asteroids.Managers;


namespace Asteroids.Game
{
    public class GameplayState : IState
    {
        #region Fields

        private GameStateMachine gameStateMachine;
        private GameManager gameManager;
        private UIManager uiManager;

        #endregion



        #region Class lifecycle

        public GameplayState(GameStateMachine gameStateMachine, GameManager gameManager, UIManager uiManager)
        {
            this.gameStateMachine = gameStateMachine;
            this.gameManager = gameManager;
            this.uiManager = uiManager;
        }

        #endregion



        #region Public methods

        public void Enter()
        {
            gameManager.OnPlayerWin += GameManager_OnPLayerWin;
            gameManager.OnPlayerLose += GameManager_OnPlayerLose;
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
            gameStateMachine.EnterState<InterWinState>();
        }
        
        
        private void GameManager_OnPlayerLose()
        {
            gameManager.StopGame();
            gameStateMachine.EnterState<InterLoseState>();
        }
        
        #endregion
    }
}
