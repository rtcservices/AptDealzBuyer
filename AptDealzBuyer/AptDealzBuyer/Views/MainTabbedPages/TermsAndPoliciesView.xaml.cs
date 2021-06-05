using AptDealzBuyer.Views.MasterData;
using System;
using Xamarin.Forms;
using AptDealzBuyer.Utility;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermsAndPoliciesView : ContentView
    {
        #region Objects
        // create objects here
        public event EventHandler isRefresh;
        #endregion

        public TermsAndPoliciesView()
        {
            InitializeComponent();
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

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            //Common.BindAnimation(image: ImgBack);
            App.Current.MainPage = new MasterDataPage();
        }
    }
}