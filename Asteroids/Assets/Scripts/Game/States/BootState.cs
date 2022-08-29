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
            gameStateMachine.EnterState<MainMenuState, GameType>(GameType.None);
        }

        public void Exit() { }

        #endregion
    }
}
