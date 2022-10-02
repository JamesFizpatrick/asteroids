using System;
using System.Collections.Generic;
using Asteroids.Asteroids;
using UnityEngine;


namespace Asteroids.Managers
{
    public interface IAsteroidsManager : IManager
    {
        event Action OnFracturesDestroyed;
        event Action OnAllAsteroidsDestroyed;
        event Action<Asteroid> OnAsteroidDestroyed;
        
        void SpawnAsteroids(int quantity, Vector3Int playerPosition, int safeRadius);
        void SpawnNewAsteroidOutOfFOV();
        int GetActiveAsteroidsCount();
        List<SpawnAsteroidData> GetActiveAsteroidsData();
        void Reset();
    }
}
