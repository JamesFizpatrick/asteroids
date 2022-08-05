namespace Asteroids.Managers
{
    public interface IManagersHub
    {
        TManagerType GetManager<TManagerType>();
    }
}
