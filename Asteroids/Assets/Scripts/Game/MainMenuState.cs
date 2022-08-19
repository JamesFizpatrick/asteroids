using Asteroids.Managers;
using Asteroids.UI;


namespace Asteroids.Game
{
    public class MainMenuState : IState
    {
        private GameStateMachine gameStateMachine;


        public MainMenuState(GameStateMachine gameStateMachine)
        {
            this.gameStateMachine = gameStateMachine;
        }


        public void Enter() =>
            ManagersHub.Instance.GetManager<UIManager>().ShowScreen(ScreenType.Start, gameStateMachine);


        public void Exit() {}
    }
}
