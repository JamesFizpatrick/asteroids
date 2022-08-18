using Asteroids.Managers;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class ControlsScreen : BaseScreen
    {
        #region Fields

        [SerializeField] private Button actionButton;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            ScreenType = ScreenType.Controls;
            actionButton.onClick.AddListener(ActionButton_OnClick);
        }

        #endregion



        #region Protected methods

        protected override void PreClose() =>
            actionButton.onClick.RemoveListener(ActionButton_OnClick);

        #endregion



        #region Event handlers

        private void ActionButton_OnClick()
        {
            UIManager uiManager = ManagersHub.Instance.GetManager<UIManager>();
            uiManager.ShowScreen(ScreenType.Start);
        }

        #endregion
    }
}
