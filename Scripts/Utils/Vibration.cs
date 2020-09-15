using UnityEngine;

namespace FazAppFramework.Utils
{
    //Class from this thread: https://www.reddit.com/r/Unity3D/comments/4j5js7/unity_vibrate_android_device_for_custom_duration/
    public class Vibration
    {
#if UNITY_ANDROID && !UNITY_EDITOR
    private static readonly AndroidJavaObject Vibrator =
        new AndroidJavaClass("com.unity3d.player.UnityPlayer")// Get the Unity Player.
        .GetStatic<AndroidJavaObject>("currentActivity")// Get the Current Activity from the Unity Player.
        .Call<AndroidJavaObject>("getSystemService", "vibrator");// Then get the Vibration Service from the Current Activity.
#endif

        static Vibration()
        {
            // Trick Unity into giving the App vibration permission when it builds.
            // This check will always be false, but the compiler doesn't know that.
            if (Application.isEditor) Handheld.Vibrate();
        }

        public static void Vibrate(long milliseconds)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        Vibrator.Call("vibrate", milliseconds);
#endif
        }

        public static void Vibrate(long[] pattern, int repeat)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        Vibrator.Call("vibrate", pattern, repeat);
#endif
        }
    }
}
