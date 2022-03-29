using UnityEngine;


namespace Asteroids.Managers
{
    public interface IManager
    {
        void Initialize(GameObject root);
        
        void Unload();
    }
}
