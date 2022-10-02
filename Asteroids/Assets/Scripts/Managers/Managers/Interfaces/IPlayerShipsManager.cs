using System;
using Asteroids.Ships;


namespace Asteroids.Managers
{
    public interface IPlayerShipsManager : IManager, IUnloadableManager
    {
        event Action OnPlayerKilled;
        event Action<int> OnPlayerHealthValueChanged;
        
        void SpawnPlayer();
        void RespawnPlayer(float preDelay, float respawnDelay, float iFramesDelay);
        void RespawnPlayer();
        void Reset();
        Ship GetPlayer();
    }
}
