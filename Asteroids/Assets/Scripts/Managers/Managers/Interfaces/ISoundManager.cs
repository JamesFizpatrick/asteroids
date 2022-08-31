namespace Asteroids.Managers
{
    public interface ISoundManager : IManager
    {
        void PlaySound(SoundType soundType);
    }
}
