using System;
using Asteroids.Data;
using Asteroids.Handlers;
using UnityEngine;


namespace Asteroids.Managers
{
    public class PlayerProgressManager : IPlayerProgressManager
    {
        #region Fields

        private const string ProgressKey = "Progress";
        
        private PlayerProgress playerProgress;
        private ulong currentHighscore;
        
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
        
        
        public void Unload() => SaveProgress();
        
        
        public void SetLevelIndex(int index) => playerProgress.LevelIndex = index;


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
        }

        
        public void SetCurrentSurvivalHighScore(ulong score) => currentHighscore = score;


        public Highscore[] GetHighscores() => playerProgress.SurvivalHighscores;

        #endregion



        #region Private methods

        private Highscore[] CreateShiftedArray(Highscore[] source, int startIndex)
        {
            Highscore[] result = source;

            for (int i = source.Length - 2; i >= startIndex; i--)
            {
                result[i + 1] = source[i];
            }

            return result;
        }
        
        
        private PlayerProgress NewPlayerProgress() => new PlayerProgress();


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
