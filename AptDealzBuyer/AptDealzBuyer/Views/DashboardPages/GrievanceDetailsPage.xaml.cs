using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.MasterData;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GrievanceDetailsPage : ContentPage
    {
        #region Objects
        // create objects here
        #endregion

        #region Constructor
        public GrievanceDetailsPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        // wrire methods gere
        #endregion

        #region Events
        // create events here
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
            Common.BindAnimation(image: ImgBack);
            Navigation.PopAsync();
        }

        private void FrmSubmit_Tapped(object sender, EventArgs e)
        {

        }
        #endregion
    }
}