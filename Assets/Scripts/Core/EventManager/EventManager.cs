using System;
using System.Collections.Generic;
using Utils.ObjectsExtension;

namespace Core.EventManager
{
    public static class  EventManager
    {
        public delegate void EventDelegate<in T>(T e) where T : EventData;
        private delegate void EventDelegate(EventData eventData);

        private static readonly Dictionary<Type, EventDelegate> _events = new();
        private static readonly Dictionary<Delegate, EventDelegate> _delegates = new();

        public static void Trigger(EventData eventData)
        {
            if (!_events.TryGetValue(eventData.GetType(), out EventDelegate eventDelegate)) return;
            eventDelegate?.Invoke(eventData);
        }

        public static void Register<T>(EventDelegate<T> del) where T : EventData
        {
            if (_delegates.ContainsKey(del)) return; // Already contained 
            void NewDelegate(EventData e) => del((T)e);
            _delegates[del] = NewDelegate;
            bool found = _events.TryGetValue(typeof(T), out EventDelegate dDelegate);
            _events[typeof(T)] = found ? dDelegate + NewDelegate : NewDelegate;
        }

        public static void Unregister<T>(EventDelegate<T> del) where T : EventData
        {
            if (!_delegates.TryGetValue(del, out EventDelegate foundDelegate)) return;
            if (_events.TryGetValue(typeof(T), out EventDelegate eventDelegate))
            {
                eventDelegate -= foundDelegate;
                if (eventDelegate.Null()) _events.Remove(typeof(T));
                else _events[typeof(T)] = eventDelegate;
            }
            _delegates.Remove(del);
        }
        
        public static void RemoveAllListeners()
        {
            _events.Clear();
            _delegates.Clear();
        }
    }
    

}



