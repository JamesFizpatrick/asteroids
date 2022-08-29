using Asteroids.Data;
using Asteroids.Managers;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class HighscoreScreen : BaseScreen
    {
        #region Fields
        
        [SerializeField] private HighscoreElement highscoreElementPrefab;
        [SerializeField] private Transform highscoresRoot;
        [SerializeField] private Button mainMenuButton;
        
        private PlayerProgressManager progressManager;
        
        #endregion


        
        #region Unity lifecycle

        private void OnEnable() => mainMenuButton.onClick.AddListener(MainMenuButton_OnClick);

        
        private void OnDisable() => mainMenuButton.onClick.RemoveListener(MainMenuButton_OnClick);

        #endregion



        #region Protected methods

        protected override void Init()
        {
            progressManager = Managers.ManagersHub.Instance.GetManager<PlayerProgressManager>();
            InitElements();
        }

        #endregion



        #region Private methods

        private void InitElements()
        {
            Highscore[] highscores = progressManager.GetHighscores();

            if (highscores == null)
            {
                return;
            }
            
            foreach (Highscore highscore in highscores)
            {
                HighscoreElement element = Instantiate(highscoreElementPrefab, highscoresRoot);
                element.Init(highscore);
            }
        }

        #endregion



        #region Event handlers

        private void MainMenuButton_OnClick() => CloseScreen();

        #endregion
    }
}
