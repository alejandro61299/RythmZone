using UnityEngine;

namespace Core.EventManager
{
    public abstract class EventData
    {
        public string Name => GetType().Name;
    };
}