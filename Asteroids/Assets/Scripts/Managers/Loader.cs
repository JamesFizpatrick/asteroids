using UnityEngine;
using Asteroids.Managers;


namespace Asteroids
{
    public class Loader : MonoBehaviour
    {
        private void Start() => ManagersHub.Initialize();


        private void OnDestroy() => ManagersHub.Deinitialize();
    }
}
