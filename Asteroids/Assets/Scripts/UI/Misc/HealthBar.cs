using System.Collections.Generic;
using Asteroids.Data;
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

        private IPlayerShipsManager playerShipsManager;

        #endregion



        #region Unity lifecycle

        public void OnDestroy()
        {
            playerShipsManager.OnPlayerHealthValueChanged -= GameManager_OnPlayerHealthValueChanged;
        }

        #endregion



        #region Public methods

        public void Init(IPlayerShipsManager playerShipsManager)
        {
            this.playerShipsManager = playerShipsManager;

            playerShipsManager.OnPlayerHealthValueChanged += GameManager_OnPlayerHealthValueChanged;

            SetHealth(DataContainer.GamePreset.PlayerLivesQuantity);
        }

        #endregion



        #region Private methods

        private void SetHealth(int healthValue)
        {
            if (pool.Count >= healthValue)
            {
                for (int i = 0; i < pool.Count; i++)
                {
                    pool[i].SetActive(i < healthValue);
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
        
        #endregion
    }
}
