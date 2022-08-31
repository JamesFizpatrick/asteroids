using System;


namespace Asteroids.Managers
{
    public interface IEnemiesManager : IManager, IUnloadableManager
    {
        Action OnEnemyKilled { get; set; }
        
        void StartSpawnCoroutine(float delay);
        bool HasActiveEnemy();
        void Reset();
    }
}