using Asteroids.Managers;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class LoseScreen : BaseScreen
    {
        #region Fields

        [SerializeField] private Button resetButton;
        [SerializeField] private Button mainMenuButton;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            ScreenType = ScreenType.Lose;
           
            resetButton.onClick.AddListener(ResetButton_OnClick);
            mainMenuButton.onClick.AddListener(MainMenuButton_OnClick);
        }


        private void OnDestroy()
        {
            OnClose?.Invoke();

            resetButton.onClick.RemoveListener(ResetButton_OnClick);
            mainMenuButton.onClick.RemoveListener(MainMenuButton_OnClick);
        }

        #endregion



        #region Event handlers

        private void ResetButton_OnClick()
        {
            GameManager gameManager = ManagersHub.Instance.GetManager<GameManager>();
            gameManager.ResetGame();

            CloseScreen();
        }
    


        private void MainMenuButton_OnClick()
        {
            UIManager uiManager = ManagersHub.Instance.GetManager<UIManager>();
            uiManager.ShowScreen(ScreenType.Start);

            CloseScreen();
        }

        #endregion
    }
}