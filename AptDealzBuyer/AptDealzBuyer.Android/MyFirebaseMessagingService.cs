using Android.App;
using Android.Content;
using AptDealzBuyer.Droid.DependencService;
using AptDealzBuyer.Utility;
using Firebase.Messaging;

namespace AptDealzBuyer.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        public MyFirebaseMessagingService()
        {

        }
        public override void OnMessageReceived(RemoteMessage message)
        {
            try
            {
                base.OnMessageReceived(message);
                if (!Utility.Settings.IsMuteMode)
                {
                    NotificationHelper notificationHelper = new NotificationHelper();
                    notificationHelper.ScheduleNotification(message.GetNotification().Title, message.GetNotification().Body);
                }
            }
            catch (System.Exception ex)
            {
                Common.DisplayErrorMessage("MyFirebaseMessagingService/OnMessageReceived: " + ex.Message);
            }
        }
    }
}