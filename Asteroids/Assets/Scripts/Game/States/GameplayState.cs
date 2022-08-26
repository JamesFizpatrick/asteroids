using Asteroids.Managers;
using Asteroids.UI;


namespace Asteroids.Game
{
    public class GameplayState : IParametricState<GameType>
    {
        #region Fields

        private GameStateMachine gameStateMachine;
        private GameManager gameManager;
        private UIManager uiManager;
        
        private GameType gameType;

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
        
        public void Enter(GameType parameter)
        {
            switch (parameter)
            {
                case GameType.Classic:
                    uiManager.ShowScreen<ClassicGameScreen>(gameManager.CurrentGameplayController() as ClassicGameplayController);
                    break;
                case GameType.Survival:
                    uiManager.ShowScreen<SurvivalGameScreen>(gameManager.CurrentGameplayController() as SurvivalGameplayController);
                    break;
            }
            
            gameType = parameter;
            
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
