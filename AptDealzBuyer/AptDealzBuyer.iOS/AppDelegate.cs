﻿using AptDealzBuyer.Interfaces;
using AptDealzBuyer.iOS.Service;
using DLToolkit.Forms.Controls;
using FFImageLoading.Forms.Platform;
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
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
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
                Firebase.Core.App.Configure();

                Plugin.LocalNotification.NotificationCenter.AskPermission();

                LoadApplication(new App());

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

                FirebasePushNotificationManager.Initialize(options, true);
                DependencyService.Register<IFirebaseAuthenticator, FirebaseAuthenticator>();

                // Added by BK 10-14-2021
                FirebasePushNotificationManager.CurrentNotificationPresentationOption = UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Badge;
                UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

                return base.FinishedLaunching(app, options);
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Exception-FinishedLaunching", ex.Message, "Ok");
                throw;
            }
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
