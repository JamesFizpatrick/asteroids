using System;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.UI
{
    public class BaseScreen : MonoBehaviour
    {
        #region Fields

        public Action OnClose;

        protected IManagersHub managersHub;
        protected object parameter;

        #endregion



        #region Public methods

        public virtual void Init(IManagersHub hub, object parameter = null)
        {
            managersHub = hub;
            this.parameter = parameter;
        }


        public void CloseScreen()
        {
            OnClose?.Invoke();
            Destroy(gameObject);
        }

        #endregion
    }
}
