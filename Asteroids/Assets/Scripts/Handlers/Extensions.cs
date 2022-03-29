using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;


namespace Asteroids.Handlers
{
    public static class Extensions
    {
        public static EnumType Next<EnumType>(this EnumType src) where EnumType : struct
        {
            if (!typeof(EnumType).IsEnum)
            {
                throw new ArgumentException($"Argument {typeof(EnumType).FullName} is not an Enum");
            }

            EnumType[] enumValues = (EnumType[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf(enumValues, src) + 1;
            return (enumValues.Length==j) ? enumValues[0] : enumValues[j];            
        }
    
    
        public static float GetRandomFloat(this Random random, float min, float max)
        {
            double result = random.NextDouble() * (max - min) + min;
            return (float)result;
        }
        
        
        public static int GetRandomExclude(this Random random, int min, int max, int excludeMin, int excludeMax)
        {
            int baseElementsCount = Mathf.Abs(max - min);
            int excludedElementsCount = Mathf.Abs(excludeMax - excludeMin);
            int finalElementsCount = baseElementsCount - excludedElementsCount;
            
            IEnumerable<int> range = Enumerable.Range(min, baseElementsCount)
                .Where(i => i < excludeMin || i > excludeMax);
            
            int index = random.Next(0, finalElementsCount - 1);
            return range.ElementAt(index);
        }
        
        
        public static (Vector3 leftVector, Vector3 rightVector) GetBreakVectors(this Vector3 origin, float angleInDegrees)
        {
            float gradToRadianConst = Mathf.PI / 180f;
            float leftAngle = angleInDegrees * gradToRadianConst;
            float rightAngle = -angleInDegrees * gradToRadianConst;

            return (origin.RotateVectorByAngle(leftAngle), origin.RotateVectorByAngle(rightAngle));
        }


        public static Vector3 RotateVectorByAngle(this Vector3 origin, float angleInRadians)
        {
            float originX = origin.x;
            float originY = origin.y;
        
            float newX = Mathf.Cos(angleInRadians) * originX - Mathf.Sin(angleInRadians) * originY;
            float newY = Mathf.Sin(angleInRadians) * originX + Mathf.Cos(angleInRadians) * originY;

            return new Vector3(newX, newY, 0f);
        }
    }
}
