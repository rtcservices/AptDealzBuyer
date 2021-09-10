using AptDealzBuyer.Interfaces;
using AptDealzBuyer.Model;
using AptDealzBuyer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.SplashScreen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class WelcomePage : ContentPage
    {
        #region [ Objecst ]      
        public List<CarousellImage> mCarousellImages = new List<CarousellImage>();
        #endregion

        #region [ Constructor ]
        public WelcomePage()
        {
            InitializeComponent();
            Settings.IsViewWelcomeScreen = false;
        }
        #endregion

        #region [ Methods ]  
        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Dispose();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindCarousallData();
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            try
            {
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await DisplayAlert(Constraints.Alert, Constraints.DoYouWantToExit, Constraints.Yes, Constraints.No);
                        if (result)
                        {
                            Xamarin.Forms.DependencyService.Get<ICloseAppOnBackButton>().CloseApp();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("WelcomePage/OnBackButtonPressed: " + ex.Message);
            }
            return true;
        }

        private void BindCarousallData()
        {
            try
            {
                mCarousellImages = new List<CarousellImage>()
            {
                new CarousellImage{ImageName=Constraints.Img_WelcomeOne},
                new CarousellImage{ImageName=Constraints.Img_WelcomeTwo},
                new CarousellImage{ImageName=Constraints.Img_WelcomeThree},
            };
                Indicators.ItemsSource = cvWelcome.ItemsSource = mCarousellImages.ToList();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("WelcomePage/BindCarousallData: " + ex.Message);
            }
        }
        #endregion

        #region [ Events ]  
        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnLogin);
                    await Navigation.PushAsync(new Login.LoginPage());
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("WelcomePage/BtnLogin_Clicked: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }
        #endregion
    }
}