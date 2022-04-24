using AptDealzBuyer.Interfaces;
using AptDealzBuyer.iOS.Service;
using AptDealzBuyer.Utility;
using DLToolkit.Forms.Controls;
using FFImageLoading.Forms.Platform;
using Firebase.CloudMessaging;
using Foundation;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using UIKit;
using UserNotifications;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AptDealzBuyer.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            try
            {
                global::Xamarin.Forms.Forms.Init();

                CachedImageRenderer.Init();
                FlowListView.Init();
                ZXing.Net.Mobile.Forms.iOS.Platform.Init();
                Rg.Plugins.Popup.Popup.Init();
                CarouselView.FormsPlugin.iOS.CarouselViewRenderer.Init();

                Plugin.LocalNotification.NotificationCenter.AskPermission();
                LoadApplication(new App());

                RegisterForRemoteNotifications();

               // Messaging.SharedInstance.Delegate = this;
                if (UNUserNotificationCenter.Current != null)
                {
                    UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();
                }

                Firebase.Core.App.Configure();
                FirebasePushNotificationManager.Initialize(options, new NotificationUserCategory[]
                {
                     new NotificationUserCategory("message",new List<NotificationUserAction>
                     {
                         new NotificationUserAction("Reply","Reply",NotificationActionType.Foreground)
                     }),
                     new NotificationUserCategory("request",new List<NotificationUserAction>
                     {
                         new NotificationUserAction("Accept","Accept"),
                         new NotificationUserAction("Reject","Reject",NotificationActionType.Destructive)
                     })
                });

                //FirebasePushNotificationManager.Initialize(options, true);
                DependencyService.Register<IFirebaseAuthenticator, FirebaseAuthenticator>();

                // Added by BK 10-14-2021
                FirebasePushNotificationManager.CurrentNotificationPresentationOption = UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Badge;
                //UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

                return base.FinishedLaunching(app, options);
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Exception-FinishedLaunching", ex.Message, "Ok");
                throw;
            }
        }

        /// <summary>
        /// Code added by Jino on 24/04/2022
        /// </summary>
        private void RegisterForRemoteNotifications()
        {

            // Register your app for remote notifications.

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {

                // For iOS 10 display notification (sent via APNS)

                UNUserNotificationCenter.Current.Delegate = this;

                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;

                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, async (granted, error) =>

                {
                    Console.WriteLine($"Permission  {granted}");
                    await System.Threading.Tasks.Task.Delay(500);
                });

            }

            else
            {

                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;

                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);

                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();

        }
        /// <summary>
        /// Code Added By BK 10-13-2021
        /// </summary>
        /// <param name="application"></param>
        /// <param name="deviceToken"></param>
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {

            try
            {
                if (Messaging.SharedInstance != null)
                    Messaging.SharedInstance.ApnsToken = deviceToken;
                Firebase.Auth.Auth.DefaultInstance.SetApnsToken(deviceToken, Firebase.Auth.AuthApnsTokenType.Production); // Production if you are ready to release your app, otherwise, use Sandbox.
                FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Exception-RegisteredForRemoteNotifications", ex.Message, "Ok");
            }
        }

        /// <summary>
        /// Code Added By BK 10-13-2021
        /// </summary>
        /// <param name="application"></param>
        /// <param name="error"></param>
        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            try
            {
                FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Exception-FailedToRegisterForRemoteNotifications", ex.Message, "Ok");
            }
        }
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return false;
            try
            {
                var openUrlOptions = new UIApplicationOpenUrlOptions(options);
                return OpenUrl(app, url, openUrlOptions.SourceApplication, openUrlOptions.Annotation);
            }
            catch
            {
                if (Firebase.Auth.Auth.DefaultInstance.CanHandleUrl(url))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Code Added By BK 10-13-2021
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userInfo"></param>
        /// <param name="completionHandler"></param>
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            try
            {
                if (Firebase.Auth.Auth.DefaultInstance.CanHandleNotification(userInfo))
                {
                    completionHandler(UIBackgroundFetchResult.NoData);
                    return;
                }

                //FirebasePushNotificationManager.DidReceiveMessage(userInfo);
                //completionHandler(UIBackgroundFetchResult.NewData);
                //ProcessNotification(userInfo);
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Exception-DidReceiveRemoteNotification", ex.Message, "Ok");
            }
        }

        [Export("messaging:didReceiveRegistrationToken")]

        public void DidReceiveRegistrationToken(string fcmToken)
        {
            Settings.fcm_token = fcmToken;
                Xamarin.Forms.Application.Current.Properties["Fcmtocken"] = Messaging.SharedInstance.FcmToken ?? "";
                Xamarin.Forms.Application.Current.SavePropertiesAsync();
            System.Diagnostics.Debug.WriteLine($"######Token######  :  {fcmToken}");
            Console.WriteLine(fcmToken);
        }
        /// <summary>
        /// Code Added By BK 10-14-2021
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userInfo"></param>
        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            if (userInfo == null)
                return;

            ProcessNotification(userInfo);
        }

        /// <summary>
        /// Code Added By BK 10-14-2021
        /// </summary>
        /// <param name="options"></param>
        void ProcessNotification(NSDictionary options)
        {
            try
            {
                if (options != null && options.ContainsKey(new NSString("aps")))
                {
                    string body = string.Empty;
                    string title = AppInfo.Name;
                    body = (options[new NSString("Message")] as NSString).ToString();

                    if (!string.IsNullOrEmpty(body))
                    {
                        //  App.PushNotificationForiOS(title, body);
                    }
                }
            }
            catch (System.Exception ex)
            {
                if (!ex.Message.ToLower().Contains("object reference"))
                    App.Current.MainPage.DisplayAlert("ProcessNotification", ex.Message, "Ok");
            }
        }

        /// <summary>
        /// Code Added By BK 10-14-2021
        /// </summary>
        /// <param name="uiApplication"></param>
        public override void WillEnterForeground(UIApplication uiApplication)
        {
            Plugin.LocalNotification.NotificationCenter.ResetApplicationIconBadgeNumber(uiApplication);
        }

    }
}
