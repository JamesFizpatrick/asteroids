using System;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.Asteroids
{
    public class Asteroid : MonoBehaviour
    {
        #region Fields

        public Action<Asteroid> Destroyed;
        
        private const float RotationSpeed = 0.7f;
        private const float MoveSpeed = 1f;

        [SerializeField] private AsteroidType asteroidType;
        [SerializeField] private Image noMineralsImage;
        [SerializeField] private Image mineralsImage;

        private int weaponLayerMask;
        private Vector3 currentDirection;
        
        #endregion



        #region Properties

        public AsteroidType Type => asteroidType;
        
        public Vector3 CurrentDirection => currentDirection;
        
        #endregion


        
        #region Unity lifecycle

        private void Awake()
        {
            weaponLayerMask = LayerMask.NameToLayer("Weapon");
            SelectMineralsTexture();
        }


        private void FixedUpdate()
        {
            transform.Translate(currentDirection * MoveSpeed);

            noMineralsImage.transform.Rotate(Vector3.forward, RotationSpeed);
            mineralsImage.transform.Rotate(Vector3.forward, RotationSpeed);
        }


        private void OnTriggerEnter2D(Collider2D other) => ProcessOnTriggerEnter(other);

        #endregion


        
        #region Public methods

        public void OverrideDirection(Vector3 newDirection)
        {
            currentDirection = newDirection;
        }

        #endregion

        

        #region Private methods

        private void SelectMineralsTexture()
        {
            System.Random random = new System.Random();
            int decision = random.Next(0, 2);
                
            noMineralsImage.gameObject.SetActive(decision == 0);
            mineralsImage.gameObject.SetActive(decision != 0);
        }


        private void ProcessOnTriggerEnter(Collider2D inputCollider)
        {
            if (inputCollider.gameObject.layer == weaponLayerMask)
            {
                Destroyed?.Invoke(this);
                gameObject.SetActive(false);
            }
        }
        
        #endregion
    }
}
