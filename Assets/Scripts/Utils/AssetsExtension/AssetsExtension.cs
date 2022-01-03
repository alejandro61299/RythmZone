using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Utils.AssetsExtension
{
    public static class AssetsExtension
    {
        public enum AssetType { Prefab, ScriptableObject }

        private static readonly Dictionary<AssetType, string> _extensionString = new()
        {
            [AssetType.Prefab] = "prefab",
            [AssetType.ScriptableObject] = "asset"
        };

        public static string GetExtensionString(AssetType assetType) => "." + _extensionString[assetType];
        public static string GetSearchPattern(AssetType assetType) => "*" + GetExtensionString(assetType);

    }
}