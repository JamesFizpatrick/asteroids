using System;


namespace Asteroids.Managers
{
    public interface IEnemiesManager : IManager, IUnloadableManager
    {
        event Action OnEnemyKilled;
        
        void StartSpawnCoroutine(float delay);
        bool HasActiveEnemy();
        void Reset();
    }
}