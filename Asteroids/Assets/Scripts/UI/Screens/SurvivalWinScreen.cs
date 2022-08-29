using Asteroids.Managers;
using Asteroids.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.Game
{
    public class SurvivalWinScreen : BaseScreen
    {
        #region Fields
        
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button saveButton;

        private PlayerProgressManager progressManager;
        
        #endregion


        
        #region Unity lifecycle

        private void OnEnable() => saveButton.onClick.AddListener(SaveButton_OnClick);


        private void OnDisable() => saveButton.onClick.RemoveListener(SaveButton_OnClick);

        #endregion


        
        #region Protected methods

        protected override void Init()
        {
            progressManager = (PlayerProgressManager)Parameter;
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
