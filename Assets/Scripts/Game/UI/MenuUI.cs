using Core.EventManager;
using UnityEngine;

namespace Game.UI
{
    public abstract class MenuUI : ElementUI
    {
        protected virtual void OnEnable()
        {
            EventManager.Trigger(new OnMenuEnabled(this, true));
            MenuEnabled();
        }

        protected virtual void OnDisable()
        {
            EventManager.Trigger(new OnMenuEnabled(this, false));
            MenuDisabled();
        }
        protected virtual void MenuEnabled() {}
        protected virtual void MenuDisabled() {}
    }
}