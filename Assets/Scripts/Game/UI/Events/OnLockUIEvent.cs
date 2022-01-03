using Core.EventManager;

namespace Game.UI.Events
{
    public class OnLockUIEvent : EventData
    {
        private readonly bool _isToLock;

        public bool IsToLock => _isToLock;

        private object _id;

        public object ID => _id;

        public OnLockUIEvent(bool isToLock, object id)
        {
            _isToLock = isToLock;
            _id = id;
        }
    }
}