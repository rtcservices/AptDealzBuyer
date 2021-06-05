using AptDealzBuyer.Model;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.MasterData;
using AptDealzBuyer.Views.PopupPages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderView : ContentView
    {
        #region Objects
        // create objects here
        public event EventHandler isRefresh;
        public List<OrderM> OrderMs = new List<OrderM>();
        #endregion

        #region Constructor
        public OrderView()
        {
            InitializeComponent();
            BindOrders();
        }
        #endregion

        #region Methods
        // write methods here
        public void BindOrders()
        {
            lstOrders.ItemsSource = null;
            OrderMs = new List<OrderM>()
            {
                new OrderM{ OrdNo="INV#123", OrdPrice="Rs 2143", OrdStatus="Shipped", OrdDate="12-05-2021"},
                new OrderM{ OrdNo="INV#456", OrdPrice="Rs 2143", OrdStatus="Accepted", OrdDate="12-05-2021"},
                new OrderM{ OrdNo="INV#789", OrdPrice="Rs 2143", OrdStatus="Completed", OrdDate="12-05-2021"},
                new OrderM{ OrdNo="INV#012", OrdPrice="Rs 2143", OrdStatus="Ready for pickup", OrdDate="12-05-2021"},
                new OrderM{ OrdNo="INV#345", OrdPrice="Rs 2143", OrdStatus="Shipped", OrdDate="12-05-2021"},
                new OrderM{ OrdNo="INV#678", OrdPrice="Rs 2143", OrdStatus="Accepted", OrdDate="12-05-2021"},
                new OrderM{ OrdNo="INV#901", OrdPrice="Rs 2143", OrdStatus="Completed", OrdDate="12-05-2021"},
                new OrderM{ OrdNo="INV#234", OrdPrice="Rs 2143", OrdStatus="Shipped", OrdDate="12-05-2021"},
            };
            lstOrders.ItemsSource = OrderMs.ToList();
        }
        #endregion

        #region Events
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(image: ImgMenu);
            try
            {
                if (Common.MasterData != null)
                {
                    Common.MasterData.IsGestureEnabled = true;
                    Common.MasterData.IsPresented = true;
                }
            }
            catch (Exception ex)
            {
                //Common.DisplayErrorMessage("HomeView/ImgMenu_Tapped: " + ex.Message);
            }
        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(image: ImgBack);
            App.Current.MainPage = new MasterDataPage();
        }

        private void ImgSearch_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new PopupPages.SearchPopup());
        }

        private void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            var sortby = new PrevReqPopup("Sort By", "ID", "Quotes");
            sortby.isRefresh += (s1, e1) =>
            {
                //get result from popup
            };
            PopupNavigation.Instance.PushAsync(sortby);
        }

        private void FrmStatus_Tapped(object sender, EventArgs e)
        {
            var sortby = new PrevReqPopup("Status", "All", "Date");
            sortby.isRefresh += (s1, e1) =>
            {
                //get result from popup
            };
            PopupNavigation.Instance.PushAsync(sortby);
        }

        private void GrdViewOrderDetails_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Orders.OrderDetailsPage());
        }

        private void FrmStatusActions_Tapped(object sender, EventArgs e)
        {

        }
        #endregion
    }
}