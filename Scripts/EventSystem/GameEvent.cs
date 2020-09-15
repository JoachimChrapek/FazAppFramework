using System;

namespace FazAppFramework.EventSystem
{
    public abstract class GameEvent
    {
        public string Identifier => GetType().FullName;

        public object Sender { get; private set; }
        internal void SetSender(object sender) => Sender = sender;

        public bool Blocked { get; set; }

        public void Publish()
        {
            EventSystemManager.Publish(null, this);
        }

        public void Publish(object sender)
        {
            EventSystemManager.Publish(sender, this);
        }
    }
}
