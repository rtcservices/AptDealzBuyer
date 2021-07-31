using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace AptDealzBuyer.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage
    {
        #region Objects
        private List<NotificationData> mNotificationsList;
        #endregion

        #region Constructor
        public NotificationPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                GetNotification();
            }
            catch (Exception ex)
            {

            }
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            try
            {
                Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/OnBackButtonPressed: " + ex.Message);
            }
            return true;
        }

        private async Task GetNotification()
        {
            NotificationAPI notificationAPI = new NotificationAPI();
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await notificationAPI.GetAllNotificationsForUser();
                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    if (result != null)
                    {
                        mNotificationsList = result.ToObject<List<NotificationData>>();
                        if (mNotificationsList != null && mNotificationsList.Count > 0)
                        {
                            lstNotification.IsVisible = true;
                            lblNoRecord.IsVisible = false;
                            lstNotification.ItemsSource = mNotificationsList.ToList();
                        }
                        else
                        {
                            lstNotification.IsVisible = false;
                            lblNoRecord.IsVisible = true;
                        }
                    }
                }
                else
                {
                    lblNoRecord.IsVisible = true;
                    if (mResponse != null && !Common.EmptyFiels(mResponse.Message))
                        lblNoRecord.Text = mResponse.Message;
                    else
                        lblNoRecord.Text = Constraints.Something_Wrong;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("NotificationPage/GetNotification: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async Task SetNoficiationAsRead(string NotificationId)
        {
            try
            {
                var isReded = await DependencyService.Get<INotificationRepository>().SetUserNoficiationAsRead(NotificationId);
                if (isReded)
                {
                    mNotificationsList.Clear();
                    await GetNotification();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("NotificationPage/GetNotification: " + ex.Message);
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
            Common.BindAnimation(imageButton: ImgBack);
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private async void ImgClose_Tapped(object sender, EventArgs e)
        {
            try
            {
                var ImageButtonExp = (ImageButton)sender;
                var notificationData = ImageButtonExp.BindingContext as NotificationData;
                await SetNoficiationAsRead(notificationData.NotificationId);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("NotificationPage/ImgClose_Tapped: " + ex.Message);
            }
        }

        private void lstNotification_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstNotification.SelectedItem = null;
        }

        private async void lstNotification_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lstNotification.IsRefreshing = true;
                mNotificationsList.Clear();
                await GetNotification();
                lstNotification.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("NotificationPage/lstNotification_Refreshing: " + ex.Message);
            }
        }
        #endregion

        private void GrdList_Tapped(object sender, EventArgs e)
        {
            try
            {
                var GridExp = (Grid)sender;
                var mNotification = GridExp.BindingContext as NotificationData;
                if (mNotification.NavigationScreen == (int)NavigationScreen.RequirementDetails)
                {
                    Navigation.PushAsync(new DashboardPages.ViewRequirememntPage("active", mNotification.ParentKeyId));
                }
                else if (mNotification.NavigationScreen == (int)NavigationScreen.QuoteDetails)
                {
                    Navigation.PushAsync(new QuoteDetailsPage(mNotification.ParentKeyId));
                }
                else if (mNotification.NavigationScreen == (int)NavigationScreen.OrderDetails)
                {
                    Navigation.PushAsync(new Orders.OrderDetailsPage(mNotification.ParentKeyId));
                }
                else if (mNotification.NavigationScreen == (int)NavigationScreen.GrievanceDetails)
                {
                    Navigation.PushAsync(new GrievanceDetailsPage(mNotification.ParentKeyId));
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("NotificationPage/GrdList_Tapped: " + ex.Message);
            }
        }
    }
}