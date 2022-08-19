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


        public void Enter()
        {
            UIManager uiManager = ManagersHub.Instance.GetManager<UIManager>();
            uiManager.ShowScreen<StartScreen>(gameStateMachine);
        }


        public void Exit() {}
    }
}
