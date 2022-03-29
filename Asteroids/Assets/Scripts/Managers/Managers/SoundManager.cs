using UnityEngine;


namespace Asteroids.Managers
{
    public class SoundManager : BaseManager<SoundManager>
    {
        #region Fields
        
        private AudioSource audioSource;
        private DataManager dataManager;
        
        #endregion
        
        
        
        #region Public methods

        public void PlaySound(SoundType soundType)
        {
            AudioClip clip = dataManager.SoundPreset.GetAudioClip(soundType);

            audioSource.clip = clip;
            audioSource.Play();
        }
        
        #endregion

        

        #region Protected methods

        protected override void Initialize()
        {
            dataManager = ManagersHub.GetManager<DataManager>();
            
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }

        protected override void Deinitialize() { }

        #endregion
    }
}
