using UnityEngine;


namespace Asteroids.Managers
{
    public struct SpawnAsteroidData
    {
        public Vector3Int LocalPosition;
        public Vector2Int ColliderSize;

        public SpawnAsteroidData(Vector3 localPosition, Vector2 colliderSize)
        {
            LocalPosition = Vector3Int.FloorToInt(localPosition);
            ColliderSize = Vector2Int.FloorToInt(colliderSize);
        }
    }
}
