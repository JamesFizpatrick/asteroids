using Asteroids.Managers;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class WinScreen : BaseScreen
    {
        #region Fields

        [SerializeField] private Button startButton;

        #endregion



        #region Unity lifecycle

        private void Awake() => startButton.onClick.AddListener(StartButton_OnClick);

        #endregion



        #region Protected methods

        protected override void PreClose() =>
            startButton.onClick.RemoveListener(StartButton_OnClick);

        #endregion



        #region Event handlers

        private void StartButton_OnClick()
        {
            GameManager gameManager = ManagersHub.Instance.GetManager<GameManager>();
            gameManager.ResetGame();

            CloseScreen();
        }

        #endregion
    }
}
