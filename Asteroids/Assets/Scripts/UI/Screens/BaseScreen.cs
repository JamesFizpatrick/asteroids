using System;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.UI
{
    public class BaseScreen : MonoBehaviour
    {
        #region Fields

        public Action OnClose;

        protected IManagersHub ManagersHub;
        protected object Parameter;

        #endregion



        #region Public methods

        public void Init(IManagersHub hub, object parameter = null)
        {
            ManagersHub = hub;
            Parameter = parameter;
            
            Init();
        }


        public void CloseScreen()
        {
            OnClose?.Invoke();
            Destroy(gameObject);
        }

        #endregion

        

        #region Protected methods

        protected virtual void Init() { }

        #endregion
    }
}
