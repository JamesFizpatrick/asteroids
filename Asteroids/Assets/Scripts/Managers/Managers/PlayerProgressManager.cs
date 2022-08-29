using System;
using Asteroids.Data;
using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.Managers
{
    public class PlayerProgressManager : IManager, IUnloadableManager
    {
        #region Fields

        public Action<int> OnLevelIndexChanged;

        private const string ProgressKey = "Progress";

        private IManagersHub hub;
        private PlayerProgress playerProgress;

        private bool hasPreviousProgress = true;
        
        #endregion


        #region Public methods

        public void Initialize(IManagersHub hub)
        {
            this.hub = hub;
            playerProgress = LoadProgress();

            if (playerProgress == null)
            {
                playerProgress = NewPlayerProgress();
                hasPreviousProgress = false;
            }
        }


        public bool HasPreviousProgress() => hasPreviousProgress;

        
        public void Unload() => SaveProgress();


        public void SetLevelIndex(int index)
        {
            playerProgress.LevelIndex = index;
            OnLevelIndexChanged?.Invoke(index);
        }

        
        public int IncreaseLevelIndex(int by)
        {
            playerProgress.LevelIndex += by;
            OnLevelIndexChanged?.Invoke(playerProgress.LevelIndex);
            return playerProgress.LevelIndex;
        }

        
        public int GetLevelIndex() => playerProgress.LevelIndex;

        
        public void ResetProgress() => playerProgress = NewPlayerProgress();

        
        public void SetSurvivalHighScore(ulong score) => playerProgress.SurvivalHighscore = score;

        #endregion



        #region Private methods

        private PlayerProgress NewPlayerProgress()
        {
            PlayerProgress progress = new PlayerProgress
            {
                LevelIndex = -1
            };

            return progress;
        }
        
        
        private PlayerProgress LoadProgress()
        {
            string json = PlayerPrefs.GetString(ProgressKey);
            return String.IsNullOrEmpty(json) ? null : json.ToDeserialized<PlayerProgress>();
        }

        
        private void SaveProgress()
        {
            string json = playerProgress.ToSerialized();
            PlayerPrefs.SetString(ProgressKey, json);
        }

        #endregion
    }
}
