using AptDealzBuyer.Repository;
using AptDealzBuyer.Services;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.MasterData;
using Plugin.FirebasePushNotification;
using Plugin.LocalNotification;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AptDealzBuyer
{
    public partial class App : Application
    {
        #region [ Objects ]
        public static int latitude = 0;
        public static int longitude = 0;
        public static StoppableTimer stoppableTimer;
        public static StoppableTimer chatStoppableTimer;
        #endregion 

        #region [ Constructor ]
        public App()
        {
            try
            {
                Device.SetFlags(new string[]
                {
                    "MediaElement_Experimental",
                    "AppTheme_Experimental",
                    "FastRenderers_Experimental",
                    "CollectionView_Experimental"
                });

                InitializeComponent();

                if (Settings.IsDarkMode)
                {
                    Application.Current.UserAppTheme = OSAppTheme.Dark;
                }
                else
                {
                    Application.Current.UserAppTheme = OSAppTheme.Light;
                }

                var mainPage = new Views.SplashScreen.Spalshscreen();
                RegisterDependencies();
                //GetCurrentLocation();
                BindCrossFirebasePushNotification();

                MainPage = mainPage;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("App/Constructor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]
        public static void RegisterDependencies()
        {
            Xamarin.Forms.DependencyService.Register<IAuthenticationRepository, AuthenticationRepository>();
            Xamarin.Forms.DependencyService.Register<IFileUploadRepository, FileUploadRepository>();
            Xamarin.Forms.DependencyService.Register<IProfileRepository, ProfileRepository>();
            Xamarin.Forms.DependencyService.Register<IRequirementRepository, RequirementRepository>();
            Xamarin.Forms.DependencyService.Register<IQuoteRepository, QuoteRepository>();
            Xamarin.Forms.DependencyService.Register<IOrderRepository, OrderRepository>();
            Xamarin.Forms.DependencyService.Register<IGrievanceRepository, GrievanceRepository>();
            Xamarin.Forms.DependencyService.Register<INotificationRepository, NotificationRepository>();
        }

        private async void GetCurrentLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    latitude = (int)location.Latitude;
                    longitude = (int)location.Longitude;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("App/GetCurrentLocation: " + ex.Message);
            }
        }

        private void BindCrossFirebasePushNotification()
        {
            try
            {
                CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
                {
                    System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
                    if (DeviceInfo.Platform == DevicePlatform.iOS && !Common.EmptyFiels(p.Token))
                    {
                        Utility.Settings.fcm_token = p.Token;
                    }
                };

                CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
                {
                    System.Diagnostics.Debug.WriteLine("Received");
                    MessagingCenter.Send<string>(string.Empty, Constraints.NotificationReceived);
                    //if (Settings.IsNotification)
                    //{
                    //    if (Common.mBuyerDetail != null && !Common.EmptyFiels(Common.mBuyerDetail.BuyerId) && !Common.EmptyFiels(Common.Token))
                    //    {
                    //        MainPage = new MasterDataPage();
                    //    }
                    //    else
                    //    {
                    //   MainPage = new Views.SplashScreen.Spalshscreen();
                    //    }
                    //}
                    //App.Current.MainPage.DisplayAlert("Alert3", "App > OnNotificationReceived" + Settings.IsNotification, "ok");
                };

                CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
                {
                    Settings.IsNotification = true;
                    if (Settings.IsNotification)
                    {
                        if (Common.mBuyerDetail != null && !Common.EmptyFiels(Common.mBuyerDetail.BuyerId) && !Common.EmptyFiels(Common.Token))
                        {
                            MainPage = new MasterDataPage();
                        }
                        else
                        {
                            MainPage = new Views.SplashScreen.Spalshscreen();
                        }
                    }
                    Settings.IsNotification = false;
                    //App.Current.MainPage.DisplayAlert("Alert4", "App > OnNotificationOpened" + Settings.IsNotification, "ok");
                };

                CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
                {
                    System.Diagnostics.Debug.WriteLine("Action");

                    if (!string.IsNullOrEmpty(p.Identifier))
                    {
                        System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                        foreach (var data in p.Data)
                        {
                            System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                        }
                    }
                    //App.Current.MainPage.DisplayAlert("Alert5", "App > OnNotificationAction" + Settings.IsNotification, "ok");
                };

                CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
                {
                    System.Diagnostics.Debug.WriteLine("Deleted");
                    //App.Current.MainPage.DisplayAlert("Alert6", "App > OnNotificationDeleted" + Settings.IsNotification, "ok");
                };
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("App/BindCrossFirebasePushNotification: " + ex.Message);
            }
        }

        public static void PushNotificationForiOS(string title, string message)
        {
            try
            {
                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    var notification = new NotificationRequest
                    {
                        NotificationId = 100,
                        Title = title,
                        Description = message,
                        BadgeNumber = 1,
                    };
                    NotificationCenter.Current.Show(notification);
                }
            }
            catch (System.Exception ex)
            {
                Common.DisplayErrorMessage("App/PushNotificationForiOS: " + ex.Message);
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            if (App.stoppableTimer != null)
                stoppableTimer.Stop();
        }

        protected override void OnResume()
        {
            if (App.stoppableTimer != null)
                stoppableTimer.Start();
        }
        #endregion
    }
}
