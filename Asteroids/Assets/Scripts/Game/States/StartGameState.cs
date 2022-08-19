using Asteroids.Managers;
using Asteroids.UI;


namespace Asteroids.Game
{
    public class StartGameState : IState
    {
        #region Fields

        private GameStateMachine gameStateMachine;

        #endregion



        #region Class lifecycle

        public StartGameState(GameStateMachine gameStateMachine) =>
            this.gameStateMachine = gameStateMachine;

        #endregion



        #region Public methods

        public void Enter()
        {
            UIManager uiManager = ManagersHub.Instance.GetManager<UIManager>();
            uiManager.ShowScreen<GameScreen>();

            GameManager gameManager = ManagersHub.Instance.GetManager<GameManager>();
            gameManager.StartGame();

            gameStateMachine.EnterState<GameplayState>();
        }


        public void Exit() { }

        #endregion
    }
}
