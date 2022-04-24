using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;
using AptDealzBuyer.Droid.DependencService;
using AptDealzBuyer.Utility;
using System;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationHelper))]
namespace AptDealzBuyer.Droid.DependencService
{
    public class NotificationHelper : INotificationHelper
    {
        #region [ Properties ]
        const string channelId = "fcm_fallback_notification_channel";
        const string channelName = "Miscellaneous";

        const int pendingIntentId = 0;

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized;
        int messageId = -1;
        NotificationManager manager;

        public event EventHandler NotificationReceived;
        #endregion

        /// <summary>
        /// Initializes Required Properties for Notification in Android.
        /// </summary>
        /// <returns></returns>
        public void Initialize()
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }
        }

        /// <summary>
        /// Schedules Notification to be sent.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public int ScheduleNotification(string title, string message)
        {
            try
            {

                if (Utility.Settings.IsMuteMode)
                {
                    return 0;
                }

                if (!channelInitialized)
                {
                    CreateNotificationChannel();
                }

                messageId++;

                Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
                intent.PutExtra(TitleKey, title);
                intent.PutExtra(MessageKey, message);

                PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId, intent, PendingIntentFlags.OneShot);

                NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                    .SetContentIntent(pendingIntent)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetSmallIcon(Resource.Drawable.icon);

                var notificationsound = MainActivity.DefaultNotificationSoundURI;
                if (notificationsound != null)
                {
                    builder.SetSound(notificationsound);
                }

                var notification = builder.Build();
                manager.Notify(messageId, notification);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("NotificationHelper/ScheduleNotification: " + ex.Message);
            }

            return messageId;
        }

        /// <summary>
        /// Initialized NotificationEventArgs and invokes Notification Received.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
        }

        /// <summary>
        /// Creates Notification Channel for Android.
        /// </summary>
        public void CreateNotificationChannel(NotificationChannel chan = null)
        {
            try
            {
                manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {

                    if (chan != null)
                    {
                        manager.CreateNotificationChannel(chan);
                    }
                    else
                    {
                        var channelNameJava = new Java.Lang.String(channelName);
                        var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.High);
                        channel.EnableVibration(true);
                        channel.LockscreenVisibility = NotificationVisibility.Public;

                        var notificationsound = MainActivity.DefaultNotificationSoundURI;
                        if (notificationsound != null)
                        {
                            var alarmAttributes = new AudioAttributes.Builder()
                                                       .SetContentType(AudioContentType.Sonification)
                                                       .SetUsage(AudioUsageKind.NotificationRingtone).Build();

                            channel.SetSound(notificationsound, alarmAttributes);
                        }
                        manager.CreateNotificationChannel(channel);
                    }
                }

                channelInitialized = true;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("NotificationHelper/CreateNotificationChannel: " + ex.Message);
            }
        }
    }
}