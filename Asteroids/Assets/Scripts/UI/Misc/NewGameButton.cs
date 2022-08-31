using System;
using Asteroids.Game;
using UnityEngine;
using UnityEngine.UI;


namespace Asteroids.UI
{
    public class NewGameButton : MonoBehaviour
    {
        #region Fields

        public Action<GameType> OnStartNewGame;

        [Header("Buttons")]
        [SerializeField] private Button newGameBtn;
        [SerializeField] private Button classicBtn;
        [SerializeField] private Button survivalBtn;

        [Header("Roots")]
        [SerializeField] private GameObject newGameBtnRoot;
        [SerializeField] private GameObject gameModesBtnsRoot;

        #endregion

        

        #region Unity lifecycle

        private void Awake() => SetButtonsActivity(false);


        private void OnEnable()
        {
            newGameBtn.onClick.AddListener(NewGameBtn_OnClick);
            classicBtn.onClick.AddListener(ClassicBtn_OnClick);
            survivalBtn.onClick.AddListener(SurvivalBtn_OnClick);
        }


        private void OnDisable()
        {
            newGameBtn.onClick.RemoveListener(NewGameBtn_OnClick);
            classicBtn.onClick.RemoveListener(ClassicBtn_OnClick);
            survivalBtn.onClick.RemoveListener(SurvivalBtn_OnClick);
        }
        
        #endregion


        
        #region Event handlers
        
        private void SurvivalBtn_OnClick() => OnStartNewGame.Invoke(GameType.Survival);

        
        private void ClassicBtn_OnClick() => OnStartNewGame.Invoke(GameType.Classic);

        
        private void NewGameBtn_OnClick() => SetButtonsActivity(true);

        #endregion


        
        #region Private methods

        private void SetButtonsActivity(bool showGameModes)
        {
            newGameBtnRoot.SetActive(!showGameModes);
            gameModesBtnsRoot.SetActive(showGameModes);
        }

        #endregion
    }
}
