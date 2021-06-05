using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using AptDealzBuyer.Constants;
using AptDealzBuyer.Droid.DependencService;
using AptDealzBuyer.Utility;
using DLToolkit.Forms.Controls;
using Firebase;
using Plugin.FirebasePushNotification;
using Plugin.Permissions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AptDealzBuyer.Droid
{
    [Activity(Label = "AptDealzBuyer", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;

            FirebaseApp.InitializeApp(this);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            FirebasePushNotificationManager.ProcessIntent(this, Intent);
            CreateNotificationFromIntent(Intent);

            FlowListView.Init();
            UserDialogs.Init(this);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            Rg.Plugins.Popup.Popup.Init(this);

            LoadApplication(new App());
            //FirebasePushNotificationManager.ProcessIntent(this, Intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        }


        protected override void OnNewIntent(Intent intent)
        {
            FirebasePushNotificationManager.ProcessIntent(this, intent);
            CreateNotificationFromIntent(intent);
        }

        void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                var isEnable = Preferences.Get(AppKeys.Notification, true);
                if (isEnable)
                {
                    string title = intent.Extras.GetString(NotificationHelper.TitleKey);
                    string message = intent.Extras.GetString(NotificationHelper.MessageKey);
                    DependencyService.Get<INotificationHelper>().ReceiveNotification(title, message);
                }
            }
        }
    }
}