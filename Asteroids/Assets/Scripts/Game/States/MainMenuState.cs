using Asteroids.Managers;
using Asteroids.UI;


namespace Asteroids.Game
{
    public class MainMenuState : IState
    {
        #region Fields

        private GameStateMachine gameStateMachine;

        #endregion



        #region Class lifecycle

        public MainMenuState(GameStateMachine gameStateMachine)
        {
            this.gameStateMachine = gameStateMachine;
        }

        #endregion



        #region Public methods

        public void Enter()
        {
            UIManager uiManager = ManagersHub.Instance.GetManager<UIManager>();
            uiManager.ShowScreen<MenuScreen>(gameStateMachine);
        }


        public void Exit() {}

        #endregion
    }
}
