using Asteroids.Managers;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class SettingsScreen : BaseScreen
    {
        #region Fields

        [SerializeField] private Button muteButton;
        [SerializeField] private Button closeButton;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            ScreenType = ScreenType.Controls;

            muteButton.onClick.AddListener(MuteButton_OnClick);
            closeButton.onClick.AddListener(CloseButton_OnClick);
        }


        private void OnDestroy()
        {
            OnClose?.Invoke();
            muteButton.onClick.RemoveListener(MuteButton_OnClick);
            closeButton.onClick.RemoveListener(CloseButton_OnClick);
        }

        #endregion



        #region Event handlers

        private void CloseButton_OnClick()
        {
            UIManager uiManager = ManagersHub.Instance.GetManager<UIManager>();
            uiManager.ShowScreen(ScreenType.Start);
        }


        private void MuteButton_OnClick()
        {
            // Mute
        }

        #endregion
    }
}

