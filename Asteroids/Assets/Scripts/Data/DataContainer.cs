using System;
using System.Collections.Generic;
using Asteroids.Data;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Asteroids.Managers
{
    public class DataContainer
    {
        #region Fields
        
        private static Dictionary<Type, Object> dataPool = new Dictionary<Type, Object>();

        #endregion

        
        
        #region Public methods

        public static PlayerPreset PlayerPreset => GetData<PlayerPreset>("Data/PlayerPreset");
        
        
        public static SoundPreset SoundPreset => GetData<SoundPreset>("Data/SoundPreset");


        public static GamePreset GamePreset => GetData<GamePreset>("Data/GamePreset");
        
        
        public static VFXPreset VFXPreset => GetData<VFXPreset>("Data/VFXPreset");
        
        #endregion


        
        #region Private methods

        private static TDataType GetData<TDataType>(string path) where TDataType : ScriptableObject
        {
            Type type = typeof(TDataType);

            if (!dataPool.ContainsKey(type))
            {
                TDataType data = Resources.Load<TDataType>(path);
                dataPool.Add(type, data);
                return data;
            }

            return dataPool[type] as TDataType;
        }

        #endregion
    }
}
