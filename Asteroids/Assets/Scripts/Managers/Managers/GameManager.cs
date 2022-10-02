using System;
using Asteroids.Game;
using UnityEngine;


namespace Asteroids.Managers
{
    public class GameManager : IGameManager
    {
        #region Fields

        public event Action OnPlayerWin;
        public event Action OnPlayerLose;
        
        private BaseGameplayController gameplayController;
        private IManagersHub hub;
        
        #endregion
        
        

        #region Public methods

        public void Initialize(IManagersHub hub) => this.hub = hub;


        public void Unload()
        {
            if (gameplayController != null)
            {
                gameplayController.OnPlayerLose -= GameplayController_OnPlayerLose;
                gameplayController.OnPlayerWin -= GameplayController_OnPlayerWin;
            }
        }


        public BaseGameplayController GetCurrentGameplayController() => gameplayController;
        
        
        public void SetGameplayType(GameType gameType)
        {
            gameplayController = GameplayControllerFactory.CreateGameplayController(gameType, hub);

            gameplayController.OnPlayerLose += GameplayController_OnPlayerLose;
            gameplayController.OnPlayerWin += GameplayController_OnPlayerWin;
        }
        

        public void StartGame() => gameplayController.StartGame();


        public void StopGame() => gameplayController.StopGame();


        public void SetPause(bool isActive) => Time.timeScale = isActive ? 0f : 1f;


        public void Reset() => gameplayController.Reset();
        
        #endregion


        
        #region Event handlers

        private void GameplayController_OnPlayerWin() => OnPlayerWin?.Invoke();


        private void GameplayController_OnPlayerLose() => OnPlayerLose?.Invoke();

        #endregion
    }
}
