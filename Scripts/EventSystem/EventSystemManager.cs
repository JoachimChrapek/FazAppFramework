using System;
using System.Collections.Generic;

namespace FazAppFramework.EventSystem
{
    public static class EventSystemManager
    {
        private static readonly Dictionary<Type, List<GameEventHandler>> SubscribedEvents = new Dictionary<Type, List<GameEventHandler>>();

        public static void Subscribe<T>(GameEventHandler handler) where T : GameEvent
        {
            Subscription(typeof(T), handler);
        }

        public static void Subscribe(Type T, GameEventHandler handler)
        {
            Subscription(T, handler);
        }

        public static void Unsubscribe<T>(GameEventHandler handler) where T : GameEvent
        {
            Unsubscription(typeof(T), handler);
        }

        public static void Unsubscribe(Type T, GameEventHandler handler)
        {
            Unsubscription(T, handler);
        }

        public static void UnsubcribeAllHandlers<T>() where T : GameEvent
        {
            var eventType = typeof(T);
            
            if (SubscribedEvents.ContainsKey(eventType))
            {
                SubscribedEvents[eventType].Clear();
            }
        }

        public static void Publish<T>() where T : GameEvent, new()
        {
            Publish(null, new T());
        }

        public static void Publish<T>(object sender) where T : GameEvent, new()
        {
            Publish(sender, new T());
        }

        public static void Publish<T>(T gameEvent) where T : GameEvent
        {
            Publish(null, gameEvent);
        }

        public static void Publish<T>(object sender, T gameEvent) where T : GameEvent
        {
            var eventType = typeof(T);
            
            if (!SubscribedEvents.ContainsKey(eventType))
                return;

            gameEvent.SetSender(sender);
            
            var tmp = SubscribedEvents[eventType];

            for (int i = tmp.Count - 1; i >= 0; i--)
            {
                if (!(gameEvent).Blocked)
                {
                    if (SubscribedEvents[eventType].Contains(tmp[i]))
                    {
                        tmp[i]?.Invoke(gameEvent);
                    }
                }
            }
        }

        private static void Subscription(Type eventType, GameEventHandler handler)
        {
            if (!SubscribedEvents.ContainsKey(eventType))
            {
                SubscribedEvents.Add(eventType, new List<GameEventHandler>());
            }

            if (!SubscribedEvents[eventType].Contains(handler))
            {
                SubscribedEvents[eventType].Add(handler);
            }
        }

        private static void Unsubscription(Type eventType, GameEventHandler handler)
        {
            if (SubscribedEvents.ContainsKey(eventType))
            {
                SubscribedEvents[eventType].Remove(handler);
            }
        }
    }
}
