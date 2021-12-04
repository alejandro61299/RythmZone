using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class SingletonBehaviour<T> : MonoBehaviour where T: SingletonBehaviour<T>
    {
        private static T _instance;
        protected static bool IsInstanced => Instance != null;
        protected static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance ??= FindObjectOfType (typeof (T)) as T;
                _instance ??= new GameObject(typeof(T).Name).AddComponent<T>();
                return _instance;
            }
        }
    }
}


