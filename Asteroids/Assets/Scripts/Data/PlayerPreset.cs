using Asteroids.Asteroids;
using UnityEngine;


namespace Asteroids.Data
{
    [CreateAssetMenu(fileName = "CustomAssets/PlayerPreset")]
    public class PlayerPreset : ScriptableObject
    {
        #region Fields

        [SerializeField] private GameObject ship;
        [SerializeField] private GameObject weapon;
        [SerializeField] private Asteroid[] asteroids;

        #endregion


        
        #region Properties

        public GameObject Ship => ship;
        public GameObject Weapon => weapon;
        public Asteroid[] Asteroids => asteroids;

        #endregion
    }
}
