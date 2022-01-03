using System.Collections.Generic;
using Core.EventManager;
using Game.UI.Events;

namespace Game.UI.Controllers
{
    public class MenusController : Controller
    {
        private readonly List<MenuUI> _menus = new ();
        protected override void RegisterEvents() => EventManager.Register<OnMenuEnabledEvent>(OnMenuEnabled);
        protected override void UnregisterEvents() => EventManager.Unregister<OnMenuEnabledEvent>(OnMenuEnabled);
        
        private void OnMenuEnabled(OnMenuEnabledEvent eventData)
        {
            if (eventData.Enabled) 
                _menus.Add(eventData.MenuUI);
            else _menus.Remove(eventData.MenuUI);
        }
        
    }
}