using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using AptDealzBuyer.Utility;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaiseGrievancePage : ContentPage
    {
        #region Objects
        // create objects here
        #endregion

        #region Constructor
        public RaiseGrievancePage()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindType();
        }

        public void BindType()
        {
            var types = new List<string>()
            {
                "Complaint A",
                "Complaint B",
                "Complaint C"
            };
            pkType.ItemsSource = types.ToList();
        }
        #endregion

        #region MyRegion
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

        private void ImgType_Tapped(object sender, EventArgs e)
        {
            pkType.Focus();
        }

        private void FrmSubmit_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgImageUpload_Tapped(object sender, EventArgs e)
        {

        }
        #endregion
    }
}