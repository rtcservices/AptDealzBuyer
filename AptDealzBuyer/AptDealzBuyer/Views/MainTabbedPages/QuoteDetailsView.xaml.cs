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