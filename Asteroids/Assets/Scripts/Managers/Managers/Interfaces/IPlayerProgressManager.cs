using Asteroids.Data;


namespace Asteroids.Managers
{
    public interface IPlayerProgressManager : IManager, IUnloadableManager
    {
        void SetLevelIndex(int index);
        int GetLevelIndex();
        void SaveCurrentSurvivalHighScore(string name);
        void SetCurrentSurvivalHighScore(ulong score);
        Highscore[] GetHighscores();
    }
}
