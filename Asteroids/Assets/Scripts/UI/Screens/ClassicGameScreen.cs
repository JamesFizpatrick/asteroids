using System;
using Asteroids.Game;
using Asteroids.Managers;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class ClassicGameScreen : BaseScreen
    {
        #region Fields

        public Action OnPauseButtonClick;
        
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private TMPro.TextMeshProUGUI levelNumber;
        [SerializeField] private Button pauseButton;

        private ClassicGameplayController gameplayController;
        
        #endregion



        #region Unity lifecycle
        
        private void OnDestroy()
        {
            gameplayController.OnLevelIndexChanged -= GameplayController_OnLevelIndexChanged;
            pauseButton.onClick.RemoveListener(PauseButton_OnClick);
        }
        
        #endregion

        

        #region Protected methods

        protected override void Init()
        {
            healthBar.Init(Managers.ManagersHub.Instance.GetManager<PlayerShipsManager>());
            
            gameplayController = (ClassicGameplayController)Parameter;
            
            SetLevelNumber(gameplayController.GetCurrentLevelIndex() + 1);
            gameplayController.OnLevelIndexChanged += GameplayController_OnLevelIndexChanged;
            pauseButton.onClick.AddListener(PauseButton_OnClick);
        }

        #endregion

        
        
        #region Private methods

        private void SetLevelNumber(int number) => levelNumber.text = $"LEVEL: {number}";

        #endregion
        
        
        
        #region Event handlers

        private void PauseButton_OnClick() => OnPauseButtonClick?.Invoke();


        private void GameplayController_OnLevelIndexChanged(int levelIndex) => SetLevelNumber(levelIndex + 1);

        #endregion
    }
}
