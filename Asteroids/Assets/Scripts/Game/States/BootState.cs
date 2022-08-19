using Asteroids.Managers;


namespace Asteroids.Game
{
    public class BootState : IState
    {
        #region Fields

        private GameStateMachine gameStateMachine;

        #endregion



        #region Class lifecycle

        public BootState(GameStateMachine gameStateMachine)
        {
            this.gameStateMachine = gameStateMachine;
        }

        #endregion



        #region Public methods

        public void Enter()
        {
            ManagersHub.Instance.Initialize();
            gameStateMachine.EnterState<MainMenuState>();
        }

        public void Exit() { }

        #endregion
    }
}
