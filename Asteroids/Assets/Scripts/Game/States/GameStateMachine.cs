using System;
using System.Collections.Generic;
using Asteroids.Managers;


namespace Asteroids.Game
{
    public class GameStateMachine
    {
        #region Fields

        private Dictionary<Type, IExitableState> states;
        private IExitableState currentState;

        #endregion



        #region Class lifecycle

        public GameStateMachine(IManagersHub managersHub)
        {
            states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootState)] = new BootState(this),
                [typeof(MainMenuState)] = new MainMenuState(this),
                [typeof(StartGameState)] = new StartGameState(this),
                [typeof(GameplayState)] = new GameplayState(this)
            };
        }

        #endregion



        #region Public methods

        public void EnterState<TState>() where TState : class, IState
        {
            IState newState = ChangeState<TState>();
            newState.Enter();
        }

      
        public void EnterState<TState, TParameter>(TParameter parameter) where TState : class, IParametricState<TParameter>
        {
            IParametricState<TParameter> newState = ChangeState<TState>();
            newState.Enter(parameter);
        }


        #endregion



        #region Private methods

        private TState GetState<TState>() where TState : class, IExitableState =>
            states[typeof(TState)] as TState;


        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            currentState?.Exit();

            TState newState = GetState<TState>();
            currentState = newState;

            return newState;
        }

        #endregion
    }
}
