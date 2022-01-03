#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Utils.AddressableExtension;
using Utils.AssetsExtension;
using Object = UnityEngine.Object;

namespace Tools.AddressableGenerator
{
    public class AddressableGenerator : EditorWindow
    {
        private const string WINDOW_NAME = "Addressable Generator";
        private const string MENU_PATH = "Tools/Addressable Generator";
        private const string SCRIPTABLE_PATH = "Assets/ScriptableObjects/Tools/AddressableGenerator/AddressablesContainer.asset";
        [MenuItem(MENU_PATH)] static void ShowWindow() => GetWindow<AddressableGenerator>(WINDOW_NAME);
        
        private AddressablesContainer _container;
        private bool _hasToBuild;
        private bool _autoBuild;
        private List<AddressableInfo> AddressableInfos => _container.AddressableInfos;
        
        private void OnEnable()
        {
            _container = AssetDatabase.LoadAssetAtPath<AddressablesContainer>(SCRIPTABLE_PATH);
        }

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
            foreach (var info in AddressableInfos)
                GenerateAddressableGroup(info);
        }
        
        private void GenerateAddressableButtons()
        {
            foreach (var info in AddressableInfos)
                GenerateGroupButton(info);
        }
        
        private void GenerateGroupButton(AddressableInfo info)
        {
            if (!GUILayout.Button(info.GroupName)) return;
            GenerateAddressableGroup(info);
        }

        private void GenerateAddressableGroup(AddressableInfo info)
        {
            AddressableExtension.RemoveGroup(info.GroupName);
            string searchPatter = AssetsExtension.GetSearchPattern(info.AssetsType);
            string[] paths = Directory.GetFiles(info.Path, searchPatter, SearchOption.AllDirectories);

            foreach (var path in paths)
                CreateAddressable(info.GroupName, path);
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

