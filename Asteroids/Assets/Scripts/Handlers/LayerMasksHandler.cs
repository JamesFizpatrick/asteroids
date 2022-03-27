using UnityEngine;


namespace Asteroids.Handlers
{
    public static class LayerMasksHandler
    {
        #region Properties

        public static int PlayerProjectiles => GetLayer("PlayerProjectiles");
        
        public static int EnemyProjectiles => GetLayer("EnemyProjectiles");
        
        public static int Asteroid => GetLayer("Asteroid");

        public static int Enemy => GetLayer("Enemy");
        
        public static int Player => GetLayer("Player");

        #endregion



        
        #region Private methods

        private static int GetLayer(string layer) => LayerMask.NameToLayer(layer);
        
        #endregion
    }
}
