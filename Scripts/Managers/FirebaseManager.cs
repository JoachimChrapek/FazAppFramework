using System;
using System.Threading.Tasks;
using FazAppFramework.Development;
using FazAppFramework.DI;
using Firebase;
using Firebase.Analytics;
using Firebase.RemoteConfig;
using LogLevel = FazAppFramework.Development.LogLevel;

namespace FazAppFramework.Managers
{
    public class FirebaseManager : Service
    {
        private FirebaseOwnInterstitial ownInterstitial;

        internal override void Initialize()
        {
            if (!frameworkValues.UseFirebase)
            {
                Status = ServiceStatus.Off;
                return;
            }

            ownInterstitial = Master.Resolve<FirebaseOwnInterstitial>();

            this.Log("[FIREBASE] Initializing Firebase...", LogLevel.FrameworkInfo);

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

                    Firebase.Messaging.FirebaseMessaging.TokenRegistrationOnInitEnabled = false;
                    Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
                    Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;

                    this.Log("[FIREBASE] Initialization completed.", LogLevel.FrameworkInfo);
                    Status = ServiceStatus.Initialized;

                    LogEvent("firebase_started");

                    if (frameworkValues.UseOwnIntertital)
                    {
                        InitializeRemoteConfig();
                        FetchDataAsync();
                    }
                }
                else
                {
                    this.Log("[FIREBASE] Initialization failed. DependencyStatus: " + dependencyStatus, LogLevel.FrameworkErrorInfo);
                    Status = ServiceStatus.FailedToInitialize;
                    ownInterstitial.OnFirebaseInitializationFailed();
                }
            });
        }

        public void LogEvent(string name)
        {
            if(Status != ServiceStatus.Initialized)
                return;

            FirebaseAnalytics.LogEvent(name);
        }

        public void LogEvent(string name, string parameterName, int parameterValue)
        {
            if(Status != ServiceStatus.Initialized)
                return;

            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }

        public void LogEvent(string name, string parameterName, double parameterValue)
        {
            if(Status != ServiceStatus.Initialized)
                return;

            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }

        public void LogEvent(string name, string parameterName, string parameterValue)
        {
            if(Status != ServiceStatus.Initialized)
                return;

            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }

        private void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
        {
            this.Log("[FIREBASE] Received Registration Token: " + token.Token, LogLevel.FrameworkInfo);
        }

        private void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
        {
            this.Log("[FIREBASE] Received a new message from: " + e.Message.From, LogLevel.FrameworkInfo);
        }
        
        private void InitializeRemoteConfig()
        {
            FirebaseRemoteConfig.SetDefaults(ownInterstitial.GetInterstitialDefaultKeys());
        }

        private Task FetchDataAsync()
        {
            this.Log("[FIREBASE] Fetching own interstitial data...", LogLevel.FrameworkInfo);

            Task fetchTask = FirebaseRemoteConfig.FetchAsync(TimeSpan.Zero);
            return fetchTask.ContinueWith(FetchComplete);
        }

        private void FetchComplete(Task fetchTask)
        {
            if (fetchTask.IsCanceled)
            {
                this.Log("[FIREBASE] Own interstitial data fetch canceled.", LogLevel.FrameworkErrorInfo);
            }
            else if (fetchTask.IsFaulted)
            {
                this.Log("[FIREBASE] Own interstitial data fetch encountered an error.", LogLevel.FrameworkErrorInfo);
            }
            else if (fetchTask.IsCompleted)
            {
                this.Log("[FIREBASE] Own interstitial data fetch completed successfully!", LogLevel.FrameworkInfo);
            }

            var info = FirebaseRemoteConfig.Info;
            switch (info.LastFetchStatus)
            {
                case LastFetchStatus.Success:
                    FirebaseRemoteConfig.ActivateFetched();
                    this.Log($"[FIREBASE] Own interstitial data loaded and ready (last fetch time {info.FetchTime}).", LogLevel.FrameworkInfo);

                    LogEvent("own_interstitial_load_status", "status", "success");
                    ownInterstitial.PrepareInterstitialData();
                    break;
                case LastFetchStatus.Failure:
                    LogEvent("own_interstitial_load_status", "status", "failure");
                    ownInterstitial.OnFirebaseInitializationFailed();

                    switch (info.LastFetchFailureReason)
                    {
                        case FetchFailureReason.Error:
                            this.Log("[FIREBASE] Own interstitial data fetch failed for unknown reason.", LogLevel.FrameworkErrorInfo);
                            break;
                        case FetchFailureReason.Throttled:
                            this.Log("[FIREBASE] Own interstitial data fetch throttled until " +
                                             info.ThrottledEndTime, LogLevel.FrameworkErrorInfo);
                            break;
                    }

                    break;
                case LastFetchStatus.Pending:
                    this.Log("[FIREBASE] Latest own interstitial data fetch call still pending.", LogLevel.FrameworkInfo);
                    break;
            }
        }
    }
}