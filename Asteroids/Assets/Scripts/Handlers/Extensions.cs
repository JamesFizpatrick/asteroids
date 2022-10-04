using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;


namespace Asteroids.Handlers
{
    public static class Extensions
    {
        /// <summary>
        /// Get next enum element
        /// </summary>
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
    
        
        /// <summary>
        /// Get random float value between two values
        /// </summary>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value (will not be included)</param>
        /// <returns></returns>
        public static float GetRandomFloat(this Random random, float min, float max)
        {
            double result = random.NextDouble() * (max - min) + min;
            return (float)result;
        }
        
        
        /// <summary>
        /// Get random int between two values excluding a range using LINQ
        /// </summary>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        /// <param name="excludeMin">Min element of excluded list</param>
        /// <param name="excludeMax">Max element of excluded list</param>
        /// <returns></returns>
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
        
        
        /// <summary>
        /// Get two vectors created from initial one and an angle of spread
        /// </summary>
        /// <param name="angleInDegrees">Spread angle</param>
        /// <returns></returns>
        public static (Vector3 leftVector, Vector3 rightVector) GetBreakVectors(this Vector3 origin, float angleInDegrees)
        {
            float gradToRadianConst = Mathf.PI / 180f;
            float leftAngle = angleInDegrees * gradToRadianConst;
            float rightAngle = -angleInDegrees * gradToRadianConst;

            return (origin.RotateVectorByAngle(leftAngle), origin.RotateVectorByAngle(rightAngle));
        }

        
        /// <summary>
        /// Rotate vector by an euler angle
        /// </summary>
        /// <param name="angleInRadians">Angle in radians</param>
        /// <returns></returns>
        public static Vector3 RotateVectorByAngle(this Vector3 origin, float angleInRadians)
        {
            float originX = origin.x;
            float originY = origin.y;
        
            float newX = Mathf.Cos(angleInRadians) * originX - Mathf.Sin(angleInRadians) * originY;
            float newY = Mathf.Sin(angleInRadians) * originX + Mathf.Cos(angleInRadians) * originY;

            return new Vector3(newX, newY, 0f);
        }


        public static TObjectType ToDeserialized<TObjectType>(this String json) => JsonUtility.FromJson<TObjectType>(json);

        
        public static string ToSerialized<TObjectType>(this TObjectType target) => JsonUtility.ToJson(target);
    }
}
