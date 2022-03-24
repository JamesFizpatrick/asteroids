using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;


namespace Asteroids.Handlers
{
    public static class Extensions
    {
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));
            }

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length==j) ? Arr[0] : Arr[j];            
        }
    
    
        public static float GetRandomFloat(this Random random, float min, float max)
        {
            double result = (random.NextDouble() * (max - min) + min);
            return (float)result;
        }
        
        
        public static int GetRandomExclude(this Random random, int min, int max, int excludeMin, int excludeMax)
        {
            int baseElementsCount = Mathf.Abs(max - min);
            int excludedElementsCount = Mathf.Abs(excludeMax - excludeMin);
            int finalElementsCount = baseElementsCount - excludedElementsCount;
            
            IEnumerable<int> range = Enumerable.Range(min, baseElementsCount)
                .Where(i => i < excludeMin || i > excludeMax);
            
            int index = random.Next(0, finalElementsCount);
            return range.ElementAt(index);
        }
    }
}
