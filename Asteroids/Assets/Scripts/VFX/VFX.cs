using System;
using System.Collections;
using Asteroids.Handlers;
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

        private void Awake() =>
            destroyCoroutine = CoroutinesHandler.Instance.StartCoroutine(DestroyWithDelay(lifetime));


        private void OnDestroy()
        {
            if (destroyCoroutine != null)
            {
                CoroutinesHandler.Instance.StopCoroutine(destroyCoroutine);
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
