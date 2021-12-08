#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Utils.AddressableExtension;
using Object = UnityEngine.Object;

namespace Tools.AddressableGenerator
{
    public class AddressableGeneratorWindow : EditorWindow
    {
        private static readonly string WINDOW_NAME =
            "Addressable Generator";
        
        private readonly Dictionary<string, string> _groupPath = new()
        {
            ["Levels"] = "Assets/Prefabs/Game/Levels",
            ["Menus"]  = "Assets/Prefabs/Game/Menus",
        };

        private bool _hasToBuild;
        private bool _autoBuild;
        private void ToBuild() => _hasToBuild = true;
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
            {
                Object prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
                if (prefab is not GameObject gameObject) continue;
                gameObject.CreateAddressable(groupName, gameObject.name);
            }
            
            ToBuild();
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
        

        [MenuItem("Tools/Addressable Generator")]
        public static void ShowWindow()
        {
            GetWindow<AddressableGeneratorWindow>(WINDOW_NAME);
        }

    }
}

#endif

