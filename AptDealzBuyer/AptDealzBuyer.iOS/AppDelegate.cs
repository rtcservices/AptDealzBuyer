using DLToolkit.Forms.Controls;
using Foundation;
using UIKit;

namespace AptDealzBuyer.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            FlowListView.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            Rg.Plugins.Popup.Popup.Init();
            CarouselView.FormsPlugin.iOS.CarouselViewRenderer.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
