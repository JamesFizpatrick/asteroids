using Asteroids.Managers;
using Asteroids.UI;
using UnityEngine;


namespace Asteroids.Game
{
    public class SurvivalGameScreen : BaseGameScreen
    {
        #region Fields
        
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private TMPro.TextMeshProUGUI scoreLabel;

        private SurvivalGameplayController gameplayController;
        
        #endregion
        
        
        
        #region Unity lifecycle
        
        private void OnDestroy() => gameplayController.OnScoreChanged -= GameplayController_OnScoreChanged;
        
        #endregion
        
        
        
        #region Protected methods

        protected override void Init()
        {
            healthBar.Init(ManagersHub.GetManager<IPlayerShipsManager>());
            
            gameplayController = (SurvivalGameplayController)Parameter;
            
            SetScoreLabel(0.ToString());
            gameplayController.OnScoreChanged += GameplayController_OnScoreChanged;
        }
        
        #endregion


        
        #region Private methods
        
        private void SetScoreLabel(string score) => scoreLabel.text = "SCORE: " + score;
        
        #endregion


        
        #region Event handlers

        private void GameplayController_OnScoreChanged(ulong score) => SetScoreLabel(score.ToString());

        #endregion
    }
}
