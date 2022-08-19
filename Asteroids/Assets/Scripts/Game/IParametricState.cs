namespace Asteroids.Game
{
    public interface IParametricState<TParameter> : IExitableState
    {
        void Enter(TParameter parameter);
    }
}
