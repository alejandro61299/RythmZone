using System;
using System.Collections.Generic;
using Core.Singleton;

namespace Core.EventManager
{
    public abstract class Event
    {
    };

    public class EventManager : SingletonBehaviour<EventManager>
    {
        public delegate void EventDelegate<T>(T e) where T : Event;

        private delegate void EventDelegate(Event eventData);

        private readonly Dictionary<Type, EventDelegate> _events = new();
        private readonly Dictionary<Delegate, EventDelegate> _delegates = new();

        public static void TriggerEvent(Event eventData)
        {
            if (!IsInstanced) return;
            if (!Instance._events.TryGetValue(eventData.GetType(), out EventDelegate eventDelegate)) return;
            eventDelegate?.Invoke(eventData);
        }

        public static void AddListener<T>(EventDelegate<T> del) where T : Event
        {
            if (!IsInstanced) return;
            if (Instance._delegates.ContainsKey(del)) return; // Already contained 

            void NewDelegate(Event e) => del((T)e);
            Instance._delegates[del] = NewDelegate;
            bool found = Instance._events.TryGetValue(typeof(T), out EventDelegate dDelegate);
            Instance._events[typeof(T)] = found ? dDelegate + NewDelegate : NewDelegate;
        }

        public static void RemoveListener<T>(EventDelegate<T> del) where T : Event
        {
            if (!IsInstanced) return;
            if (!Instance._delegates.TryGetValue(del, out EventDelegate foundDelegate)) return;
            if (Instance._events.TryGetValue(typeof(T), out EventDelegate eventDelegate))
            {
                eventDelegate -= foundDelegate;
                if (eventDelegate.IsNull()) Instance._events.Remove(typeof(T));
                else Instance._events[typeof(T)] = eventDelegate;
            }

            Instance._delegates.Remove(del);
        }


        public static void RemoveAllListeners()
        {
            if (!IsInstanced) return;
            Instance._events.Clear();
            Instance._delegates.Clear();
        }
        

    }
    
    public static class ObjectExtension
    {
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }
    }
}



