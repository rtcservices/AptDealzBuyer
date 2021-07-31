using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AptDealzBuyer.Utility;

namespace AptDealzBuyer.Views.OtherPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        #region Ctor
        public SettingsPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<string>(this, "NotificationCount", (count) =>
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
            Navigation.PushAsync(new DashboardPages.NotificationPage());
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Navigation.PopAsync();
        }

        private void ImgLanguage_Tapped(object sender, EventArgs e)
        {
            pkLanguage.Focus();
        }

        private void ImgAlertTone_Tapped(object sender, EventArgs e)
        {
            pkAlertTone.Focus();
        }

        private void ImgSwitch_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (imgSwitch.Source.ToString().Replace("File: ", "") == Constraints.Switch_Off)
                {
                    imgSwitch.Source = Constraints.Switch_On;
                }
                else
                {
                    imgSwitch.Source = Constraints.Switch_Off;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SettingsPage/ImgSwitch_Tapped: " + ex.Message);
            }
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
        #endregion
    }
}