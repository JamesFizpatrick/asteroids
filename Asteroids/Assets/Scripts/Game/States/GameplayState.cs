using Asteroids.Managers;
using Asteroids.UI;


namespace Asteroids.Game
{
    public class GameplayState : IParametricState<GameType>
    {
        #region Fields

        private readonly GameStateMachine gameStateMachine;
        private readonly GameManager gameManager;
        private readonly UIManager uiManager;
        private readonly PlayerProgressManager progressManager;

        private GameType gameType;

        private BaseGameScreen gameScreen;

        #endregion



        #region Class lifecycle

        public GameplayState(GameStateMachine gameStateMachine, GameManager gameManager, UIManager uiManager,
            PlayerProgressManager progressManager)
        {
            this.gameStateMachine = gameStateMachine;
            this.gameManager = gameManager;
            this.uiManager = uiManager;
            this.progressManager = progressManager;
        }

        #endregion



        #region Public methods
        
        public void Enter(GameType parameter)
        {
            switch (parameter)
            {
                case GameType.Classic:
                    gameScreen = CreateScreen<ClassicGameScreen>();
                    break;
                case GameType.Survival:
                    gameScreen = CreateScreen<SurvivalGameScreen>();
                    break;
            }

            gameScreen.OnPauseButtonClick += GameScreen_OnPauseButtonClick;
            
            gameType = parameter;
            
            gameManager.OnPlayerWin += GameManager_OnPLayerWin;
            gameManager.OnPlayerLose += GameManager_OnPlayerLose;
        }

        
        public void Exit()
        {
            gameManager.OnPlayerWin -= GameManager_OnPLayerWin;
            gameManager.OnPlayerLose -= GameManager_OnPlayerLose;

            if (gameScreen != null)
            {
                gameScreen.OnPauseButtonClick -= GameScreen_OnPauseButtonClick;
            }
        }

        #endregion


        
        #region MyRegion

        private BaseGameScreen CreateScreen<TGameScreen>()
            where TGameScreen : BaseGameScreen
        {
            BaseScreen screen = uiManager.ShowScreen<TGameScreen>(progressManager);
            return (BaseGameScreen)screen;
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


        private void GameScreen_OnPauseButtonClick()
        {
            gameStateMachine.EnterState<PauseState, GameType>(gameType);
        }

        #endregion
    }
}
