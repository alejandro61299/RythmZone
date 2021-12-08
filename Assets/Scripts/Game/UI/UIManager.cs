using System;
using System.Collections.Generic;
using Core.EventManager;
using UnityEngine;

namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {
        private readonly List<MenuUI> _menus = new ();

        private void Awake() => DontDestroyOnLoad(gameObject);

        private void OnEnable()
        {
            EventManager.Register<OnMenuEnabled>(OnMenuEnabled);
        }

        private void OnDisable()
        {
            EventManager.Unregister<OnMenuEnabled>(OnMenuEnabled);
        }

        private void OnMenuEnabled(OnMenuEnabled eventData)
        {
            if (eventData.Enabled) 
                 _menus.Add(eventData.Menu);
            else _menus.Remove(eventData.Menu);
        }
        
    }
    
    public class OnMenuEnabled : EventData
    {
        private readonly bool _enabled;
        private readonly MenuUI _menu;

        public bool Enabled => _enabled;
        public MenuUI Menu => _menu;

        public OnMenuEnabled(MenuUI menu, bool enabled)
        {
            _menu = menu;
            _enabled = enabled;
        }
    }
}