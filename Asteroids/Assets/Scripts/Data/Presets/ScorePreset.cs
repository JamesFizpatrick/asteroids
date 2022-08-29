using System;
using System.Linq;
using Asteroids.Asteroids;
using UnityEngine;


namespace Asteroids.Data
{
    [CreateAssetMenu(fileName = "CustomAssets/ScorePreset")]
    public class ScorePreset : ScriptableObject
    {
        #region Nested types

        [Serializable]
        public struct Asteroid
        {
            public AsteroidType Type;
            public int Points;
        }

        #endregion


        
        #region Fields

        [Header("Asteroids")]
        [SerializeField] private Asteroid[] asteroids;
        
        [Header("Enemies")]
        [SerializeField] private int enemyPoints;

        #endregion



        #region Public methods

        public int GetEnemyPoints() => enemyPoints;
        
        
        public int GetAsteroidScore(AsteroidType type) => asteroids.FirstOrDefault(x => x.Type == type).Points;

        #endregion
    }
}
