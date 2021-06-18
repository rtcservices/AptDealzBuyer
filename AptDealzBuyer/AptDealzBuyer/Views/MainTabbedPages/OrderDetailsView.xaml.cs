using AptDealzBuyer.Utility;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailsView : ContentView
    {
        public OrderDetailsView()
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

        private void FrmRaiseComplaint_Tapped(object sender, EventArgs e)
        {

        }

        private void FrmShowQrCode_Tapped(object sender, EventArgs e)
        {
            grdQrButton.IsVisible = false;
            grdQrCode.IsVisible = true;
        }
    }
}