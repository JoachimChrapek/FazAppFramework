using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using FazAppFramework.Managers;
using UnityEngine;

namespace FazAppFramework
{
    [Serializable]
    public class FrameworkValues
    {
        public string MORE_GAMES_LINK = "https://play.google.com/store/apps/developer?id=FazApp";
        public float SERVICE_WAITING_TIME = 5f;

        public bool UseAdmob = true;
        public bool TestAds = true;
        public string AdmobAppID = "ca-app-pub-3940256099942544~3347511713";
        public string InterstitialID = "";
        public string RewardedVideoID = "";
        public int PreloadedRewardedVideosCount = 2;
        public float TimeAfterAdFailedToLoad = 10f;

        public List<string> TestDevices = new List<string>();

        public bool UseFirebase = true;
        public bool UseOwnIntertital = true;
        public string FIREBASE_REMOTE_CONFIG_SHOW_OWN_INTERSTITIAL_KEY_B = "show_own_interstitial";
        public string FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_IMAGE_URL_KEY_S = "own_interstitial_image_url";
        public string FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_STORE_URL_KEY_S = "own_interstitial_store_url";
        public string FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_TITLE_KEY_S = "own_interstitial_title";
        public string FIREBASE_REMOTE_CONFIG_OWN_INTERSTITIAL_DESCRIPTION_KEY_S = "own_interstitial_description";
        
        public bool UseLocalNotifications = true;
        public LocalNotificationData[] NotificationsData;
    }

    internal static class FrameworkValuesHelper
    {
        public static FrameworkValues GetFrameworkValues()
        {
            if (!File.Exists(FrameworkConstantValues.FrameworkValuesFilePath))
            {
                var exceptionMessage = "FAZAPP FRAMEWORK - FrameworkValues.json (in Assets folder) is missing! Can't start application";
                throw new Exception(exceptionMessage);
            }

            try
            {
                var serialized = File.ReadAllText(FrameworkConstantValues.FrameworkValuesFilePath);
            
                var values = JsonUtility.FromJson<FrameworkValues>(serialized);

                if (values == null)
                {
                    var exceptionMessage = "FAZAPP FRAMEWORK - Deserialized FrameworkValues is null! Can't start application";
                    throw new Exception(exceptionMessage);
                }

                return values;
            }
            catch (Exception e)
            {
                throw new Exception($"Exception occured during loading and registering FrameworkValues: {e}");
            }
        }
    }
}
