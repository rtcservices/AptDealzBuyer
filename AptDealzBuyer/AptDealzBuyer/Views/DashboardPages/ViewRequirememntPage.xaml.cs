using AptDealzBuyer.Model;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.PopupPages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewRequirememntPage : ContentPage
    {
        #region Objects
        // create objects here
        public List<ReceivedQuote> ReceivedQuotes = new List<ReceivedQuote>();
        string ReqType = string.Empty;
        #endregion

        #region Constructor
        public ViewRequirememntPage(string ReqType)
        {
            InitializeComponent();
            this.ReqType = ReqType;
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ReqType == "active")
            {
                BindReceivedQuote();
                grdPrevReq.IsVisible = false;
                grdActiveReq.IsVisible = true;
            }
            else
            {
                grdActiveReq.IsVisible = false;
                grdPrevReq.IsVisible = true;
            }
        }

        public void BindReceivedQuote()
        {
            lstQoutes.ItemsSource = null;
            ReceivedQuotes = new List<ReceivedQuote>()
            {
                new ReceivedQuote{ QuoteNo="QUO#123", QuotePrice="Rs 2143", Validity="Validity : 5 days"},
                new ReceivedQuote{ QuoteNo="QUO#456", QuotePrice="Rs 2143", Validity="Validity : 4 days"},
                new ReceivedQuote{ QuoteNo="QUO#789", QuotePrice="Rs 2143", Validity="Validity : 2 days"},
                new ReceivedQuote{ QuoteNo="QUO#012", QuotePrice="Rs 2143", Validity="Validity : 6 days"},
                new ReceivedQuote{ QuoteNo="QUO#345", QuotePrice="Rs 2143", Validity="Validity : 3 days"},
                new ReceivedQuote{ QuoteNo="QUO#678", QuotePrice="Rs 2143", Validity="Validity : 7 days"},
                new ReceivedQuote{ QuoteNo="QUO#901", QuotePrice="Rs 2143", Validity="Validity : 9 days"},
                new ReceivedQuote{ QuoteNo="QUO#234", QuotePrice="Rs 2143", Validity="Validity : 8 days"},
            };
            lstQoutes.ItemsSource = ReceivedQuotes.ToList();
            lstQoutes.HeightRequest = 4 * 100;
        }
        #endregion

        #region Events
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

        private void FrmCancelRequirement_Tapped(object sender, EventArgs e)
        {

        }

        private void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            var sortby = new SortByPopup("ID", "Amount", "Validity");
            sortby.isRefresh += (s1, e1) =>
            {
                //get result from popup
            };
            PopupNavigation.Instance.PushAsync(sortby);
        }

        private void GrdViewQuote_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DashboardPages.QuoteDetailsPage());
        }
        #endregion
    }
}