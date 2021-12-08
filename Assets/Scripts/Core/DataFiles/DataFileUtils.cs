using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Core.DataFiles
{
    public class DataFileUtils
    {
        public static void SaveFile(string filePath, DataFile dataFile)
        {
            if (!dataFile.GetType().IsSerializable) return;
            string tempPath = Path.Combine(Application.persistentDataPath, filePath);
            string jsonData = JsonUtility.ToJson(dataFile, true);
            byte[] jsonByte = Encoding.ASCII.GetBytes(jsonData);
            string directory = Path.GetDirectoryName(tempPath) ?? string.Empty;
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            File.WriteAllBytes(tempPath, jsonByte);
        }
        
        public static DataFile LoadFile(string filePath, Type type)
        {
            if (!type.IsSerializable) return null;
            string tempPath = Path.Combine(Application.persistentDataPath, filePath);
            if (!Directory.Exists(Path.GetDirectoryName(tempPath))) return null;
            byte[] jsonByte = File.ReadAllBytes(tempPath);
            string jsonData = Encoding.ASCII.GetString(jsonByte);
            object resultValue = JsonUtility.FromJson(jsonData, type);
            return (DataFile)Convert.ChangeType(resultValue, type);
        }
        
        public static T LoadFile<T>(string filePath) where T : DataFile
        {
            DataFile dataFile = LoadFile(filePath, typeof(T));
            return dataFile is T finalDataFile ? finalDataFile : null;
        }
    }
}