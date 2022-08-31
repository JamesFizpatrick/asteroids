using System;
using UnityEngine;


namespace Asteroids.UI
{
    public class BaseScreen : MonoBehaviour
    {
        #region Fields

        public Action OnClose;
        
        protected object Parameter;

        #endregion



        #region Public methods

        public void Init(object parameter = null)
        {
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
