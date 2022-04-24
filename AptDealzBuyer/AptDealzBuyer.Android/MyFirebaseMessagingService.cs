using Android.App;
using Android.Content;
using Android.OS;
using AptDealzBuyer.Droid.DependencService;
using AptDealzBuyer.Utility;
using Firebase.Messaging;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AptDealzBuyer.Droid
{
    [Service(Exported = true, Name = "com.zartek.quotesouk.bidder.MyFirebaseMessagingService")]
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
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (!Utility.Settings.IsMuteMode)
                    {
                        NotificationHelper notificationHelper = new NotificationHelper();
                        notificationHelper.ScheduleNotification(message.GetNotification().Title, message.GetNotification().Body);
                    }
                    MessagingCenter.Send<string>(string.Empty, Constraints.NotificationReceived);
                });
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MyFirebaseMessagingService/OnMessageReceived: " + ex.Message);
            }
        }
    }
}