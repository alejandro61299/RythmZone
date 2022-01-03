namespace Game.UI.Controllers
{
    public abstract class Controller
    {
        public bool IsEnabled => _isEnabled;
        private bool _isEnabled;

        protected virtual void RegisterEvents() {}
        protected virtual void UnregisterEvents() {}

        public void Enable()
        {
            if (_isEnabled) return;
            _isEnabled = true;
            RegisterEvents();
        }

        public void Disable()
        {
            if (!_isEnabled) return;
            _isEnabled = false;
            UnregisterEvents();
        }
    }
}