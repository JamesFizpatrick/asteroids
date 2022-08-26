using System;
using Asteroids.Game;
using UnityEngine;


namespace Asteroids.Managers
{
    public class GameManager : IManager, IUnloadableManager
    {
        #region Fields

        public Action OnPlayerWin;
        public Action OnPlayerLose;
        
        private BaseGameplayController gameplayController;
        private IManagersHub hub;
        
        #endregion
        
        

        #region Public methods

        public void Initialize(IManagersHub hub) => this.hub = hub;


        public void Unload()
        {
            gameplayController.OnPlayerLose -= GameplayController_OnPlayerLose;
            gameplayController.OnPlayerWin -= GameplayController_OnPlayerWin;
        }
        
        
        public BaseGameplayController CurrentGameplayController() => gameplayController;
        
        #endregion



        #region Public methods

        public void SetGameplayType(GameType gameType)
        {
            gameplayController = GameplayControllerFactory.CreateGameplayController(gameType, hub);

            gameplayController.OnPlayerLose += GameplayController_OnPlayerLose;
            gameplayController.OnPlayerWin += GameplayController_OnPlayerWin;
        }
        

        public void StartGame() => gameplayController.StartGame();


        public void StopGame() => gameplayController.StopGame();


        public void Reset() => gameplayController.Reset();
        
        #endregion


        
        #region Event handlers

        private void GameplayController_OnPlayerWin() => OnPlayerWin?.Invoke();


        private void GameplayController_OnPlayerLose() => OnPlayerLose?.Invoke();

        #endregion
    }
}
