using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.UI
{
    public class ClassicGameScreen : BaseGameScreen
    {
        #region Fields
        
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private TMPro.TextMeshProUGUI levelNumber;

        private PlayerProgressManager playerProgressManager;
        
        #endregion



        #region Unity lifecycle
        
        private void OnDestroy() => playerProgressManager.OnLevelIndexChanged -= GameplayController_OnLevelIndexChanged;

        #endregion

        

        #region Protected methods

        protected override void Init()
        {
            healthBar.Init(ManagersHub.GetManager<PlayerShipsManager>());
            
            playerProgressManager = (PlayerProgressManager)Parameter;
            
            SetLevelNumber(playerProgressManager.GetLevelIndex() + 1);
            playerProgressManager.OnLevelIndexChanged += GameplayController_OnLevelIndexChanged;
        }

        #endregion

        
        
        #region Private methods

        private void SetLevelNumber(int number) => levelNumber.text = $"LEVEL: {number}";

        #endregion
        
        
        
        #region Event handlers
        
        private void GameplayController_OnLevelIndexChanged(int levelIndex) => SetLevelNumber(levelIndex + 1);

        #endregion
    }
}
