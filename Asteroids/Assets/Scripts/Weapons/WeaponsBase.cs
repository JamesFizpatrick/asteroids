using UnityEngine;


namespace Asteroids.Game
{
    public class WeaponsBase : MonoBehaviour
    {
        #region Fields

        [SerializeField] private float speed;
        [SerializeField] private float fireCooldown;
        
        #endregion


    
        #region Properties

        public float FireCooldown => fireCooldown;

        #endregion


    
        #region Unity lifecycle
        
        private void FixedUpdate() => transform.Translate(Vector3.up * speed);


        private void OnTriggerEnter2D(Collider2D col)
        {
            ProcessOnTriggerEnter(col);
        }
    
        #endregion


    
        #region Protected metohds

        protected virtual void ProcessOnTriggerEnter(Collider2D col) {}

        #endregion
    }
}
