using Asteroids.Managers;
using Asteroids.UI;


namespace Asteroids.Game
{
    public class InterLoseState : IParametricState<GameType>
    {
        #region Fields

        private readonly GameStateMachine gameStateMachine;
        private readonly UIManager uiManager;
        private readonly GameManager gameManager;
        
        private InterLoseScreen screen;
        
        private GameType gameType;

        #endregion


        #region Class lifecycle

        public InterLoseState(GameStateMachine gameStateMachine, UIManager uiManager, GameManager gameManager)
        {
            this.gameStateMachine = gameStateMachine;
            this.uiManager = uiManager;
            this.gameManager = gameManager;
        }

        #endregion

        

        #region Public methods

        public void Enter(GameType parameter)
        {
            gameType = parameter;
            
            screen = (InterLoseScreen)uiManager.ShowScreen<InterLoseScreen>();
            
            screen.OnReset += Screen_OnReset;
            screen.OnExitToMainMenu += Screen_OnExitToMainMenu;
        }

        
        public void Exit()
        {
            if (screen != null)
            {
                screen.OnReset -= Screen_OnReset;
                screen.OnExitToMainMenu -= Screen_OnExitToMainMenu;
            }
        }

        #endregion

        

        #region Private methods

        private void UnsubscribeAll()
        {
            screen.OnReset -= Screen_OnReset;
            screen.OnExitToMainMenu -= Screen_OnExitToMainMenu;
        }

        private void RestartGame()
        {
            gameManager.Reset();
            uiManager.ShowScreen<GameScreen>();
            gameManager.StartGame();
            gameStateMachine.EnterState<GameplayState, GameType>(gameType);
        }


        private void ReturnToMainMenu()
        {
            uiManager.ShowScreen<MenuScreen>();
            gameStateMachine.EnterState<MainMenuState>();
        }
        
        #endregion


        
        #region Event handlers

        private void Screen_OnExitToMainMenu()
        {
            UnsubscribeAll();
            ReturnToMainMenu();
        }

        
        private void Screen_OnReset()
        {
            UnsubscribeAll();
            RestartGame();
        }
        
        #endregion
    }
}
