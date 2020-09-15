using System;
using System.Collections;
using UnityEngine;

namespace FazAppFramework.Utils
{
    public class CoroutineHelper : MonoBehaviour
    {
        private static CoroutineHelper instance = null;

        private void Awake()
        {
            instance = this;
        }

        public static Coroutine InvokeWithDelay(float delay, Action action)
        {
            return instance.StartCoroutine(instance.InvokeWithDelayCoroutine(delay, action));
        }
        
        private IEnumerator InvokeWithDelayCoroutine(float delay, Action action)
        {
            yield return new WaitForSecondsRealtime(delay);
            action?.Invoke();
        }

        public static void StopInvoke(Coroutine coroutine)
        {
            instance.StopCoroutine(coroutine);
        }

        public static Coroutine StartCoroutineHelperFunction(IEnumerator iEnumerator)
        {
            return instance.StartCoroutine(iEnumerator);
        }

        public static void StopCoroutineHelperFunction(Coroutine coroutine)
        {
            instance.StopCoroutine(coroutine);
        }
    }
}
