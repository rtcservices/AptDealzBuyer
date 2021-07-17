using AptDealzBuyer.Model;
using AptDealzBuyer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage
    {
        #region Objects
        // create objects here
        public List<Model.Notification> Notifications = new List<Model.Notification>();
        #endregion

        #region Constructor
        public NotificationPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        // write methods here
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindNotification();
        }

        public void BindNotification()
        {
            lstNotification.ItemsSource = null;
            Notifications = new List<Model.Notification>()
            {
                new Model.Notification{ NotificationTitle="New quote received for REQ#123", NotificationDesc=""},
                new Model.Notification{ NotificationTitle="New quote received for REQ#121", NotificationDesc=""},
                new Model.Notification{ NotificationTitle="New response received for your grienvance GR#01", NotificationDesc=""},
                new Model.Notification{ NotificationTitle="Free Requirements post limit reached. Make payment to post further requirements.", NotificationDesc=""},
                new Model.Notification{ NotificationTitle="Your order for INV#121 has been shipped.", NotificationDesc=""},
                new Model.Notification{ NotificationTitle="New response received for your grienvance GR#03", NotificationDesc=""},
                new Model.Notification{ NotificationTitle="Your order for INV#123 has been shipped.", NotificationDesc=""},
            };
            lstNotification.ItemsSource = Notifications.ToList();
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
            Navigation.PopAsync();
        }

        private void ImgClose_Tapped(object sender, EventArgs e)
        {

        }

        private void lstNotification_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstNotification.SelectedItem = null;
        }
        #endregion

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
    }
}