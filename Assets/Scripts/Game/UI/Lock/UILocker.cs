using System.Collections.Generic;
using Core.EventManager;
using Game.UI.Events;
using Utils.ListExtension;

namespace Game.UI.Lock
{
    public abstract class UILocker<T> where T : ILockableUIElement
    {
        protected T Element;
        private readonly List<object> _lockIDs = new ();

        public void Initialize(T element)
        {
            Element = element;
            EventManager.Register<OnLockUIEvent>(OnLockUIEvent);
        }

        public void Terminate()
        {
            EventManager.Unregister<OnLockUIEvent>(OnLockUIEvent);
        }
        
        private void OnLockUIEvent(OnLockUIEvent eventData)
        {
            if (eventData.IsToLock) AddLock(eventData.ID);
            else                    RemoveLock(eventData.ID);
        }
        
        /// <summary>
        /// Add lock to lock IDs list
        /// </summary>
        /// <param name="id">Lock identifier</param>
        public void AddLock(object id)
        {
            if (_lockIDs.Contains(id)) return;
            if (_lockIDs.IsEmpty()) Lock();
            _lockIDs.Add(id);
        }

        /// <summary>
        /// Remove lock from lock IDs list
        /// </summary>
        /// <param name="id">Lock identifier</param>
        public void RemoveLock(object id)
        {
            if (!_lockIDs.Contains(id)) return;
            _lockIDs.Remove(id);
            if (_lockIDs.IsEmpty()) Unlock();
        }

        protected abstract void Lock();
        protected abstract void Unlock();
    }
}