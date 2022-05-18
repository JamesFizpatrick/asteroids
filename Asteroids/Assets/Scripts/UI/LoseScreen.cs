using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class LoseScreen : BaseScreen
    {
        #region Fields
    
        [SerializeField] private Button resetButton;

        #endregion



        #region Unity lifecycle

        private void Awake()
        {
            ScreenType = ScreenType.Win;
            resetButton.onClick.AddListener(ResetButton_OnClick);

            Time.timeScale = 0.0f;
        }

        
        private void OnDestroy()
        {
            OnClose?.Invoke();
            Time.timeScale = 1.0f;
            resetButton.onClick.RemoveListener(ResetButton_OnClick);
        }

        #endregion



        #region Event handlers

        private void ResetButton_OnClick() => Destroy(gameObject);

        #endregion
    }
}
