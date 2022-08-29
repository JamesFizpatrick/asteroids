using Asteroids.Managers;
using Asteroids.UI;
using UnityEngine;


namespace Asteroids.Game
{
    public class SurvivalGameScreen : BaseGameScreen
    {
        #region Fields
        
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private TMPro.TextMeshProUGUI scoreLabel;

        private PlayerProgressManager playerProgressManager;
        
        #endregion
    }
}