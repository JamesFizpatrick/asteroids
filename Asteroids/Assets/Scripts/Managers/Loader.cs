using UnityEngine;
using Asteroids.Managers;


namespace Asteroids
{
    public class Loader : MonoBehaviour
    {
        private void Awake() => ManagersHub.Initialize();


        private void OnDestroy() => ManagersHub.Deinitialize();
    }
}
