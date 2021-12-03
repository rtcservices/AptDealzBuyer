using AptDealzBuyer.API;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.DashboardPages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.OtherPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        #region [ Ctor ]
        public SettingsPage()
        {
            InitializeComponent();

            MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount); MessagingCenter.Subscribe<string>(this, Constraints.Str_NotificationCount, (count) =>
            {
                if (!Common.EmptyFiels(Common.NotificationCount))
                {
                    lblNotificationCount.Text = count;
                    frmNotification.IsVisible = true;
                }
                else
                {
                    frmNotification.IsVisible = false;
                    lblNotificationCount.Text = string.Empty;
                }
            });


            if (Common.mBuyerDetail.IsNotificationMute)
            {
                BtnMuteNotifications.Source = Constraints.Img_SwitchOn;
                lblmute.Text = "On";
            }
            else
            {
                BtnMuteNotifications.Source = Constraints.Img_SwitchOff;
                lblmute.Text = "Off";
            }
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

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
            return true;
        }
        #endregion

        #region [ Events ]
        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private async void ImgNotification_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new NotificationPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SettingsPage/ImgNotification_Tapped: " + ex.Message);
            }
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("FAQHelp"));
        }

        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            await Common.BindAnimation(imageButton: ImgBack);
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private void BtnLight_Clicked(object sender, EventArgs e)
        {
            Settings.IsDarkMode = false;
            Application.Current.UserAppTheme = OSAppTheme.Light;
        }

        private void BtnDark_Clicked(object sender, EventArgs e)
        {
            Settings.IsDarkMode = true;
            Application.Current.UserAppTheme = OSAppTheme.Dark;
        }

        private void pkAlertTone_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BtnAlertTone_Clicked(object sender, EventArgs e)
        {
            // pkAlertTone.Focus();
        }

        private async void BtnMuteNotifications_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (BtnMuteNotifications.Source.ToString().Replace("File: ", "") == Constraints.Img_SwitchOff)
                {
                    BtnMuteNotifications.Source = Constraints.Img_SwitchOn;
                    Settings.IsMuteMode = true;
                    lblmute.Text = "On";
                }
                else
                {
                    BtnMuteNotifications.Source = Constraints.Img_SwitchOff;
                    Settings.IsMuteMode = false;
                    lblmute.Text = "Off";
                }

                ProfileAPI profileAPI = new ProfileAPI();
                var mResponse = await profileAPI.UpdateUserMuteNotification(Settings.UserId, Settings.IsMuteMode);
                if (mResponse != null)
                {
                    if (!mResponse.Succeeded)
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplaySuccessMessage(mResponse.Message);
                }
                else
                {
                    Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SettingsPage/BtnMuteNotifications_Clicked: " + ex.Message);
            }
        }

        private void Picker_Unfocused(object sender, FocusEventArgs e)
        {

        }
        #endregion
    }
}