using System;
using Asteroids.Data;
using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.Managers
{
    public class PlayerProgressManager : IManager, IUnloadableManager
    {
        #region Fields

        private const string ProgressKey = "Progress";
        private PlayerProgress playerProgress;
        private bool hasPreviousProgress = true;
        
        #endregion

        

        #region Public methods

        public void Initialize(IManagersHub hub)
        {
            playerProgress = LoadProgress();

            if (playerProgress == null)
            {
                playerProgress = NewPlayerProgress();
            }
        }


        public bool HasPreviousProgress() => hasPreviousProgress;

        
        public void Unload() => SaveProgress();
        
        
        public void ResetProgress() => playerProgress = NewPlayerProgress();


        public void SetLevelIndex(int index)
        {
            playerProgress.LevelIndex = index;
            hasPreviousProgress = true;
        }


        public int GetLevelIndex() => playerProgress.LevelIndex;


        public void SetSurvivalHighScore(ulong score)
        {
            playerProgress.SurvivalHighscore = score;
            hasPreviousProgress = true;
        }
        
        #endregion



        #region Private methods

        private PlayerProgress NewPlayerProgress()
        {
            PlayerProgress progress = new PlayerProgress
            {
                LevelIndex = -1,
                SurvivalHighscore = 0
            };

            hasPreviousProgress = false;

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
