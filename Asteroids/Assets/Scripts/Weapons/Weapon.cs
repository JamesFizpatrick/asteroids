using System.Collections.Generic;
using UnityEngine;


namespace Asteroids.Game
{
    public struct Weapon
    {
        public GameObject ShotPrefab;
        public List<ShotBase> Pool;
        
        public Weapon(GameObject prefab)
        {
            ShotPrefab = prefab;
            Pool = new List<ShotBase>();
        }
    }
}
