using Asteroids.Game;
using UnityEngine;


namespace Asteroids.UI
{
    public class SurvivalGameScreen : BaseGameScreen
    {
        #region Fields

        private const string ScoreText = "SCORE: ";

        [SerializeField] private TMPro.TextMeshProUGUI scoreLabel;

        private SurvivalGameplayController gameplayController;

        #endregion
        
        
        
        #region Unity lifecycle
        
        private void OnDestroy() => gameplayController.OnScoreChanged -= GameplayController_OnScoreChanged;
        
        #endregion
        
        
        
        #region Protected methods

        protected override void Init()
        {
            gameplayController = (SurvivalGameplayController)Parameter;
            
            SetScoreLabel(0.ToString());
            gameplayController.OnScoreChanged += GameplayController_OnScoreChanged;
        }
        
        #endregion


        
        #region Private methods
        
        private void SetScoreLabel(string score) => scoreLabel.text = ScoreText + score;
        
        #endregion


        
        #region Event handlers

        private void GameplayController_OnScoreChanged(ulong score) => SetScoreLabel(score.ToString());

        #endregion
    }
}
