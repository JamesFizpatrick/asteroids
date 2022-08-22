using Asteroids.Managers;

namespace Asteroids.Game
{
    public class GameplayState : IState
    {
        #region Fields

        private GameStateMachine gameStateMachine;
        private IManagersHub managersHub;

        #endregion



        #region Class lifecycle

        public GameplayState(GameStateMachine gameStateMachine, IManagersHub managersHub)
        {
            this.gameStateMachine = gameStateMachine;
            this.managersHub = managersHub;
        }

        #endregion



        #region Public methods

        public void Enter()
        {
            // Subscribe to events
        }

        public void Exit() { }

        #endregion
    }
}
