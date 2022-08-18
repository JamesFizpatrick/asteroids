using Asteroids.Managers;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class GameScreen : BaseScreen
    {
        #region Fields

        [SerializeField] private HealthBar healthBar;
        [SerializeField] private TMPro.TextMeshProUGUI levelNumber;
        //[SerializeField] private Button pauseButton;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            ScreenType = ScreenType.Game;
            //pauseButton.onClick.AddListener(PauseButton_OnClick);
        }


        private void Start() => healthBar.Init(ManagersHub.Instance);


        private void OnDestroy()
        {
            OnClose?.Invoke();
            //pauseButton.onClick.RemoveListener(PauseButton_OnClick);
        }

        #endregion



        #region Event handlers

        private void PauseButton_OnClick()
        {
            UIManager uiManager = ManagersHub.Instance.GetManager<UIManager>();
            //uiManager.ShowScreen(ScreenType.Pause);
        }

        #endregion
    }
}
