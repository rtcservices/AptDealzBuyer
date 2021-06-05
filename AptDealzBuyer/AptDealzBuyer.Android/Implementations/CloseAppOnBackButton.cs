using Android.App;
using AptDealzBuyer.Interfaces;
using System;

namespace AptDealzBuyer.Droid.Implementations
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class CloseAppOnBackButton : ICloseAppOnBackButton, IDisposable
    {
        public void CloseApp()
        {
            try
            {
                var activity = (Activity)Xamarin.Forms.Forms.Context;
                activity.FinishAffinity();
            }
            catch (Exception)
            {
            }
        }

        public void Dispose()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}