using AptDealzBuyer.API;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Utility;
using Newtonsoft.Json.Linq;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AptDealzBuyer
{
    public partial class App : Application
    {
        public static double latitude = 0;
        public static double longitude = 0;
        public static List<Country> countries;

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

                GetCurrentLocation();
                BindCrossFirebasePushNotification();
                Application.Current.UserAppTheme = OSAppTheme.Light;

                BackgroundWorker backgroundWorker = new BackgroundWorker();
                backgroundWorker.DoWork += delegate
                  {
                      GetCountries();
                  };
                backgroundWorker.RunWorkerAsync();

                MainPage = new Views.SplashScreen.Spalshscreen();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("App/Constructor: " + ex.Message);
            }
        }

        async void GetCountries()
        {
            try
            {
                ProfileAPI profileAPI = new ProfileAPI();
                var mResponse = await profileAPI.GetCountry((int)App.Current.Resources["PageNumber"], (int)App.Current.Resources["PageSize"]);
                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    countries = result.ToObject<List<Country>>();
                }
                else
                {
                    Common.DisplayErrorMessage(mResponse.Message);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("App/GetCountries: " + ex.Message);
            }
        }

        public async void GetCurrentLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    latitude = location.Latitude;
                    longitude = location.Longitude;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("App/GetCurrentLocation: " + ex.Message);
            }
        }

        void BindCrossFirebasePushNotification()
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
    }
}
