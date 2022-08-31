using System;
using System.Collections.Generic;
using Asteroids.Asteroids;
using UnityEngine;


namespace Asteroids.Managers
{
    public interface IAsteroidsManager : IManager
    {
        Action OnFracturesDestroyed { get; set; }
        Action OnAllAsteroidsDestroyed  { get; set; }
        Action<Asteroid> OnAsteroidDestroyed  { get; set; }
        
        void SpawnAsteroids(int quantity, Vector3Int playerPosition, int safeRadius);
        void SpawnNewAsteroidOutOfFOV();
        int GetActiveAsteroidsCount();
        List<SpawnAsteroidData> GetActiveAsteroidsData();
        void Reset();
    }
}
