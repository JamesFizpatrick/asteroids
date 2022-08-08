using System;
using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.Game
{
    [RequireComponent(typeof(ShipMovementController))]
    [RequireComponent(typeof(ShipVisualAppearanceController))]

    public class Player : MonoBehaviour
    {
        #region Fields

        public Action<Vector3> OnPositionChanged;
        public Action Killed;
        
        private ShipMovementController shipMovementController;
        private ShipVisualAppearanceController shipVisualAppearanceController;

        private int enemyProjectilesLayer;
        private int asteroidsLayer;
        private int enemyLayer;

        private Collider2D collider;
        
        #endregion



        #region Unity lifecycle

        public void Awake()
        {
            enemyProjectilesLayer = LayerMasksHandler.EnemyProjectiles;
            asteroidsLayer = LayerMasksHandler.Asteroid;
            enemyLayer = LayerMasksHandler.Enemy;

            InitControllers();
            shipMovementController.OnPositionChanged += ShipMovementController_OnPositionChanged;

            collider = GetComponent<Collider2D>();
        }
        
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            int layer = col.gameObject.layer;
            if (layer == enemyProjectilesLayer || layer == asteroidsLayer || layer == enemyLayer)
            {
                Killed?.Invoke();
            }
        }

        
        public void OnDestroy() => shipMovementController.OnPositionChanged -= ShipMovementController_OnPositionChanged;

        #endregion



        #region Public methods

        public void EnableIFrames(bool canMove)
        {
            shipVisualAppearanceController.StartToBlink();
            collider.enabled = false;

            if (canMove)
            {
                shipMovementController.Move();
            }
            else
            {
                shipMovementController.Stop();
                shipMovementController.Reset();
            }
        }


        public void DisableIFrames()
        {
            shipVisualAppearanceController.StopToBlink();
            collider.enabled = true;
        }

        #endregion

        

        #region Private methods

        private void InitControllers()
        {
            shipMovementController = GetComponent<ShipMovementController>();
            shipVisualAppearanceController = GetComponent<ShipVisualAppearanceController>();
        }
        
        #endregion


        
        #region Event handlers

        private void ShipMovementController_OnPositionChanged(Vector3 position) => OnPositionChanged?.Invoke(position);

        #endregion
    }
}
