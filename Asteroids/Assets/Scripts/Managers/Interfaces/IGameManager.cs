using System;
using Asteroids.Game;


namespace Asteroids.Managers
{
    public interface IGameManager : IManager, IUnloadableManager
    {
        Action OnPlayerWin { get; set; }
        Action OnPlayerLose { get; set; }

        BaseGameplayController GetCurrentGameplayController();
        void SetGameplayType(GameType gameType);
        void StartGame();
        void StopGame();
        void SetPause(bool isActive);
        void Reset();
    }
}
