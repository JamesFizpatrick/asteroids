using System;
using UnityEngine;


namespace Asteroids.Game
{
    public class BoundsController : MonoBehaviour
    {
        #region Fields

        

        #endregion
        public Action<Collider2D> OnCollisionExit;
        
        
        private void OnTriggerExit2D(Collider2D entity) => OnCollisionExit?.Invoke(entity);
    }
}
