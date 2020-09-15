//using UnityEngine;

//namespace FazAppFramework.UI
//{
//    public class RateUsPanel : MonoBehaviour
//    {
//        /// <summary>
//        /// Button event for Rate Us
//        /// </summary>
//        /// <param name="answer">1 for Yes, 0 for Later, -1 for No</param>
//        public void OnRateUsAnswer(int answer)
//        {
//            if (answer == 1 || answer == -1)
//            {
//                PlayerPrefs.SetInt(FrameworkConstantValues.RATE_US_DONE_KEY, 1);
//            }

//            if (answer == 1)
//            {
//                Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
//            }

//            //TODO close RateUs panel
//        }
//    }
//}
