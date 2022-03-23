using UnityEngine;


namespace Asteroids.Data
{
    [CreateAssetMenu(fileName = "CustomAssets/PlayerPreset")]
    public class PlayerPreset : ScriptableObject
    {
        #region Fields

        [SerializeField] private GameObject ship;
        [SerializeField] private GameObject weapon;

        #endregion


        
        #region Properties

        public GameObject Ship => ship;
        public GameObject Weapon => weapon;

        #endregion
    }
}
