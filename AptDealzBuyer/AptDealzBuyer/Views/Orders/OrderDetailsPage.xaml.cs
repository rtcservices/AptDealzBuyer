using System;
using Xamarin.Forms;
using AptDealzBuyer.Utility;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailsPage : ContentPage
    {
        public OrderDetailsPage()
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
            Navigation.PopAsync();
        }

        private void FrmCancelOrder_Tapped(object sender, EventArgs e)
        {

        }

        private void FrmTrackOrder_Tapped(object sender, EventArgs e)
        {

        }

        private void FrmRaiseGrievance_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Orders.RaiseGrievancePage());
        }
    }
}