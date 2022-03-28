using System;
using System.Collections.Generic;
using Asteroids.Data;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Asteroids.Managers
{
    public class DataManager : IManager
    {
        #region Fields
        
        private static DataManager instance;
        private Dictionary<Type, Object> dataPool = new Dictionary<Type, Object>();

        #endregion

        

        #region Properties

        public static DataManager Instance => instance == null ? instance = new DataManager() : instance;

        #endregion


        
        #region Public methods

        public PlayerPreset PlayerPreset => GetData<PlayerPreset>("Data/PlayerPreset");
        
        
        public SoundPreset SoundPreset => GetData<SoundPreset>("Data/SoundPreset");


        public GamePreset GamePreset => GetData<GamePreset>("Data/GamePreset");
        
        
        public VFXPreset VFXPreset => GetData<VFXPreset>("Data/VFXPreset");

        
        public void Initialize() { }

        
        public void Unload() { }
        
        #endregion


        
        #region Private methods

        private TDataType GetData<TDataType>(string path) where TDataType : ScriptableObject
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
