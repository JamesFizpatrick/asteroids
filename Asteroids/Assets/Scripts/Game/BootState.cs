using Asteroids.Managers;


namespace Asteroids.Game
{
    public class BootState : IState
    {
        private GameStateMachine gameStateMachine;

        public BootState(GameStateMachine gameStateMachine)
        {
            this.gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            ManagersHub.Instance.Initialize();
            gameStateMachine.EnterState<MainMenuState>();
        }


        public void Exit()
        {
        }
    }
}
