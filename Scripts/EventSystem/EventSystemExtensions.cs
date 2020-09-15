using System;
using System.Reflection;
using FazAppFramework.Attributes;
using FazAppFramework.Reflection;

namespace FazAppFramework.EventSystem
{
    public static class EventSystemExtensions
    {
        public static void SubscribeEventHandlers<T>(this T obj)
        {
            var methods = obj.GetMethodsWithAttribute<T, SubscribeEventHandlerAttribute>();

            for (int i = 0; i < methods.Length; i++)
            {
                var attribute = methods[i].GetCustomAttribute<SubscribeEventHandlerAttribute>();

                var handler = Delegate.CreateDelegate(typeof(GameEventHandler), obj, methods[i]) as GameEventHandler;

                EventSystemManager.Subscribe(attribute.EventType, handler);
            }
        }

        public static void UnsubscribeEventHandlers<T>(this T obj)
        {
            var methods = obj.GetMethodsWithAttribute<T, SubscribeEventHandlerAttribute>();

            for (int i = 0; i < methods.Length; i++)
            {
                var attribute = methods[i].GetCustomAttribute<SubscribeEventHandlerAttribute>();

                var handler = Delegate.CreateDelegate(typeof(GameEventHandler), obj, methods[i]) as GameEventHandler;

                EventSystemManager.Unsubscribe(attribute.EventType, handler);
            }
        }
    }
}
