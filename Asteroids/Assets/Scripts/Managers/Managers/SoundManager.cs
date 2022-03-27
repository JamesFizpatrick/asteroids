using System.Collections;
using System.Collections.Generic;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Managers
{
    public class SoundManager : MonoBehaviour, IManager
    {
        #region Fields
    
        private static SoundManager instance;

        private AudioSource audioSource;
        private DataManager dataManager;
        
        #endregion


        
        #region Properties

        public static SoundManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject managerGo = new GameObject("SoundManager");
                    SoundManager manager = managerGo.AddComponent<SoundManager>();
                    instance = manager;
                }

                return instance;
            }
        }

        #endregion


    
        #region Public methods

        public void PlaySound(SoundType soundType)
        {
            AudioClip clip = dataManager.SoundPreset.GetAudioClip(soundType);

            audioSource.clip = clip;
            audioSource.Play();
        }

        
        public void Initialize()
        {
            dataManager = ManagersHub.GetManager<DataManager>();
            
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
        }

        public void Unload() { }

        #endregion
    }
}
