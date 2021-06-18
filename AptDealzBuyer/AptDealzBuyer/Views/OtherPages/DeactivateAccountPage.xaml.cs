using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.SplashScreen;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.OtherPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeactivateAccountPage : ContentPage
    {
        #region Objects

        #endregion

        #region Constructor
        public DeactivateAccountPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        async void DeactivateAccount()
        {
            try
            {
                var result = await DisplayAlert(Constraints.Alert, Constraints.AreYouSureWantDeactivateAccount, Constraints.Yes, Constraints.No);
                if (result)
                {
                    ProfileAPI profileAPI = new ProfileAPI();
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    var mResponse = await profileAPI.DeactiviateUser(Settings.UserId);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                    }
                    else
                    {
                        if (mResponse != null)
                            Common.DisplayErrorMessage(mResponse.Message);
                        else
                            Common.DisplayErrorMessage(Constraints.Something_Wrong);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("DeactivateAccountPage/DeactivateAccount: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion

        #region Events
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(image: ImgMenu);
            //Common.OpenMenu();
        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void FrmConfirmDeactivation_Tapped(object sender, EventArgs e)
        {
            DeactivateAccount();
        }
        #endregion
    }
}