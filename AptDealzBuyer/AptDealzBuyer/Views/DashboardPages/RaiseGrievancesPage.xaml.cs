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
    public partial class RaiseGrievancesPage : ContentPage
    {
        #region Objects
        // create objects here
        public List<RaiseGrievanceM> RaiseGrievanceMs = new List<RaiseGrievanceM>();
        #endregion

        #region Constructor
        public RaiseGrievancesPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        // write methods here
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindRaiseGrievance();
        }

        public void BindRaiseGrievance()
        {
            lstGrievance.ItemsSource = null;
            RaiseGrievanceMs = new List<RaiseGrievanceM>()
            {
                new RaiseGrievanceM{ GrNo="INV#123", GrPrice="Rs 2143", GrStatus="Shipped", GrDate="12-05-2021"},
                new RaiseGrievanceM{ GrNo="INV#456", GrPrice="Rs 2143", GrStatus="Accepted", GrDate="12-05-2021"},
                new RaiseGrievanceM{ GrNo="INV#789", GrPrice="Rs 2143", GrStatus="Completed", GrDate="12-05-2021"},
                new RaiseGrievanceM{ GrNo="INV#012", GrPrice="Rs 2143", GrStatus="Shipped", GrDate="12-05-2021"},
                new RaiseGrievanceM{ GrNo="INV#345", GrPrice="Rs 2143", GrStatus="Accepted", GrDate="12-05-2021"},
                new RaiseGrievanceM{ GrNo="INV#678", GrPrice="Rs 2143", GrStatus="Completed", GrDate="12-05-2021"},
                new RaiseGrievanceM{ GrNo="INV#901", GrPrice="Rs 2143", GrStatus="Shipped", GrDate="12-05-2021"},
                new RaiseGrievanceM{ GrNo="INV#234", GrPrice="Rs 2143", GrStatus="Accepted", GrDate="12-05-2021"},
            };
            lstGrievance.ItemsSource = RaiseGrievanceMs.ToList();
        }
        #endregion

        #region Events
        // create events here
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
            Navigation.PopAsync();
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
            var sortby = new PrevReqPopup("Status", "Open", "Close");
            sortby.isRefresh += (s1, e1) =>
            {
                //get result from popup
            };
            PopupNavigation.Instance.PushAsync(sortby);
        }

        private void FrmSelect_Tapped(object sender, EventArgs e)
        {

        }
        #endregion
    }
}