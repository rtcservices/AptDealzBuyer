using AptDealzBuyer.API;
using AptDealzBuyer.Interfaces;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.DashboardPages;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.OtherPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        Dictionary<int, string> mRingtones = new Dictionary<int, string>();

        #region [ Ctor ]
        public SettingsPage()
        {
            try
            {
                InitializeComponent();

                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    DependencyService.Get<IOpenWriteSettings>().GrantWriteSettings();
                }

                MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                MessagingCenter.Subscribe<string>(this, Constraints.Str_NotificationCount, (count) =>
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


                if (Common.mBuyerDetail != null && Common.mBuyerDetail.IsNotificationMute)
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
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SettingsPage/Ctor: " + ex.Message);
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                mRingtones = Xamarin.Forms.DependencyService.Get<IRingtoneManager>().GetRingtones();
                if (mRingtones != null && mRingtones.Count > 0)
                {
                    pkAlertTone.ItemsSource = mRingtones.Select(x => x.Value).OrderBy(x => x).ToList();
                    if (!Common.EmptyFiels(Settings.NotificationToneName))
                    {
                        pkAlertTone.SelectedItem = Settings.NotificationToneName;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SettingsPage/OnAppearing: " + ex.Message);
            }
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
                await Navigation.PushAsync(new DashboardPages.NotificationPage("SettingsPage"));
                //await Navigation.PushAsync(new DashboardPages.NotificationPage());
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
            try
            {
                if (!Common.EmptyFiels(pkAlertTone.SelectedItem as string))
                {
                    var tone = pkAlertTone.SelectedItem as string;
                    var selectedToneId = mRingtones.Where(x => x.Value == tone).FirstOrDefault().Key;

                    Xamarin.Forms.DependencyService.Get<IRingtoneManager>().PlayRingTone(selectedToneId);
                    Xamarin.Forms.DependencyService.Get<IRingtoneManager>().SaveRingTone(selectedToneId);

                    if (!Common.EmptyFiels(Settings.NotificationToneName))
                    {
                        pkAlertTone.SelectedItem = Settings.NotificationToneName;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SettingsPage/Picker_Unfocused: " + ex.Message);
            }
        }

        private void BtnAlertTone_Clicked(object sender, EventArgs e)
        {
            pkAlertTone.Focus();
        }
        #endregion
    }
}