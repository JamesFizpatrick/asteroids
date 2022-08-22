using UnityEngine;
using Asteroids.Managers;
using Asteroids.Game;


namespace Asteroids
{
    public class Loader : MonoBehaviour
    {
        private GameStateMachine stateMachine;


        private void Start()
        {
            stateMachine = new GameStateMachine(ManagersHub.Instance);
            stateMachine.EnterState<BootState>();
        }


        private void Update() => ManagersHub.Instance.Update();

        
        private void OnDestroy() => ManagersHub.Instance.Unload();
    }
}
