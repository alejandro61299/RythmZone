using System;
using Core.EventManager;
using Game.UI;
using UnityEngine;

namespace Game.Menu
{
    public class MenuUI : MonoBehaviour
    {
        private void OnEnable()
        {
            EventManager.Trigger(new OnMenuEnabled(this, true));
        }

        private void OnDisable()
        {
            EventManager.Trigger(new OnMenuEnabled(this, false));
        }
    }
}