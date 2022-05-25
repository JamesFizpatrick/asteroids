using UnityEngine;


namespace Asteroids.Handlers
{
    public class GameSceneReferences : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Camera mainCamera;
        [SerializeField] private Canvas mainCanvas;

        #endregion


        
        #region Properties

        /// <summary>
        /// Get main scene camera
        /// </summary>
        public static Camera MainCamera { get; private set; }
        
        
        /// <summary>
        /// Get main scene canvas
        /// </summary>
        public static Canvas MainCanvas { get; private set; }

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            MainCamera = mainCamera;
            MainCanvas = mainCanvas;
        }

        #endregion
    }
}
