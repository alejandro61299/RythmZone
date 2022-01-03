using Core.EventManager;

namespace Game.UI.Lock
{
    public interface ILockableUIElement
    {
        public void Lock(object id);
        public void Unlock(object id);
    }
}