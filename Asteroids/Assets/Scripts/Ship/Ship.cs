using System;
using Asteroids.Handlers;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Game
{
    [RequireComponent(typeof(ShipMovementController))]
    [RequireComponent(typeof(ShipVisualAppearanceController))]
    public class Ship : MonoBehaviour
    {
        #region Fields

        public Action<Vector3> OnPositionChanged;
        public Action OnPlayerDamaged;
        
        private ShipMovementController shipMovementController;
        private ShipVisualAppearanceController shipVisualAppearanceController;
        private ShipWeaponController shipWeaponController;

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
            if (layer == enemyProjectilesLayer ||
                layer == asteroidsLayer ||
                layer == enemyLayer)
            {
                OnPlayerDamaged?.Invoke();
            }
        }

        
        public void OnDestroy()
        {
            shipMovementController.OnPositionChanged -= ShipMovementController_OnPositionChanged;
            shipWeaponController.Dispose();
        }

        #endregion



        #region Public methods

        public void SetWeaponActivity(bool isActive) => shipWeaponController.SetWeaponActivity(isActive);


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
            shipMovementController.Init(ManagersHub.Instance.GetManager<InputManager>());
            
            shipVisualAppearanceController = GetComponent<ShipVisualAppearanceController>();

            shipWeaponController = new ShipWeaponController(
                ManagersHub.Instance.GetManager<SoundManager>(),
                ManagersHub.Instance.GetManager<GameObjectsManager>(),
                gameObject,
                ManagersHub.Instance.GetManager<InputManager>());
        }
        
        #endregion


        
        #region Event handlers

        private void ShipMovementController_OnPositionChanged(Vector3 position) => OnPositionChanged?.Invoke(position);

        #endregion
    }
}
