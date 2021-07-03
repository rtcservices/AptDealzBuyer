using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using AptDealzBuyer.Utility;
using Xamarin.Forms.Xaml;
using AptDealzBuyer.Repository;

namespace AptDealzBuyer.Views.Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaiseGrievancePage : ContentPage
    {
        #region Objects
        private string relativePath = string.Empty;
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
            Navigation.PopAsync();
        }

        private void ImgType_Tapped(object sender, EventArgs e)
        {
            pkType.Focus();
        }

        private void FrmSubmit_Tapped(object sender, EventArgs e)
        {

        }

        private async void UploadProductImage_Tapped(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(image: ImgUplode);
                ImageConvertion.SelectedImagePath = ImgProductImage;
                ImageConvertion.SetNullSource((int)FileUploadCategory.ProfileDocuments);
                await ImageConvertion.SelectImage();
                //relativePath = await DependencyService.Get<IFileUploadRepository>().UploadFile((int)FileUploadCategory.ProfileDocuments);

            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RaiseGrievancePage/UploadProductImage: " + ex.Message);
            }
        }
        #endregion
    }
}