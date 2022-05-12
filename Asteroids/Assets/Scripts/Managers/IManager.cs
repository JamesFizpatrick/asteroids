namespace Asteroids.Managers
{
    public interface IManager
    {
        void Initialize(ManagersHub hub);


        void Update();
        
        
        void Unload();
    }
}
