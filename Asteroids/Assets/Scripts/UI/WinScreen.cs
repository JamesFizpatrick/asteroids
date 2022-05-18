using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class WinScreen : BaseScreen
    {
        #region Fields
    
        [SerializeField] private Button resetButton;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            ScreenType = ScreenType.Win;
            resetButton.onClick.AddListener(ResetButton_OnClick);
        }

        
        private void OnDestroy()
        {
            OnClose?.Invoke();
            resetButton.onClick.RemoveListener(ResetButton_OnClick);
        }

        #endregion



        #region Event handlers

        private void ResetButton_OnClick() => Destroy(gameObject);

        #endregion
    }
}
