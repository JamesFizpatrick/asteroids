using UnityEngine;


namespace Asteroids.Managers
{
    public class SoundManager : IManager
    {
        #region Fields
        
        private AudioSource audioSource;
        
        #endregion
        
        
        
        #region Public methods

        public void PlaySound(SoundType soundType)
        {
            AudioClip clip = DataContainer.SoundPreset.GetAudioClip(soundType);

            audioSource.clip = clip;
            audioSource.Play();
        }
        
        
        public void Initialize(ManagersHub hub)
        {
            GameObject audioSourceObject = new GameObject("AudioSource");
            
            audioSource = audioSourceObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }
               
        #endregion
    }
}
