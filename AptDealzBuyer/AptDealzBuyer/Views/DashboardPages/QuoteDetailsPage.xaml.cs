using AptDealzBuyer.Utility;
using Rg.Plugins.Popup.Services;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuoteDetailsPage : ContentPage
    {
        #region Objects
        // cretae objects here
        #endregion

        #region Constructor
        public QuoteDetailsPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        // write methosd here
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

        private void FrmRevealContact_Tapped(object sender, EventArgs e)
        {
            var contactPopup = new PopupPages.RevealContactPopup();
            contactPopup.isRefresh += (s1, e1) =>
              {
                  bool res = (bool)s1;
                  if (res)
                  {
                      //your logic
                  }
                  else
                  {
                      //your logic
                  }
              };
            PopupNavigation.Instance.PushAsync(contactPopup);
        }

        private void FrmAcceptQuote_Tapped(object sender, EventArgs e)
        {

        }

        private void FrmRejectQuote_Tapped(object sender, EventArgs e)
        {

        }

        private void FrmBackToQuotes_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        #endregion
    }
}