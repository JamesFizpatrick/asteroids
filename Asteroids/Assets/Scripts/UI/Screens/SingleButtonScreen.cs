using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class SingleButtonScreen : BaseScreen
    {
        #region Fields

        [SerializeField] private ScreenType screenType;
        [SerializeField] private Button actionButton;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            ScreenType = screenType;
            actionButton.onClick.AddListener(ActionButton_OnClick);
        }


        private void OnDestroy()
        {
            OnClose?.Invoke();
            actionButton.onClick.RemoveListener(ActionButton_OnClick);
        }

        #endregion



        #region Event handlers

        private void ActionButton_OnClick() => Destroy(gameObject);

        #endregion
    }
}

