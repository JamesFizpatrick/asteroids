using System;
using System.Linq;
using Asteroids.Asteroids;
using Asteroids.UI;
using UnityEngine;


namespace Asteroids.Data
{
    [CreateAssetMenu(fileName = "CustomAssets/PlayerPreset")]
    public class PlayerPreset : ScriptableObject
    {
        #region Nested types

        [Serializable]
        private struct Screen
        {
            public ScreenType type;
            public BaseScreen screen;
        }

        #endregion
        
        
        
        #region Fields

        [Header("Gameplay objects")]
        [SerializeField] private int playerLivesQuantity;
        [SerializeField] private GameObject ship;
        [SerializeField] private GameObject playerProjectiles;
        [SerializeField] private GameObject playerAltProjectiles;
        [SerializeField] private GameObject enemyProjectiles;
        [SerializeField] private GameObject enemy;
        [SerializeField] private Asteroid[] asteroids;

        [Header("UI")] 
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private Screen[] screens; 
        
        #endregion


        
        #region Properties

        public int PlayerLivesQuantity => playerLivesQuantity;

        
        public GameObject Ship => ship;
        
        
        public GameObject PlayerProjectiles => playerProjectiles;
        
        
        public GameObject PlayerAltProjectiles => playerAltProjectiles;
        
        
        public GameObject EnemyProjectiles => enemyProjectiles;

        
        public GameObject Enemy => enemy;
        
        
        public Asteroid[] Asteroids => asteroids;

        
        public HealthBar HealthBar => healthBar;

        
        public BaseScreen GetScreen(ScreenType type) => screens.FirstOrDefault(x => x.type == type).screen;

        #endregion
    }
}
