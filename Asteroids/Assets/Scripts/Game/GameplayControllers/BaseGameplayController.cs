using System;


namespace Asteroids.Game
{
    public abstract class BaseGameplayController
    {
        public Action OnPlayerWin;
        
        public Action OnPlayerLose;
        
        
        public abstract void StartGame();

        
        public abstract void StopGame();

        
        public abstract void Reset();
    }
}
