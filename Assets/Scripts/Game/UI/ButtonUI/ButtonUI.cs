using System;
using Game.UI.Lock;
using UnityEngine;

namespace Game.UI.ButtonUI
{
    public class ButtonUI : UIElement, ILockableUIElement
    {
        private readonly ButtonUILocker _locker =  new ();
        private void Awake()
        {
            _locker.Initialize(this);
        }
        private void OnDestroy()
        {
            _locker.Terminate();
        }

        public void Lock(object id) => _locker.AddLock(id);
        public void Unlock(object id) => _locker.RemoveLock(id);
    }
}