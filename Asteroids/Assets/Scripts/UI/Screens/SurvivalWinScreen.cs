using Asteroids.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class SurvivalWinScreen : BaseScreen
    {
        #region Fields
        
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button saveButton;

        private IPlayerProgressManager progressManager;
        
        #endregion


        
        #region Unity lifecycle

        private void OnEnable() => saveButton.onClick.AddListener(SaveButton_OnClick);


        private void OnDisable() => saveButton.onClick.RemoveListener(SaveButton_OnClick);

        #endregion


        
        #region Protected methods

        protected override void Init()
        {
            progressManager = (IPlayerProgressManager)Parameter;
        }

        #endregion
        


        #region Event handlers

        private void SaveButton_OnClick()
        {
            progressManager.SaveCurrentSurvivalHighScore(inputField.text);
            CloseScreen();
        }

        #endregion
    }
}
