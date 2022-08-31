using Asteroids.VFX;
using UnityEngine;


namespace Asteroids.Managers
{
    public interface IVfxManager : IManager, IUnloadableManager
    {
        void SpawnVFX(VFXType type, Vector3 position);
        void Reset();
    }
}
