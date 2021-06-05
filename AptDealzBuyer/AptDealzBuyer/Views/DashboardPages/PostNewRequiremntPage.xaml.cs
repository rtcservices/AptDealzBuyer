using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using AptDealzBuyer.Utility;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostNewRequiremntPage : ContentPage
    {
        #region Objects
        // create objects here
        #endregion

        #region Constructor
        public PostNewRequiremntPage()
        {
            InitializeComponent();
            BindCategories();
        }
        #endregion

        #region Methods
        // write methods here
        public void BindCategories()
        {
            var categories = new List<string>()
                {
                    "Category A",
                    "Category B",
                    "Category C",
                };

            var subcategories = new List<string>()
                {
                    "SubCategory A",
                    "SubCategory B",
                    "SubCategory C",
                };

            pkCategory.ItemsSource = categories.ToList();
            pkSubCategory.ItemsSource = subcategories.ToList();
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

        private void ImgCatPicker_Tapped(object sender, EventArgs e)
        {
            pkCategory.Focus();
        }

        private void ImgSubCatPicker_Tapped(object sender, EventArgs e)
        {
            pkSubCategory.Focus();
        }

        private void StkPrefer_Tapped(object sender, EventArgs e)
        {
            if (imgPrefer.Source.ToString().Replace("File: ", "") == Constraints.CheckBox_Checked)
            {
                imgPrefer.Source = Constraints.CheckBox_UnChecked;
            }
            else
            {
                imgPrefer.Source = Constraints.CheckBox_Checked;
            }
        }

        private void StkPickup_Tapped(object sender, EventArgs e)
        {
            if (imgPickup.Source.ToString().Replace("File: ", "") == Constraints.CheckBox_Checked)
            {
                imgPickup.Source = Constraints.CheckBox_UnChecked;
            }
            else
            {
                imgPickup.Source = Constraints.CheckBox_Checked;
            }
        }

        private void StkShipping_Tapped(object sender, EventArgs e)
        {
            if (imgShippingDown.Source.ToString().Replace("File: ", "") == "iconDownArrow.png")
            {
                imgShippingDown.Source = "iconUpArrow.png";
                grdShippingAddress.IsVisible = true;
            }
            else
            {
                imgShippingDown.Source = "iconDownArrow.png";
                grdShippingAddress.IsVisible = false;
            }
        }

        private void StkInsCoverage_Tapped(object sender, EventArgs e)
        {
            if (imgInsCoverage.Source.ToString().Replace("File: ", "") == "iconCheck.png")
            {
                imgInsCoverage.Source = "iconUncheck.png";
            }
            else
            {
                imgInsCoverage.Source = "iconCheck.png";
            }
        }

        private void FrmSubmitRequirement_Tapped(object sender, EventArgs e)
        {
            var successPopup = new PopupPages.SuccessPopup();
            successPopup.isRefresh += (s1, e1) =>
            {
                bool res = (bool)s1;
                if (res)
                {
                    Navigation.PopAsync();
                }
            };

            PopupNavigation.Instance.PushAsync(successPopup);
        }

        private void ImgImageUpload_Tapped(object sender, EventArgs e)
        {

        }
        #endregion
    }
}