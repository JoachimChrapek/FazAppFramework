using System.Linq;
using FazAppFramework.Development;
using FazAppFramework.EventSystem;
using FazAppFramework.Utils;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

namespace FazAppFramework.Managers
{
    public class OnRewardPlayerEvent : GameEvent
    { }
    
    public class AdManager : Service
    {
        private const string RewardedVideoIDTest = "ca-app-pub-3940256099942544/5224354917";
        private const string InterstitalIDTest = "ca-app-pub-3940256099942544/1033173712";
        
        private InterstitialAd interstitial;
        private RewardedAd[] rewardedAds;
        
        internal override void Initialize()
        {
            base.Initialize();

            if (!frameworkValues.UseAdmob)
            {
                Status = ServiceStatus.Off;
                return;
            }
                
            this.Log("[ADMOB] Initializing Mobile ADS...", LogLevel.FrameworkInfo);

            MobileAds.Initialize(initStatus =>
            {
                this.Log("[ADMOB] Initialization completed.", LogLevel.FrameworkInfo);
                Status = ServiceStatus.Initialized;
                AfterAdmobInitialization();
            });
        }

        private void AfterAdmobInitialization()
        {
            PrepareInterstitial();
            rewardedAds = new RewardedAd[frameworkValues.PreloadedRewardedVideosCount];
            for (int i = 0; i < rewardedAds.Length; i++)
            {
                PrepareAndRequestRewardedAd(i);
            }
        }

        public bool CanShowInterstitial()
        {
            if (Status != ServiceStatus.Initialized)
                return false;

#if UNITY_EDITOR
            return true;
#endif
            return interstitial.IsLoaded();
        }

        public void ShowInterstitial()
        {
#if UNITY_EDITOR
            return;
#endif

            if(!CanShowInterstitial())
                return;

            interstitial.Show();
        }

        public bool CanShowRewardedAd()
        {
            if (Status != ServiceStatus.Initialized)
                return false;

#if UNITY_EDITOR
            return true;
#endif
            return rewardedAds.Any(r => r.IsLoaded());
        }

        public void ShowRewardedAd()
        {
#if UNITY_EDITOR
            RewardPlayer();
            return;
#endif

            if(!CanShowRewardedAd())
                return;

            rewardedAds.First(r => r.IsLoaded()).Show();
        }

        private void PrepareInterstitial()
        {
            interstitial = new InterstitialAd(frameworkValues.TestAds ? InterstitalIDTest :frameworkValues.InterstitialID);

            interstitial.OnAdFailedToLoad += HandleInterstitialFailedToLoad;

            RequestInterstitial();
        }

        private void RequestInterstitial()
        {
            var request = BuildAdRequest();
            interstitial.LoadAd(request);
        }

        private void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                this.Log("[ADMOB] Interstitial failed to load with message: " + args.Message, LogLevel.FrameworkErrorInfo);
                CoroutineHelper.InvokeWithDelay(frameworkValues.TimeAfterAdFailedToLoad, RequestInterstitial);
            });
        }

        private void PrepareAndRequestRewardedAd(int id)
        {
            var rewarded = new RewardedAd(frameworkValues.TestAds ? RewardedVideoIDTest : frameworkValues.RewardedVideoID);

            rewarded.OnAdFailedToLoad += (sender, arg) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    this.Log("[ADMOB] RewardedAd failed to load with message: " + arg.Message, LogLevel.FrameworkErrorInfo);
                    CoroutineHelper.InvokeWithDelay(frameworkValues.TimeAfterAdFailedToLoad, () => PrepareAndRequestRewardedAd(id));
                });
            };
            rewarded.OnUserEarnedReward += HandleRewaredAdUserEarnedReward;
            rewarded.OnAdClosed += (sender, arg) =>
            {
                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    PrepareAndRequestRewardedAd(id);
                });
            };

            var request = BuildAdRequest();
            rewarded.LoadAd(request);

            rewardedAds[id] = rewarded;
        }
        
        private void HandleRewaredAdUserEarnedReward(object sender, Reward args)
        {
            MobileAdsEventExecutor.ExecuteInUpdate(RewardPlayer);
        }

        private void RewardPlayer()
        {
            EventSystemManager.Publish<OnRewardPlayerEvent>(this);
        }
        
        private AdRequest BuildAdRequest()
        {
            var builder = new AdRequest.Builder();

            foreach (var testDevice in frameworkValues.TestDevices)
            {
                builder.AddTestDevice(testDevice);
            }

            var request = builder.Build();

            return request;
        }
    }
}
