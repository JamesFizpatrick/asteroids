using Asteroids.Data;
using Asteroids.Handlers;
using UnityEngine;


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
            GameObject.Instantiate(DataContainer.UiPreset.StarryBackground, GameSceneReferences.MainCanvas.transform);
            gameStateMachine.EnterState<MainMenuState, GameType>(GameType.None);
        }

        public void Exit() { }

        #endregion
    }
}
