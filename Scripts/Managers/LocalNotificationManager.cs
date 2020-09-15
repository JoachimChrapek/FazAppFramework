using System;
using FazAppFramework.Development;
using Unity.Notifications.Android;

namespace FazAppFramework.Managers
{
    public enum LocalNotificationType
    {
        N24h = 24,
        N48h = 48,
        N72h = 72,
        NRepeatable96h = 96
    }

    [Serializable]
    public class LocalNotificationData
    {
        public LocalNotificationType type;
        public string title;
        public string message;
        public string callback;
        public string smallIconId;
        public string bigIconId;
    }
    
    internal class LocalNotificationManager
    {
        private const string ChannelId = "main_channel";

        public void SendFirstLaunchNotifications(FrameworkValues frameworkValues)
        {
            try
            {
                MainLogger.Log(typeof(LocalNotificationManager).Name, "[LOCAL NOTIFICATIONS]: Sending first launch notifications.", LogLevel.FrameworkInfo);

                var channel = new AndroidNotificationChannel()
                {
                    Id = ChannelId,
                    Name = "Default Channel",
                    Importance = Importance.High,
                    Description = "Generic notifications",
                    EnableLights = true,
                    EnableVibration = true,

                };
                AndroidNotificationCenter.RegisterNotificationChannel(channel);

                AndroidNotification notification;
                foreach (var data in frameworkValues.NotificationsData)
                {
                    notification = new AndroidNotification
                    {
                        Title = data.title,
                        Text = data.message,
                        FireTime = DateTime.Now.AddHours((int)data.type),
                        SmallIcon = data.smallIconId,
                        LargeIcon = data.bigIconId,
                        IntentData = data.callback,
                    };

                    if (data.type == LocalNotificationType.NRepeatable96h)
                    {
                        notification.RepeatInterval = TimeSpan.FromHours(24);
                    }

                    AndroidNotificationCenter.SendNotification(notification, ChannelId);
                }
            }
            catch (Exception e)
            {
                MainLogger.Log(typeof(LocalNotificationManager).Name, $"[LOCAL NOTIFICATIONS]: Failed to send first launch notifications: {e}", LogLevel.FrameworkErrorInfo);
            }
        }

        public bool GetNotificationCallback(out string callback)
        {
            var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();

            if (notificationIntentData != null)
            {
                callback = notificationIntentData.Notification.IntentData;
                return true;
            }

            callback = String.Empty;
            return false;
        }
    }
}