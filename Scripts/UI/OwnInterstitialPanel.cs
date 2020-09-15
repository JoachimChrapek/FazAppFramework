//using FazAppFramework.Managers;
//using UnityEngine;
//using UnityEngine.UI;

//namespace FazAppFramework.UI
//{
//    public class OwnInterstitialPanel : MonoBehaviour
//    {
//        public Image iconImage;
//        public Text titleText;
//        public Text descriptionText;
//        public Button installButton;

//        public void Show()
//        {
//            if (!MainManager.Instance.ownInterstitial.showInterstitial)
//            {
//                return;
//            }

//            FirebaseManager.LogEvent("show_own_interstitial");

//            var data = MainManager.Instance.ownInterstitial.data;
//            iconImage.sprite = MainManager.Instance.ownInterstitial.interstitialTexture;
//            titleText.text = data.title;
//            descriptionText.text = data.description;

//            installButton.onClick.RemoveAllListeners();
//            installButton.onClick.AddListener(delegate
//            {
//                Application.OpenURL(data.storeURL);
//                CloseOwnInterstitial();
//            });

//            gameObject.SetActive(true);
//        }

//        public void CloseOwnInterstitial()
//        {
//            gameObject.SetActive(false);
//        }
//    }
//}
