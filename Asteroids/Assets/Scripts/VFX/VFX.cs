using System;
using System.Collections;
using UnityEngine;


namespace Asteroids.VFX
{
    public class VFX : MonoBehaviour
    {
        #region Fields

        public Action<VFX> Destroyed;
        
        [SerializeField] private float lifetime;
        
        private Coroutine destroyCoroutine;

        #endregion


        
        #region Unity lifecycle

        private void Awake() => destroyCoroutine = StartCoroutine(DestroyWithDelay(lifetime));


        private void OnDestroy()
        {
            if (destroyCoroutine != null)
            {
                StopCoroutine(destroyCoroutine);
            }
            
            Destroyed?.Invoke(this);
        }

        #endregion


        
        #region Private methods

        private IEnumerator DestroyWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }

        #endregion
    }
}
