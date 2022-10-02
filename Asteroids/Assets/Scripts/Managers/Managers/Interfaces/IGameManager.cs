using System;
using Asteroids.Game;


namespace Asteroids.Managers
{
    public interface IGameManager : IManager, IUnloadableManager
    {
        event Action OnPlayerWin;
        event Action OnPlayerLose;

        BaseGameplayController GetCurrentGameplayController();
        void SetGameplayType(GameType gameType);
        void StartGame();
        void StopGame();
        void SetPause(bool isActive);
        void Reset();
    }
}
