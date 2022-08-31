using Asteroids.UI;


namespace Asteroids.Managers
{
    public interface IUiManager : IManager
    {
        TScreenType ShowScreen<TScreenType>(object parameter = null) where TScreenType : BaseScreen;
    }
}
