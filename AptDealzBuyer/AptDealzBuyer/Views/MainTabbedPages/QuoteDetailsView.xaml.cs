using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AptDealzBuyer.Utility;

namespace AptDealzBuyer.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuoteDetailsView : ContentView
    {
        public QuoteDetailsView()
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

        }

        private void FrmRevealContact_Tapped(object sender, EventArgs e)
        {

        }

        private void FrmAcceptQuote_Tapped(object sender, EventArgs e)
        {

        }

        private void FrmRejectQuote_Tapped(object sender, EventArgs e)
        {

        }

        private void FrmBackToQuotes_Tapped(object sender, EventArgs e)
        {

        }
    }
}