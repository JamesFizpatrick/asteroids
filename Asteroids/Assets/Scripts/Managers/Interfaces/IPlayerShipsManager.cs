using System;
using Asteroids.Game;


namespace Asteroids.Managers
{
    public interface IPlayerShipsManager : IManager, IUnloadableManager
    {
        Action OnPlayerKilled { get; set; }
        Action<int> OnPlayerHealthValueChanged { get; set; }
        
        void SpawnPlayer();
        void RespawnPlayer(float preDelay, float respawnDelay, float iFramesDelay);
        void RespawnPlayer();
        void Reset();
        Ship GetPlayer();
    }
}
