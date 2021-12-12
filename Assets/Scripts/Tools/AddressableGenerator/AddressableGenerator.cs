#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Utils.AddressableExtension;
using Object = UnityEngine.Object;

namespace Tools.AddressableGenerator
{
    public class AddressableGenerator : EditorWindow
    {
        private const string WINDOW_NAME = "Addressable Generator";
        private const string MENU_PATH = "Tools/Addressable Generator";
        [MenuItem(MENU_PATH)] static void ShowWindow() => GetWindow<AddressableGenerator>(WINDOW_NAME);
        
        private bool _hasToBuild;
        private bool _autoBuild;
        
        private readonly Dictionary<string, string> _groupPath = new()
        {
            ["Levels"] = "Assets/Prefabs/Game/Levels",
            ["Menus"]  = "Assets/Prefabs/Game/Menus",
        };
        
        private void OnGUI()
        {
            EditorGUILayout.Space();
            GenerateAllAddressableButtons();
            GenerateAddressableButtons();
            ToggleAutoBuild();
            EditorGUILayout.Space();
            BuildAddressable();
        }

        private void GenerateAllAddressableButtons()
        {
            if (!GUILayout.Button("All")) return;
            foreach (var groupName in _groupPath.Keys)
                GenerateAddressableGroup(groupName,  _groupPath[groupName]);
        }
        
        private void GenerateAddressableButtons()
        {
            foreach (var groupName in _groupPath.Keys)
                GenerateGroupButton(groupName);
        }
        
        private void GenerateGroupButton(string groupName)
        {
            if (!GUILayout.Button(groupName)) return;
            GenerateAddressableGroup(groupName, _groupPath[groupName]);
        }

        private void GenerateAddressableGroup(string groupName,  string directory)
        {
            AddressableExtension.RemoveGroup(groupName);
            string[] paths = Directory.GetFiles(directory, "*.prefab", SearchOption.AllDirectories);

            foreach (var path in paths)
                CreateAddressable(groupName, path);
            _hasToBuild = true;
        }

        private void CreateAddressable(string groupName,string path)
        {
            Object prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            if (prefab is not GameObject gameObject) return;
            gameObject.CreateAddressable(groupName, gameObject.name);
        }

        private void ToggleAutoBuild()
        {
            _autoBuild = EditorGUILayout.Toggle("Auto Build", _autoBuild);
        }
        
        private void BuildAddressable()
        {
            if (!(_hasToBuild && _autoBuild)) return;
            AddressableExtension.BuildAddressable();
            _hasToBuild = false;
        }
    }
}

#endif

