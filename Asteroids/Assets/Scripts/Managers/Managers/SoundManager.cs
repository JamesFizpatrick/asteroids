using UnityEngine;


namespace Asteroids.Managers
{
    public class SoundManager : ISoundManager
    {
        #region Fields
        
        private const string AudiosourceName = "AudioSource";
        private AudioSource audioSource;

        #endregion
        
        
        
        #region Public methods

        public void PlaySound(SoundType soundType)
        {
            AudioClip clip = DataContainer.SoundPreset.GetAudioClip(soundType);

            audioSource.clip = clip;
            audioSource.Play();
        }
        
        
        public void Initialize(IManagersHub hub)
        {
            GameObject audioSourceObject = new GameObject(AudiosourceName);
            
            audioSource = audioSourceObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }
               
        #endregion
    }
}
