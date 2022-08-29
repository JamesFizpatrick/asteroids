using Asteroids.Managers;
using Asteroids.UI;


namespace Asteroids.Game
{
    public class PauseState : IParametricState<GameType>
    {
        #region Fields

        private readonly GameStateMachine gameStateMachine;
        private readonly GameManager gameManager;
        private readonly UIManager uiManager;
        
        private PauseScreen pauseScreen;
        
        private GameType gameType;

        #endregion


        
        #region Class lifecycle

        public PauseState(GameStateMachine gameStateMachine, GameManager gameManager, UIManager uiManager)
        {
            this.gameStateMachine = gameStateMachine;
            this.gameManager = gameManager;
            this.uiManager = uiManager;
        }

        #endregion



        #region Public methods

        public void Enter(GameType gameType)
        {
            this.gameType = gameType;

            pauseScreen = uiManager.ShowScreen<PauseScreen>();
            gameManager.SetPause(true);
            
            pauseScreen.OnResumeButtonPressed += PauseScreen_OnResumeButtonPressed;
            pauseScreen.OnMainMenuButtonPressed += PauseScreen_OnMainMenuButtonPressed;
        }

        
        public void Exit()
        {
            pauseScreen.OnResumeButtonPressed -= PauseScreen_OnResumeButtonPressed;
            pauseScreen.OnMainMenuButtonPressed -= PauseScreen_OnMainMenuButtonPressed;
            
            gameManager.SetPause(false);
        }

        #endregion


        
        #region Event screen

        private void PauseScreen_OnMainMenuButtonPressed()
        {
            gameManager.StopGame();
            gameStateMachine.EnterState<MainMenuState, GameType>(gameType);
        }

        
        private void PauseScreen_OnResumeButtonPressed() => gameStateMachine.EnterState<GameplayState, GameType>(gameType);

        #endregion
    }
}
