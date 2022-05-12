using UnityEngine;
using Asteroids.Managers;


namespace Asteroids
{
    public class Loader : MonoBehaviour
    {
        private void Start()
        {
            ManagersHub.Instance.Initialize();
            ManagersHub.Instance.GetManager<GameManager>().Start();
        }


        private void Update() => ManagersHub.Instance.Update();

        
        private void OnDestroy() => ManagersHub.Instance.Unload();
    }
}
