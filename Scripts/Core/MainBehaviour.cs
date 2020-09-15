using System;
using System.Collections;
using FazAppFramework.DI;
using FazAppFramework.EventSystem;
using UnityEngine;

namespace FazAppFramework
{
    public abstract class MainBehaviour : MonoBehaviour
    {
        protected void Awake()
        {
#if UNITY_EDITOR
            this.CheckSerializedFields();
#endif
            this.GetComponents();
            this.ResolveDependencies();
            this.SubscribeEventHandlers();

            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }
        
        protected virtual void Publish<T>(T gameEvent) where T : GameEvent
        {
            gameEvent.Publish(this);
        }

        protected void InvokeWithDelay(float delay, Action action)
        {
            StartCoroutine(InvokeWithDelayEnumerator(delay, action));
        }

        protected virtual void OnDestroy()
        {
            this.UnsubscribeEventHandlers();
        }
        
        private IEnumerator InvokeWithDelayEnumerator(float delay, Action action)
        {
            yield return new WaitForSecondsRealtime(delay);
            action?.Invoke();
        }
    }
}
