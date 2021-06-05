using Android.App;
using AptDealzBuyer.Droid.DependencService;
using AptDealzBuyer.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(CloseAppOnBackButton))]

namespace AptDealzBuyer.Droid.DependencService
{
    public class CloseAppOnBackButton : ICloseAppOnBackButton
    {
        public void CloseApp()
        {
            try
            {
#pragma warning disable CS0618 // Type or member is obsolete
                var activity = (Activity)Forms.Context;
#pragma warning restore CS0618 // Type or member is obsolete
                activity.FinishAffinity();
            }
            catch (System.Exception)
            {
            }
        }
    }
}