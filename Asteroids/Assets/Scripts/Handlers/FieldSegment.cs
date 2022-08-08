using UnityEngine;


namespace Asteroids.Handlers
{
    public class FieldSegment
    {
        #region Fields

        public bool Blocked { get; private set; }
            
        private Vector2Int minCoordinates;
        private Vector2Int maxCoordinates;

        private System.Random random;

        #endregion


        
        #region Class lifecycle

        public FieldSegment(Vector2Int min, Vector2Int max, bool blocked)
        {
            Blocked = blocked;
            minCoordinates = min;
            maxCoordinates = max;

            random = new System.Random();
        }
        
        #endregion



        #region Public methods

        public void Block() => Blocked = true;


        public void Unblock() => Blocked = false;
        
        
        public bool Contains(Vector2Int inputMinRange, Vector2Int inputMaxRange)
        {
            return minCoordinates.x >= inputMinRange.x &&
                maxCoordinates.x < inputMaxRange.x &&
                minCoordinates.y >= inputMaxRange.y &&
                maxCoordinates.y < inputMaxRange.y;
        }


        public Vector2Int GetRandomCoordinate() =>
            new Vector2Int(random.Next(minCoordinates.x, maxCoordinates.x),
            random.Next(minCoordinates.y, maxCoordinates.y));
        
        #endregion
    }
}
