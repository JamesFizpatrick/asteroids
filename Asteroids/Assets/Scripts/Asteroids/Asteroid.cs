using System;
using Asteroids.Handlers;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.Asteroids
{
    public class Asteroid : MonoBehaviour
    {
        #region Fields

        public Action<Asteroid> Destroyed;
        
        private const float RotationSpeed = 0.7f;

        [SerializeField] private AsteroidType asteroidType;
        [SerializeField] private Image noMineralsImage;
        [SerializeField] private Image mineralsImage;
        [SerializeField] private float moveSpeed;

        private int weaponLayerMask;
        private Vector3 currentMoveDirection;
        
        #endregion



        #region Properties

        public AsteroidType Type => asteroidType;
        
        
        public Vector3 CurrentMoveDirection => currentMoveDirection;
        
        #endregion


        
        #region Unity lifecycle

        private void Awake()
        {
            weaponLayerMask = LayerMasksHandler.PlayerProjectiles;
            SelectMineralsTexture();
        }


        private void FixedUpdate()
        {
            transform.Translate(currentMoveDirection * moveSpeed);

            noMineralsImage.transform.Rotate(Vector3.forward, RotationSpeed);
            mineralsImage.transform.Rotate(Vector3.forward, RotationSpeed);
        }


        private void OnTriggerEnter2D(Collider2D other) => ProcessOnTriggerEnter(other);

        #endregion


        
        #region Public methods

        public void OverrideDirection(Vector3 newDirection) => currentMoveDirection = newDirection;

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
                gameObject.SetActive(false);
                Destroyed?.Invoke(this);
            }
        }
        
        #endregion
    }
}
