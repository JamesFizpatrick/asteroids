using System;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.UI
{
    public class BaseScreen : MonoBehaviour
    {
        public Action OnClose;

        protected IManagersHub managersHub;
        protected object parameter;


        public virtual void Init(IManagersHub hub, object parameter = null)
        {
            managersHub = hub;
            this.parameter = parameter;
        }


        public void CloseScreen()
        {
            PreClose();
            OnClose?.Invoke();
            Destroy(gameObject);
        }

        protected virtual void PreClose() {}
    }
}
