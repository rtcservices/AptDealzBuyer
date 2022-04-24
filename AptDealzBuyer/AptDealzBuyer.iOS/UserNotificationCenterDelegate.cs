using System;
using AptDealzBuyer.Utility;
using UserNotifications;
using Xamarin.Forms;

namespace AptDealzBuyer.iOS
{
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        public UserNotificationCenterDelegate()
        {
        }

        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            try
            {
                // Do something with the notification
                // Console.WriteLine("Active Notification: {0}", notification);
              //  MessagingCenter.Send<string>(string.Empty, Constraints.NotificationReceived);
                // Tell system to display the notification anyway or use
                // `None` to say we have handled the display locally.
                completionHandler(UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Badge);

            } catch
            {

            }
        }
    }
}