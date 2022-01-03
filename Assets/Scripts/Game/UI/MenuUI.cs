using Core.EventManager;
using Game.UI.Events;
using Game.UI.Lock;
using UnityEngine;

namespace Game.UI
{
    public abstract class MenuUI : UIElement
    {
        protected virtual void OnEnable()
        {
            EventManager.Trigger(new OnMenuEnabledEvent(this, true));
        }

        protected virtual void OnDisable()
        {
            EventManager.Trigger(new OnMenuEnabledEvent(this, false));
        }
    }
}