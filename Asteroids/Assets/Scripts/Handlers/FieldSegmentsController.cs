using System.Collections.Generic;
using System.Linq;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Handlers
{
    public class FieldSegmentsController
    {
        #region Fields

        private System.Random random;
        private List<FieldSegment> fieldSegments = new List<FieldSegment>();

        #endregion



        #region Class lifecycle

        public FieldSegmentsController()
        {
            random = new System.Random();

            InitFieldSegments();
        }

        #endregion



        #region Public methods

        public void Reset()
        {
            foreach (FieldSegment segment in fieldSegments)
            {
                segment.Unblock();
            }
        }


        public void BlockSegments(List<SpawnAsteroidData> asteroidsData)
        {
            foreach (SpawnAsteroidData data in asteroidsData)
            {
                int minX = data.LocalPosition.x - data.ColliderSize.x / 2;
                int minY = data.LocalPosition.y - data.ColliderSize.y / 2;
                int maxX = data.LocalPosition.x + data.ColliderSize.x / 2;
                int maxY = data.LocalPosition.y + data.ColliderSize.y / 2;

                Vector2Int minRange = new Vector2Int(minX, minY);
                Vector2Int maxRange = new Vector2Int(maxX, maxY);

                foreach (FieldSegment fieldSegment in fieldSegments)
                {
                    if (fieldSegment.Contains(minRange, maxRange))
                    {
                        fieldSegment.Block();
                    }
                }
            }
        }


        public FieldSegment GetRandomOpenSegment()
        {
            List<FieldSegment> openSegments = fieldSegments.Where(x => !x.Blocked).ToList();
            int index = random.Next(0, openSegments.Count);
            return openSegments[index];
        }

        #endregion



        #region Private methods

        private void InitFieldSegments()
        {
            int minX = -Screen.width / 2 + PlayerConstants.RespawnDistanceFromBorders;
            int minY = -Screen.height / 2 + PlayerConstants.RespawnDistanceFromBorders;
            int maxX = Screen.width / 2 - PlayerConstants.RespawnDistanceFromBorders;
            int maxY = Screen.height / 2 - PlayerConstants.RespawnDistanceFromBorders;

            int xStep = Mathf.Abs(maxX - minX) / PlayerConstants.RespawnFieldSegmentsGridModule;
            int yStep = Mathf.Abs(maxY - minY) / PlayerConstants.RespawnFieldSegmentsGridModule;

            int x = minX;
            int y = minY;

            while (x <= maxX)
            {
                while (y <= maxY)
                {
                    Vector2Int xRange = new Vector2Int(x, x + xStep);
                    Vector2Int yRange = new Vector2Int(y, y + yStep); ;

                    FieldSegment segment = new FieldSegment(xRange, yRange, false);

                    fieldSegments.Add(segment);

                    y += yStep;
                }

                x += xStep;
            }
        }
       
        #endregion
    }
}
