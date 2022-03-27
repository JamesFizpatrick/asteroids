using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Managers;
using UnityEngine;


namespace Asteroids.Data
{
    [CreateAssetMenu(fileName = "CustomAssets/SoundsPreset")]
    public class SoundPreset : ScriptableObject
    {
        #region Nested types

        [Serializable]
        private struct Audio
        {
            public SoundType soundType;
            public AudioClip audioClip;
        }

        #endregion


        
        #region Fields

        [SerializeField] private Audio[] clipsList;

        #endregion


        
        #region Public methods

        public AudioClip GetAudioClip(SoundType soundType, int index = -1)
        {
            List<Audio> audioList = clipsList.Where(x => x.soundType == soundType).ToList();

            if (audioList.Count == 0)
            {
                return null;
            }
            
            Audio audio = index >= 0 ? audioList[index] : audioList[0];

            return audio.audioClip;
        }

        #endregion
    }
}
