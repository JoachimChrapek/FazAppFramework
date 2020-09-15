using System.Collections;
using System.Collections.Generic;
using FazAppFramework.Development;
using FazAppFramework.DI;
using FazAppFramework.Utils;
using Firebase.RemoteConfig;
using UnityEngine;
using UnityEngine.Networking;

namespace FazAppFramework.Managers
{
    public struct OwnInterstitialData
    {
        public bool showOwnInterstitial;
        public string imageURL;
        public string storeURL;
        public string title;
        public string description;
    }
    
    public class FirebaseOwnInterstitial : Service
    {
        private OwnInterstitialData data;
        private Sprite interstitialSprite;
        
        private FirebaseManager firebaseManager;
        
        public bool CanShowOwnInterstitial()
        {
            return Status == ServiceStatus.Initialized;
        }

        public void ShowInterstitial()
        {
            firebaseManager.LogEvent("show_own_interstitial");
            //TODO show logic
        }

        internal override void Initialize()
        {
            base.Initialize();

            if (!frameworkValues.UseOwnIntertital || !frameworkValues.UseFirebase)
            {
                Status = ServiceStatus.Off;
                return;
            }

            firebaseManager = Master.Resolve<FirebaseManager>();
        }

        public void PrepareInterstitialData()
        {
            data = GetOwnInterstitialData();

            if (!data.showOwnInterstitial)
            {
                Status = ServiceStatus.Off;
                return;
            }

            CoroutineHelper.StartCoroutineHelperFunction(LoadImageFromUrl());
        }
        
        // Own interstitial depends on Firebase. If Firebase fails OwnInterstitial cannot be loaded.
        public void OnFirebaseInitializationFailed()
        {
            Status = ServiceStatus.FailedToInitialize;
        }

        public Dictionary<string, object> GetInterstitialDefaultKeys()
        {
            var defaults = new Dictionary<string, object>
            {
                {frameworkValues.FIREBASE_REMOTE_CONFIG_SHOW_OWN_INTERSTITIAL_KEY_B, false},
                {frameworkValues.FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_IMAGE_URL_KEY_S, "DEFAULT"},
                {frameworkValues.FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_STORE_URL_KEY_S, "DEFAULT"},
                {frameworkValues.FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_TITLE_KEY_S, "DEFAULT"},
                {frameworkValues.FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_DESCRIPTION_KEY_S, "DEFAULT"}
            };

            return defaults;
        }

        private OwnInterstitialData GetOwnInterstitialData()
        {
            var fetchedData = new OwnInterstitialData
            {
                showOwnInterstitial = FirebaseRemoteConfig.GetValue(frameworkValues.FIREBASE_REMOTE_CONFIG_SHOW_OWN_INTERSTITIAL_KEY_B).BooleanValue,
                imageURL = FirebaseRemoteConfig.GetValue(frameworkValues.FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_IMAGE_URL_KEY_S).StringValue,
                storeURL = FirebaseRemoteConfig.GetValue(frameworkValues.FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_STORE_URL_KEY_S).StringValue,
                title = FirebaseRemoteConfig.GetValue(frameworkValues.FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_TITLE_KEY_S).StringValue,
                description = FirebaseRemoteConfig.GetValue(frameworkValues.FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_DESCRIPTION_KEY_S).StringValue
            };

            return fetchedData;
        }

        private IEnumerator LoadImageFromUrl()
        {
            var www = UnityWebRequestTexture.GetTexture(data.imageURL);
            yield return www.SendWebRequest();

            if (www.isHttpError || www.isNetworkError)
            {
                Status = ServiceStatus.FailedToInitialize;
                this.Log("[OWN INTERSTITIAL] Image not loaded. \n" + www.error, LogLevel.FrameworkErrorInfo);
                firebaseManager.LogEvent("own_interstitial_image_load_status", "status", "failure");
            }
            else
            {
                var tex = ((DownloadHandlerTexture)www.downloadHandler).texture;
                interstitialSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
                Status = ServiceStatus.Initialized;
                this.Log("[OWN INTERSTITIAL] Image loaded.", LogLevel.FrameworkInfo);
                firebaseManager.LogEvent("own_interstitial_image_load_status", "status", "success");
            }
        }
    }
}