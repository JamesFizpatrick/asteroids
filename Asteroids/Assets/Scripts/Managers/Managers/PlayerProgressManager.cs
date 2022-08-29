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

        private ulong currentHighscore = 0;
        
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


        public void SaveCurrentSurvivalHighScore(string name)
        {
            Highscore[] highscores = playerProgress.SurvivalHighscores;
            
            for (int i = 0; i < highscores.Length; i++)
            {
                if (highscores[i].score <= currentHighscore)
                {
                    highscores = CreateShiftedArray(highscores, i);
                    highscores[i] = new Highscore(name, currentHighscore);
                    break;
                }
            }

            playerProgress.SurvivalHighscores = highscores;
            hasPreviousProgress = true;
        }

        
        public void SetCurrentSurvivalHighScore(ulong score) => currentHighscore = score;


        public Highscore[] GetHighscores() => playerProgress.SurvivalHighscores;

        #endregion



        #region Private methods

        private Highscore[] CreateShiftedArray(Highscore[] source, int startIndex)
        {
            Highscore[] result = new Highscore[source.Length];

            for (int i = startIndex; i < source.Length - 1; i++)
            {
                result[i + 1] = source[i];
            }

            return result;
        }
        
        
        private PlayerProgress NewPlayerProgress()
        {
            hasPreviousProgress = false;
            return new PlayerProgress();
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
