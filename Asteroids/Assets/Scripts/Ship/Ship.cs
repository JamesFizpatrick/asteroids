using System;
using Asteroids.Handlers;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Game
{
    [RequireComponent(typeof(ShipVisualAppearanceController))]
    public class Ship : MonoBehaviour
    {
        #region Fields

        public Action<Vector3> OnPositionChanged;
        public Action OnPlayerDamaged;

        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float maxInertia = 1;
        [SerializeField] private float rotationSpeed = 3f;
        
        private ShipVisualAppearanceController shipVisualAppearanceController;

        private ShipMovementController shipMovementController;
        private ShipWeaponController shipWeaponController;

        private int enemyProjectilesLayer;
        private int asteroidsLayer;
        private int enemyLayer;

        private Collider2D collider;
        
        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            enemyProjectilesLayer = LayerMasksHandler.EnemyProjectiles;
            asteroidsLayer = LayerMasksHandler.Asteroid;
            enemyLayer = LayerMasksHandler.Enemy;

            InitControllers();
            shipMovementController.OnPositionChanged += ShipMovementController_OnPositionChanged;

            collider = GetComponent<Collider2D>();
        }


        private void FixedUpdate() => shipMovementController.Update();


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
            shipMovementController.Dispose();
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
            shipVisualAppearanceController = GetComponent<ShipVisualAppearanceController>();

            shipMovementController = new ShipMovementController(
                ManagersHub.Instance.GetManager<InputManager>(),
                gameObject,
                moveSpeed,
                maxInertia,
                rotationSpeed);

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
