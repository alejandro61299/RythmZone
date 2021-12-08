#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using Object = UnityEngine.Object;

namespace Utils.AddressableExtension
{
    public static class AddressableExtension
    {
        private static AddressableAssetSettings Settings => AddressableAssetSettingsDefaultObject.Settings;
        
        public static void RemoveGroup(string groupName)
        {
            AddressableAssetGroup group = Settings.FindGroup(groupName);
            if (!group) return;
            Settings.RemoveGroup(group);
        }

        public static void BuildAddressable()
        {
            AddressableAssetSettings.BuildPlayerContent();
        }
        
        public static void CreateAddressable(this Object obj, string groupName, string address)
        {
            AddressableAssetGroup group = FindOrCreateGroup(groupName);
            AddressableAssetEntry entry = CreateOrMoveEntry(obj, group);
            entry.SetAddress(address);
        }
        private static AddressableAssetGroup FindOrCreateGroup(string groupName)
        {
            AddressableAssetGroup group = Settings.FindGroup(groupName);
            group ??= Settings.CreateGroup(
                groupName, false, false, true, null, 
                typeof(ContentUpdateGroupSchema), typeof(BundledAssetGroupSchema));
            return group;
        }
        private static AddressableAssetEntry CreateOrMoveEntry(Object obj, AddressableAssetGroup group)
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);
            string guid = AssetDatabase.AssetPathToGUID(assetPath);
            return Settings.CreateOrMoveEntry(guid, group, false, false);
        }
    }
}

#endif