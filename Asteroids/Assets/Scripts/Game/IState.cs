namespace Asteroids.Game
{
    public interface IState : IExitableState
    {
        void Enter();
    }


    public interface IParametricState<TParameter> : IExitableState
    {
        void Enter(TParameter parameter);
    }


    public interface IExitableState
    {
        void Exit();
    }
}