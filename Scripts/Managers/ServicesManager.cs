using System;
using System.Linq;
using FazAppFramework.DI;

namespace FazAppFramework.Managers
{
    internal class ServicesManager
    {
        private Service[] servicesToInitialize;

        private Action afterServicesInitializationAction;

        internal void InitializeServices(Action afterServicesInitialization)
        {
            this.afterServicesInitializationAction = afterServicesInitialization;

            var firebaseManager = Master.Resolve<FirebaseManager>();
            var adManager = Master.Resolve<AdManager>();
            var ownInterstitial = Master.Resolve<FirebaseOwnInterstitial>();
            
            servicesToInitialize = new Service[]
            {
                ownInterstitial,
                firebaseManager,
                adManager
            };

            foreach (var service in servicesToInitialize)
            {
                service.OnStatusChange += OnServiceStatusChange;
                service.Initialize();
            }
        }

        private void OnServiceStatusChange()
        {
            if(servicesToInitialize.Any(s => !s.IsInitializationFinished))
                return;

            OnInitializationComplete();
        }

        private void OnInitializationComplete()
        {
            foreach (var service in servicesToInitialize)
            {
                service.OnStatusChange -= OnServiceStatusChange;
            }

            afterServicesInitializationAction?.Invoke();
        }
    }
}
