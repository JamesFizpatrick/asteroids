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

        private void OnEnable() => actionButton.onClick.AddListener(ActionButton_OnClick);


        private void OnDisable() => actionButton.onClick.RemoveListener(ActionButton_OnClick);

        #endregion



        #region Event handlers

        private void ActionButton_OnClick()
        {
            UIManager uiManager = Managers.ManagersHub.Instance.GetManager<UIManager>();
            uiManager.ShowScreen<MenuScreen>();
        }

        #endregion
    }
}
