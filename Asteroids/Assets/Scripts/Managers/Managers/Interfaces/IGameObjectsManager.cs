using UnityEngine;


namespace Asteroids.Managers
{
    public interface IGameObjectsManager : IManager
    {
        GameObject CreateBullet(GameObject bulletPrefab);
        GameObject CreatePlayerShip();
        GameObject CreateAsteroid(GameObject asteroidPrefab);
        GameObject CreateEnemy();
        GameObject CreateVFX(GameObject vfxPrefab);
    }
}
