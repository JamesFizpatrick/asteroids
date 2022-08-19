using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class InterScreen : BaseScreen
    {
        #region Fields

        [SerializeField] private Button actionButton;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            actionButton.onClick.AddListener(ActionButton_OnClick);
        }

        #endregion



        #region Protected methods

        protected override void PreClose() =>
            actionButton.onClick.RemoveListener(ActionButton_OnClick);

        #endregion



        #region Event handlers

        private void ActionButton_OnClick() => Destroy(gameObject);

        #endregion
    }
}

