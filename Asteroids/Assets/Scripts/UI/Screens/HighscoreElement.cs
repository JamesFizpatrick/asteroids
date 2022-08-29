using Asteroids.Data;
using TMPro;
using UnityEngine;


namespace Asteroids.UI
{
    public class HighscoreElement : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TMP_Text name;
        [SerializeField] private TMP_Text score;

        #endregion


        
        #region Public methods

        public void Init(Highscore highscore)
        {
            name.SetText(highscore.name);
            score.SetText(highscore.score.ToString());
        }

        #endregion
    }
}
