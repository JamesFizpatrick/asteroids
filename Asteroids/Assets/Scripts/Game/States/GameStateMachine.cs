using System;
using System.Collections.Generic;
using Asteroids.Managers;


namespace Asteroids.Game
{
    public class GameStateMachine
    {
        #region Fields

        private readonly Dictionary<Type, IExitableState> states;
        private IExitableState currentState;

        #endregion



        #region Class lifecycle

        public GameStateMachine(IManagersHub managersHub)
        {
            states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootState)] = new BootState(this),
                
                [typeof(MainMenuState)] = new MainMenuState(this,
                    managersHub.GetManager<IUiManager>()),
                
                [typeof(StartGameState)] = new StartGameState(this,
                    managersHub.GetManager<IGameManager>()),
                
                [typeof(GameplayState)] = new GameplayState(this,
                    managersHub.GetManager<IGameManager>(),
                    managersHub.GetManager<IUiManager>(),
                    managersHub.GetManager<IPlayerShipsManager>()),
                
                [typeof(InterWinState)] = new InterWinState(this,
                    managersHub.GetManager<IUiManager>()),
                
                [typeof(InterLoseState)] = new InterLoseState(this,
                    managersHub.GetManager<IUiManager>(),
                    managersHub.GetManager<IGameManager>()),
                
                [typeof(PauseState)] = new PauseState(this,
                    managersHub.GetManager<IGameManager>(),
                    managersHub.GetManager<IUiManager>()),
                
                [typeof(SurvivalWinState)] = new SurvivalWinState(this,
                    managersHub.GetManager<IUiManager>(),
                    managersHub.GetManager<IPlayerProgressManager>())
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
