using System;
using Core.EventManager;
using Game.UI.Controllers;
using Game.UI.Events;
using UnityEngine;
using Utils.ControllersManager;

namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {
        private readonly ControllersManager _manager = new ( new MenusController());

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _manager.Initialize();
        }

        private void OnDestroy()
        {
            _manager.Terminate();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K)) EventManager.Trigger(new OnLockUIEvent(true ,this) );
            if (Input.GetKeyDown(KeyCode.L)) EventManager.Trigger(new OnLockUIEvent(false ,this) );
        }
    }
}