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
                    return new ClassicGameplayController(
                        hub.GetManager<IPlayerShipsManager>(),
                        hub.GetManager<IEnemiesManager>(),
                        hub.GetManager<IAsteroidsManager>(),
                        hub.GetManager<IPlayerProgressManager>());
                case GameType.Survival:
                    return new SurvivalGameplayController(
                        hub.GetManager<IPlayerShipsManager>(),
                        hub.GetManager<IEnemiesManager>(),
                        hub.GetManager<IAsteroidsManager>(),
                        hub.GetManager<IPlayerProgressManager>());
                case GameType.None:
                    throw new InvalidOperationException("GameType not specified!");
            }

            return null;
        }
    }
}
