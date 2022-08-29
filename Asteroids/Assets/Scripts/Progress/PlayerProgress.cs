using System;
using Asteroids.Handlers;


namespace Asteroids.Data
{
    [Serializable]
    public struct Highscore
    {
        public string name;
        public ulong score;

        public Highscore(string name, ulong score)
        {
            this.name = name;
            this.score = score;
        }
    }
    
    
    [Serializable]
    public class PlayerProgress
    {
        private const string NamePlaceholder = "---";
        private const ulong ScorePlaceholder = 0;
        
        public int LevelIndex;
        public Highscore[] SurvivalHighscores;

        
        public PlayerProgress()
        {
            LevelIndex = 0;
            SurvivalHighscores = new Highscore[PlayerConstants.MaxHigscoreRecords];

            for (int i = 0; i < PlayerConstants.MaxHigscoreRecords; i++)
            {
                SurvivalHighscores[i] = new Highscore(NamePlaceholder, ScorePlaceholder);
            }
        }
    }
}
