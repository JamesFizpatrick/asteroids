using System;
using UnityEngine;


namespace Asteroids.UI
{
    public class BaseScreen : MonoBehaviour
    {
        public Action OnClose;


        public ScreenType ScreenType { get; protected set; }


        public void CloseScreen()
        {
            PreClose();
            OnClose?.Invoke();
            Destroy(gameObject);
        }

        protected virtual void PreClose() {}
    }
}
