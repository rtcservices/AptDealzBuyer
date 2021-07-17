using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using AptDealzBuyer.Constants;
using AptDealzBuyer.Droid.DependencService;
using AptDealzBuyer.Interfaces;
using AptDealzBuyer.Model;
using AptDealzBuyer.Utility;
using Com.Razorpay;
using DLToolkit.Forms.Controls;
using FFImageLoading.Forms.Platform;
using Firebase;
using Newtonsoft.Json;
using Org.Json;
using Plugin.FirebasePushNotification;
using Plugin.Permissions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AptDealzBuyer.Droid
{
    [Activity(Label = "AptDealzBuyer", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IPaymentResultWithDataListener
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
            Xamarin.Forms.DependencyService.Register<IFirebaseAuthenticator, FirebaseAuthenticator>();

            CachedImageRenderer.Init(true);
            FlowListView.Init();
            UserDialogs.Init(this);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            Rg.Plugins.Popup.Popup.Init(this);

            LoadApplication(new App());

            MessagingCenter.Subscribe<RazorPayload>(this, "PayNow", (payload) =>
            {
                string username = "rzp_test_co1QTfvqLJyWXn";
                string password = "iAhjtNtHYHrQOQPE09X5XBGC";
                PayViaRazor(payload, username, password);
            });
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

        public void OnPaymentError(int p0, string p1, PaymentData p2)
        {
            RazorResponse mRazorResponse = new RazorResponse()
            {
                PaymentId = p2.PaymentId,
                OrderId = p2.OrderId,
                Signature = p2.Signature,
                isPaid = false
            };

            MessagingCenter.Send<RazorResponse>(mRazorResponse, "PaidResponse");
        }

        public void OnPaymentSuccess(string p0, PaymentData p1)
        {
            RazorResponse mRazorResponse = new RazorResponse()
            {
                PaymentId = p1.PaymentId,
                OrderId = p1.OrderId,
                Signature = p1.Signature,
                isPaid = true
            };
            MessagingCenter.Send<RazorResponse>(mRazorResponse, "PaidResponse");
        }


        public async void PayViaRazor(RazorPayload payload, string username, string password)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.razorpay.com/v1/orders"))
                    {
                        var plainTextBytes = Encoding.UTF8.GetBytes($"{username}:{password}");
                        var basicAuthKey = Convert.ToBase64String(plainTextBytes);

                        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {basicAuthKey}");

                        string jsonData = JsonConvert.SerializeObject(payload);
                        request.Content = new StringContent(jsonData);
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                        var response = await httpClient.SendAsync(request);
                        string jsonResp = await response.Content.ReadAsStringAsync();
                        RazorResponse mRazorResponse = JsonConvert.DeserializeObject<RazorResponse>(jsonResp);

                        if (!string.IsNullOrEmpty(mRazorResponse.id))
                        {
                            CheckoutRazorPay(mRazorResponse.id, username, payload);
                        }
                        else
                        {
                            var mOrderRequest = JsonConvert.DeserializeObject<OrderRequest>(jsonResp);
                            if (mOrderRequest != null && mOrderRequest.error != null)
                                Toast.MakeText(this, mOrderRequest.error.description, ToastLength.Short).Show();
                            else
                                Toast.MakeText(this, "Payment Error", ToastLength.Short).Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void CheckoutRazorPay(string orderId, string username, RazorPayload payload)
        {
            // checkout
            try
            {
                Checkout checkout = new Checkout();
                checkout.SetImage(0);
                checkout.SetKeyID(username);

                JSONObject options = new JSONObject();

                options.Put("name", "AptDealz"); //Merchant Name
                options.Put("description", $"Reference No. {payload.receipt}");
                options.Put("image", "https://s3.amazonaws.com/rzp-mobile/images/rzp.png");
                options.Put("order_id", orderId);//from response of step 3.
                options.Put("theme.color", "#006027");
                options.Put("currency", payload.currency);
                options.Put("amount", payload.amount);//pass amount in currency subunits 2000 
                options.Put("prefill.email", "test@yopmail.com");
                options.Put("prefill.contact", "1234567890");

                checkout.Open(this, options);
            }
            catch (Exception e)
            {
                // Log.e(TAG, "Error in starting Razorpay Checkout", e);
            }
        }
    }

    public class Metadata
    {
    }

    public class Error
    {
        public string code { get; set; }
        public string description { get; set; }
        public string source { get; set; }
        public string step { get; set; }
        public string reason { get; set; }
        public Metadata metadata { get; set; }
        public string field { get; set; }
    }

    public class OrderRequest
    {
        public Error error { get; set; }
    }
}