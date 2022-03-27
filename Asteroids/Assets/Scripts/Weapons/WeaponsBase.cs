using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Game
{
    public class WeaponsBase : MonoBehaviour
    {
        #region Fields

        [SerializeField] private float speed;
        [SerializeField] private float fireCooldown;
        [SerializeField] private SoundType soundType;
        
        #endregion


    
        #region Properties

        public float FireCooldown => fireCooldown;

        
        public SoundType SoundType => soundType;
        
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
