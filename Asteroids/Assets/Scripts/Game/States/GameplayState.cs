namespace Asteroids.Game
{
    public class GameplayState : IState
    {
        #region Fields

        private GameStateMachine gameStateMachine;

        #endregion



        #region Class lifecycle

        public GameplayState(GameStateMachine gameStateMachine)
        {
            this.gameStateMachine = gameStateMachine;
        }

        #endregion



        #region Public methods

        public void Enter() { }

        public void Exit() { }

        #endregion
    }
}
