using Core.EventManager;

namespace Game.UI.Events
{
    public class OnMenuEnabledEvent : EventData
    {
        private readonly bool _enabled;
        private readonly MenuUI _menuUI;

        public bool Enabled => _enabled;
        public MenuUI MenuUI => _menuUI;

        public OnMenuEnabledEvent(MenuUI menuUI, bool enabled)
        {
            _menuUI = menuUI;
            _enabled = enabled;
        }
    }
}