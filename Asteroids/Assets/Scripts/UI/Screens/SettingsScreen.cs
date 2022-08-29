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

        private void OnEnable()
        {
            muteButton.onClick.AddListener(MuteButton_OnClick);
            closeButton.onClick.AddListener(CloseButton_OnClick);
        }


        private void OnDisable()
        {
            muteButton.onClick.RemoveListener(MuteButton_OnClick);
            closeButton.onClick.RemoveListener(CloseButton_OnClick);
        }

        #endregion



        #region Event handlers

        private void CloseButton_OnClick() => CloseScreen();


        private void MuteButton_OnClick()
        {
            // Mute
        }

        #endregion
    }
}
