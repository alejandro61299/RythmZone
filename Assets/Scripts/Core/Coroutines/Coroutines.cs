using System.Collections;
using Core.Singleton;
using UnityEngine;

namespace Core.Coroutines
{
    public class Coroutines : SingletonBehaviour<Coroutines>
    {
        public static Coroutine StartRoutine(IEnumerator coroutine)
        {
            return IsInstanced ? Instance.StartCoroutine(coroutine) : null;
        }
        public static void StopRoutine(IEnumerator coroutine)
        {
            if (!IsInstanced) return;
            Instance.StopCoroutine(coroutine);
        }
    }
}