using Asteroids.Managers;
using Asteroids.UI;


namespace Asteroids.Game
{
    public class StartGameState : IState
    {
        private GameStateMachine gameStateMachine;


        public StartGameState(GameStateMachine gameStateMachine) =>
            this.gameStateMachine = gameStateMachine;


        public void Enter()
        {
            UIManager uiManager = ManagersHub.Instance.GetManager<UIManager>();
            uiManager.ShowScreen(ScreenType.Game);

            GameManager gameManager = ManagersHub.Instance.GetManager<GameManager>();
            gameManager.StartGame();

            gameStateMachine.EnterState<GameplayState>();
        }


        public void Exit() { }
    }
}
