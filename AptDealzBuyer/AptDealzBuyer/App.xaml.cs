using AptDealzBuyer.Repository;
using AptDealzBuyer.Services;
using AptDealzBuyer.Utility;
using Plugin.FirebasePushNotification;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AptDealzBuyer
{
    public partial class App : Application
    {
        #region Objects
        public static int latitude = 0;
        public static int longitude = 0;
        #endregion

        #region Constructor
        public App()
        {
            try
            {
                Device.SetFlags(new string[]
                {
                    "MediaElement_Experimental",
                    "AppTheme_Experimental"
                });

                InitializeComponent();
                Application.Current.UserAppTheme = OSAppTheme.Light;

                RegisterDependencies();
                GetCurrentLocation();
                BindCrossFirebasePushNotification();

                MainPage = new Views.SplashScreen.Spalshscreen();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("App/Constructor: " + ex.Message);
            }
        }
        #endregion

        #region Methods
        public static void RegisterDependencies()
        {
            Xamarin.Forms.DependencyService.Register<IFileUploadRepository, FileUploadRepository>();
            Xamarin.Forms.DependencyService.Register<IDeleteRepository, DeleteRepository>();
            Xamarin.Forms.DependencyService.Register<ICategoryRepository, CategoryRepository>();
            Xamarin.Forms.DependencyService.Register<IProfileRepository, ProfileRepository>();
            Xamarin.Forms.DependencyService.Register<IAuthenticationRepository, AuthenticationRepository>();
            Xamarin.Forms.DependencyService.Register<IOrderRepository, OrderRepository>();
        }

        public async void GetCurrentLocation()
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
                    };

                CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
                {
                    System.Diagnostics.Debug.WriteLine("Received");
                };

                CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
                {
                    System.Diagnostics.Debug.WriteLine("Opened");
                    foreach (var data in p.Data)
                    {
                        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                    }
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
                };

                CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
                {
                    System.Diagnostics.Debug.WriteLine("Deleted");
                };
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("App/BindCrossFirebasePushNotification: " + ex.Message);
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        #endregion
    }
}
