using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FazAppFramework.Development;
using FazAppFramework.DI;
using FazAppFramework.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FazAppFramework.Managers
{
    internal class InitializationModule
    {
        public DependencyRegisterModule DependencyRegisterModule { get; }

        public List<Type> TypesToAddToGlobalManager { get; }
        public List<GameObject> GameObjectsToAddToGlobalManager { get; }

        private FrameworkValues frameworkValues;
        private Action afterInitialization;
        private ServicesManager servicesManager;
        private bool initialized;


        public InitializationModule()
        {
            DependencyRegisterModule = new DependencyRegisterModule();

            TypesToAddToGlobalManager = new List<Type>();
            GameObjectsToAddToGlobalManager = new List<GameObject>();
        }

        public async Task InitializeFramework(Action afterInit)
        {
            afterInitialization = afterInit;

            this.Log("Starting...", LogLevel.FrameworkInfo);
            
            Master.Initialize(DependencyRegisterModule);
            frameworkValues = Master.Resolve<FrameworkValues>();
            
            this.Log("Set target frame rate based on Device, for now - 60FPS", LogLevel.ToDo);
            Application.targetFrameRate = 60;

            PrepareGlobalManager();
            
            var sessions = PlayerPrefs.GetInt(FrameworkConstantValues.SESSION_COUNTER_KEY, 0);
            PlayerPrefs.SetInt(FrameworkConstantValues.SESSION_COUNTER_KEY, sessions + 1);
            
            if (sessions == 0 && frameworkValues.UseLocalNotifications)
            {
                Master.Resolve<LocalNotificationManager>().SendFirstLaunchNotifications(frameworkValues);
            }

            servicesManager = new ServicesManager();
            servicesManager.InitializeServices(OnInitializationComplete);

            await Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(frameworkValues.SERVICE_WAITING_TIME));
                OnInitializationComplete();
            });
        }

        private void PrepareGlobalManager()
        {
            var globalManager = new GameObject("GLOBAL MANAGER");
            Object.DontDestroyOnLoad(globalManager);

            globalManager.AddComponent<CoroutineHelper>();

            foreach (var type in TypesToAddToGlobalManager)
            {
                globalManager.AddComponent(type);
            }

            foreach (var gameObject in GameObjectsToAddToGlobalManager)
            {
                gameObject.transform.SetParent(globalManager.transform);
            }
        }
        
        private void OnInitializationComplete()
        {
            if(initialized)
                return;

            initialized = true;
            
            afterInitialization?.Invoke();
        }
        
    }
}
