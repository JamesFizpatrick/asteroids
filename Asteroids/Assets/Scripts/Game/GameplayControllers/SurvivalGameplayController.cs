using System;
using Asteroids.Asteroids;
using Asteroids.Data;
using Asteroids.Handlers;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Game
{
    public class SurvivalGameplayController : BaseGameplayController
    {
        #region Fields

        public Action<ulong> OnScoreChanged;
        
        private readonly IPlayerShipsManager playerShipsManager;
        private readonly IEnemiesManager enemiesManager;
        private readonly IAsteroidsManager asteroidsManager;
        private readonly IPlayerProgressManager progressManager;

        private LevelsPreset.LevelPreset currentLevelPreset;
        private ScorePreset scorePreset;

        private ulong score;
        
        #endregion

        

        #region Class lifecycle

        public SurvivalGameplayController(IPlayerShipsManager playerShipsManager, IEnemiesManager enemiesManager,
            IAsteroidsManager asteroidsManager, IPlayerProgressManager progressManager)
        {
            this.playerShipsManager = playerShipsManager;
            this.enemiesManager = enemiesManager;
            this.asteroidsManager = asteroidsManager;
            this.progressManager = progressManager;
        }

        #endregion


        #region Public methods

        public override void StartGame()
        {
            SetScore(0);

            scorePreset = DataContainer.ScorePreset;
            LevelsPreset gamePreset = DataContainer.LevelsPreset;
            currentLevelPreset = gamePreset.GetSurvivalModePreset;

            InitialSpawnEntities();
            SubscribeToEvents();
        }

        
        public override void StopGame()
        {
            ResetManagers();
            UnsubscribeFromEvents();
        }

        
        public override void Reset()
        {
            StopGame();
            SetScore(0);
        }

        #endregion


        
        #region Private methods

        private void InitialSpawnEntities()
        {
            playerShipsManager.SpawnPlayer();
            enemiesManager.StartSpawnCoroutine(currentLevelPreset.EnemiesDelay);
            
            asteroidsManager.SpawnAsteroids(currentLevelPreset.AsteroidsCount,
                Vector3Int.FloorToInt(playerShipsManager.GetPlayer().transform.localPosition),
                PlayerConstants.InitialPlayerSafeRadius);
        }


        private void ResetManagers()
        {
            playerShipsManager.Reset();
            asteroidsManager.Reset();
            enemiesManager.Reset();

            UnsubscribeFromEvents();
        }

        
        private void SubscribeToEvents()
        {
            playerShipsManager.OnPlayerKilled += PlayerShipsManager_OnPlayerKilled;
            playerShipsManager.OnPlayerHealthValueChanged += PlayerShipManager_OnPlayerHealthValueChanged;
            
            asteroidsManager.OnFracturesDestroyed += AsteroidsManager_OnFracturesDestroyed;
            asteroidsManager.OnAsteroidDestroyed += AsteroidsManager_OnAsteroidDestroyed;

            enemiesManager.OnEnemyKilled += EnemiesManager_OnEnemyKilled;
        }
        
        
        private void UnsubscribeFromEvents()
        {
            playerShipsManager.OnPlayerKilled -= PlayerShipsManager_OnPlayerKilled;
            playerShipsManager.OnPlayerHealthValueChanged -= PlayerShipManager_OnPlayerHealthValueChanged;
            
            asteroidsManager.OnFracturesDestroyed -= AsteroidsManager_OnFracturesDestroyed;
            asteroidsManager.OnAsteroidDestroyed -= AsteroidsManager_OnAsteroidDestroyed;

            enemiesManager.OnEnemyKilled -= EnemiesManager_OnEnemyKilled;
        }
        
        
        private void SetScore(int to)
        {
            score = (ulong)to;
            OnScoreChanged?.Invoke(score);
        }


        private void AddScore(int scoreToAdd)
        {
            score += (ulong)scoreToAdd;
            OnScoreChanged?.Invoke(score);
        }


        private void ProcessPlayerWin()
        {
            OnPlayerWin?.Invoke();
            progressManager.SetCurrentSurvivalHighScore(score);
        }
        
        #endregion



        #region Event handlers

        private void PlayerShipsManager_OnPlayerKilled()
        {
            ResetManagers();
            
            // While in survival mode a player cannot lose due to the idea of survival mode
            ProcessPlayerWin();
        }


        private void PlayerShipManager_OnPlayerHealthValueChanged(int newValue)
        {
            if (asteroidsManager.GetActiveAsteroidsCount() == 0 && !enemiesManager.HasActiveEnemy())
            {
                ProcessPlayerWin();
            }
            else
            {
                playerShipsManager.RespawnPlayer(1f, 1f, 2f);
            }
        }

        
        private void EnemiesManager_OnEnemyKilled() => AddScore(scorePreset.GetEnemyPoints());


        private void AsteroidsManager_OnFracturesDestroyed() => asteroidsManager.SpawnNewAsteroidOutOfFOV();

        
        private void AsteroidsManager_OnAsteroidDestroyed(Asteroid asteroid) => 
            AddScore(scorePreset.GetAsteroidScore(asteroid.Type));

        #endregion
    }
}
