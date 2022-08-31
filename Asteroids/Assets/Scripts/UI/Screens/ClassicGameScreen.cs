using Asteroids.Game;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.UI
{
    public class ClassicGameScreen : BaseGameScreen
    {
        #region Fields
        
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private TMPro.TextMeshProUGUI levelNumber;

        private ClassicGameplayController gameplayController;
        
        #endregion



        #region Unity lifecycle
        
        private void OnDestroy() => gameplayController.OnLevelIndexChanged -= GameplayController_OnLevelIndexChanged;

        #endregion

        

        #region Protected methods

        protected override void Init()
        {
            healthBar.Init(ManagersHub.GetManager<IPlayerShipsManager>());
            
            gameplayController = (ClassicGameplayController)Parameter;
            gameplayController.OnLevelIndexChanged += GameplayController_OnLevelIndexChanged;
            
            SetLevelNumber(gameplayController.GetCurrentLevelIndex() + 1);
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
