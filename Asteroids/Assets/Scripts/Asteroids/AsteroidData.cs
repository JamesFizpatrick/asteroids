using UnityEngine;


namespace Asteroids.Asteroids
{
    public struct AsteroidData
    {
        public Vector3 Position;
        public Vector3 Direction;
            
        public AsteroidData(Vector3 position, Vector3 direction)
        {
            Position = position;
            Direction = direction;
        }
    }
}
