using Asteroids.Asteroids;
using UnityEngine;


namespace Asteroids.Data
{
    [CreateAssetMenu(fileName = "CustomAssets/GamePreset")]
    public class GamePreset : ScriptableObject
    {
        #region Fields

        [Header("Player settings")]
        [SerializeField] private int playerLivesQuantity;

        [Header("Gameplay objects")]       
        [SerializeField] private GameObject ship;
        [SerializeField] private GameObject playerProjectiles;
        [SerializeField] private GameObject playerAltProjectiles;
        [SerializeField] private GameObject enemyProjectiles;
        [SerializeField] private GameObject enemy;
        [SerializeField] private Asteroid[] asteroids;
        
        #endregion


        
        #region Properties

        public int PlayerLivesQuantity => playerLivesQuantity;

        
        public GameObject Ship => ship;
        
        
        public GameObject PlayerProjectiles => playerProjectiles;
        
        
        public GameObject PlayerAltProjectiles => playerAltProjectiles;
        
        
        public GameObject EnemyProjectiles => enemyProjectiles;

        
        public GameObject Enemy => enemy;
        
        
        public Asteroid[] Asteroids => asteroids;
        
        #endregion
    }
}
