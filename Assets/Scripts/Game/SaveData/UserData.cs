using System;
using Core.DataFiles;

namespace Game.SaveData
{
    [Serializable]
    public class UserData : DataFile
    {
        public string lastLevel;
        
        public UserData()
        {
            lastLevel = "TestLevel";
        }
        
      
    }
}