using Asteroids.Managers;
using Asteroids.UI;


namespace Asteroids.Game
{
    public class GameplayState : IParametricState<GameType>
    {
        #region Fields

        private readonly GameStateMachine gameStateMachine;
        private readonly IGameManager gameManager;
        private readonly IUiManager uiManager;
        private readonly IPlayerShipsManager playerShipsManager;

        private GameType gameType;
        private BaseGameScreen gameScreen;

        #endregion



        #region Class lifecycle

        public GameplayState(GameStateMachine gameStateMachine, IGameManager gameManager, IUiManager uiManager,
            IPlayerShipsManager playerShipsManager)
        {
            this.gameStateMachine = gameStateMachine;
            this.gameManager = gameManager;
            this.playerShipsManager = playerShipsManager;
            this.uiManager = uiManager;
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

            gameScreen.InitHealthBar(playerShipsManager);
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
            BaseScreen screen = uiManager.ShowScreen<TGameScreen>(gameManager.GetCurrentGameplayController());
            return (BaseGameScreen)screen;
        }

        #endregion

        
        
        #region Event handlers

        private void GameManager_OnPLayerWin()
        {
            gameManager.StopGame();

            switch (gameType)
            {
                case GameType.Classic:
                    gameStateMachine.EnterState<InterWinState, GameType>(gameType);
                    break;
                case GameType.Survival:
                    gameStateMachine.EnterState<SurvivalWinState>();
                    break;
            }
        }
        
        
        private void GameManager_OnPlayerLose()
        {
            gameManager.StopGame();
            gameStateMachine.EnterState<InterLoseState, GameType>(gameType);
        }


        private void GameScreen_OnPauseButtonClick() => gameStateMachine.EnterState<PauseState, GameType>(gameType);

        #endregion
    }
}
