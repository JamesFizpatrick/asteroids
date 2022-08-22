using Asteroids.Managers;
using Asteroids.UI;


namespace Asteroids.Game
{
    public class MainMenuState : IState
    {
        #region Fields

        private GameStateMachine gameStateMachine;
        private UIManager uiManager;

        #endregion



        #region Class lifecycle

        public MainMenuState(GameStateMachine gameStateMachine, UIManager uiManager)
        {
            this.gameStateMachine = gameStateMachine;
            this.uiManager = uiManager;
        }

        #endregion



        #region Public methods

        public void Enter()
        {
            uiManager.ShowScreen<MenuScreen>(gameStateMachine);
        }


        public void Exit() {}

        #endregion
    }
}
