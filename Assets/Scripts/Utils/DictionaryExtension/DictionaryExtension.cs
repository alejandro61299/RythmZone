using System;
using System.Collections.Generic;

namespace Utils.DictionaryExtension
{
    public static class DictionaryExtension
    {
        public static void ForEachKey<T1, T2>(this Dictionary<T1, T2> dictionary, Action<T1> action)
        {
            foreach (var item in dictionary.Keys)
                action?.Invoke(item);
        }
        
        public static void ForEachValue<T1, T2>(this Dictionary<T1, T2> dictionary, Action<T2> action)
        {
            foreach (var item in dictionary.Values)
                action?.Invoke(item);
        }
    }
}