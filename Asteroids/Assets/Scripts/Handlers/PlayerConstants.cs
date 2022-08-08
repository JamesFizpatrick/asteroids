namespace Asteroids.Handlers
{
    public static class PlayerConstants
    {
        // Player
        public const int InitialPlayerSafeRadius = 200;
        public const float IFramesBlinkingFrequency = 0.25f;

        public const int RespawnDistanceFromBorders = 100;
        public const int RespawnFieldSegmentsGridModule = 10;

        public const float InertiaIncreaseSpeed = 0.02f;
        public const float InertiaDecreaseSpeed = 0.005f;


        // Asteroids
        public const float AsteroidRotationSpeed = 0.7f;
        public const int AsteroidBorderGap = 60;


        // UFO
        public const int UFOSpawnDistance = 10;


        // Weapons
        public const int MaxPoolBulletsAmount = 20;
    }
}
