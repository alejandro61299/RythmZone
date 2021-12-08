using System;
using System.Collections.Generic;
using Core.DataFiles;
using Core.Singleton;
using Game.SaveData;

namespace Game.SaveLoader
{
    public class SaveLoader : SingletonBehaviour<SaveLoader>
    {
        private static readonly Dictionary<string, DataFile> _files = new()
        {
            ["Data/UserSave.data"] = new UserData()
        };

        protected override void Awake()
        {
            base.Awake();
            Dictionary<string, DataFile> toReplace = new();

            foreach (string filePath in _files.Keys)
            {
                DataFile loadedFile = DataFileUtils.LoadFile(filePath, _files[filePath].GetType());
                if (loadedFile != null) toReplace.Add(filePath, loadedFile);
            }

            foreach (string filePath in toReplace.Keys)
                _files[filePath] = toReplace[filePath];
        }

        protected override void OnDestroy()
        {
            foreach (string filePath in _files.Keys)
            {
               DataFileUtils.SaveFile(filePath, _files[filePath]);
            }
        }
    }
}