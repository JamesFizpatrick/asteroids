using System.Collections.Generic;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.UI
{
    public class HealthBar : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject healthBarElementPrefab;
        [SerializeField] private Transform segmentsRoot;
        
        private List<GameObject> pool = new List<GameObject>();

        private GameManager gameManager;

        #endregion


        
        #region Unity lifecycle
        
        public void OnDestroy()
        {
            gameManager.OnReset -= GameManager_OnReset;
            gameManager.OnPlayerHealthValueChanged -= GameManager_OnPlayerHealthValueChanged;
        }

        #endregion



        #region Public methods

        public void Init(ManagersHub hub)
        {
            gameManager = hub.GetManager<GameManager>();
            gameManager.OnReset += GameManager_OnReset;
            gameManager.OnPlayerHealthValueChanged += GameManager_OnPlayerHealthValueChanged;
            
            SetHealth(DataContainer.PlayerPreset.PlayerLivesQuantity);
        }

        #endregion


        
        #region Private methods

        private void SetHealth(int healthValue)
        {
            if (pool.Count >= healthValue)
            {
                foreach (GameObject segment in pool)
                {
                    segment.SetActive(false);
                }
                
                for (int i = 0; i < healthValue; i++)
                {
                    pool[i].SetActive(true);
                }
            }
            else
            {
                foreach (var segment in pool)
                {
                    segment.SetActive(true);
                }
                
                int countToReach = healthValue - pool.Count;

                for (int i = 0; i < countToReach; i++)
                {
                    GameObject go = Instantiate(healthBarElementPrefab, segmentsRoot);
                    pool.Add(go);
                }
            }
        }

        #endregion


        
        #region Event handlers

        private void GameManager_OnPlayerHealthValueChanged(int healthValue) => SetHealth(healthValue);
        
        private void GameManager_OnReset() => SetHealth(DataContainer.PlayerPreset.PlayerLivesQuantity);

        #endregion
    }
}
