using System;
using System.Collections.Generic;
using UnityEngine;
using static Utils.AssetsExtension.AssetsExtension;

namespace Tools.AddressableGenerator
{
    [CreateAssetMenu(fileName = "AddressablesContainer", menuName = "ScriptableObject/Tool/AddressablesContainer", order = 0)]
    public class AddressablesContainer : ScriptableObject
    {
        [SerializeField] private List<AddressableInfo> _addressableInfos;
        public List<AddressableInfo> AddressableInfos => _addressableInfos;
    }

    [Serializable]
    public class AddressableInfo
    {
        [SerializeField] private AssetType _assetsType;
        [SerializeField] private string _groupName;
        [SerializeField] private string _path;
        
        public AssetType AssetsType => _assetsType;
        public string GroupName => _groupName;
        public string Path => _path;
    }
}