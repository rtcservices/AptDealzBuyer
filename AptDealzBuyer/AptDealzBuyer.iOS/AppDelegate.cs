using AptDealzBuyer.Interfaces;
using AptDealzBuyer.iOS.Service;
using DLToolkit.Forms.Controls;
using FFImageLoading.Forms.Platform;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace AptDealzBuyer.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            CachedImageRenderer.Init();
            FlowListView.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            Rg.Plugins.Popup.Popup.Init();
            CarouselView.FormsPlugin.iOS.CarouselViewRenderer.Init();
            Firebase.Core.App.Configure();
            //Xamarin.Essentials.Platform.Init();

            LoadApplication(new App());
            DependencyService.Register<IFirebaseAuthenticator, FirebaseAuthenticator>();

            return base.FinishedLaunching(app, options);
        }
    }
}
