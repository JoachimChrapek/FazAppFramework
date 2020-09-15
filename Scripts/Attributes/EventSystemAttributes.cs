using System;

namespace FazAppFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class SubscribeEventHandlerAttribute : Attribute
    {
        public Type EventType { get; }

        public SubscribeEventHandlerAttribute(Type eventType)
        {
            EventType = eventType;
        }
    }
}
