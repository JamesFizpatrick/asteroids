using Asteroids.Game;


namespace Asteroids.UFO
{
    public class UFOWeaponController : UnitWeaponController
    {
        protected override void Awake()
        {
            currentWeaponType = WeaponType.Enemy;
            base.Awake();
        }

        private void OnEnable()
        {
            StartFire();
        }


        private void OnDisable()
        {
            StopFire();
        }
    }
}
