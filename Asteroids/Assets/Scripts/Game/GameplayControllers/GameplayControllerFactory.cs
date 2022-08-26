using System;
using Asteroids.Managers;


namespace Asteroids.Game
{
    public static class GameplayControllerFactory
    {
        public static BaseGameplayController CreateGameplayController(GameType gameType, IManagersHub hub)
        {
            switch (gameType)
            {
                case GameType.Classic:
                    return new ClassicGameplayController(hub.GetManager<PlayerShipsManager>(),
                        hub.GetManager<EnemiesManager>(),
                        hub.GetManager<AsteroidsManager>());
                case GameType.Survival:
                    return new SurvivalGameplayController(hub.GetManager<PlayerShipsManager>(),
                        hub.GetManager<EnemiesManager>(),
                        hub.GetManager<AsteroidsManager>());
                case GameType.None:
                    throw new InvalidOperationException("GameType not specified!");
            }

            return null;
        }
    }
}
