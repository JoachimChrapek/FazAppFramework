using System;
using FazAppFramework.DI;

namespace FazAppFramework.Managers
{
    public enum ServiceStatus
    {
        NotInitialized,
        Off,
        Initialized,
        FailedToInitialize
    }

    public abstract class Service
    {
        private ServiceStatus status;
        public ServiceStatus Status {
            get => status;
            protected set
            {
                status = value;
                OnStatusChange?.Invoke();
            }
        }

        internal bool IsInitializationFinished => status != ServiceStatus.NotInitialized;
        internal Action OnStatusChange;

        protected FrameworkValues frameworkValues;
        
        internal virtual void Initialize()
        {
            frameworkValues = Master.Resolve<FrameworkValues>();
        }
    }
}
