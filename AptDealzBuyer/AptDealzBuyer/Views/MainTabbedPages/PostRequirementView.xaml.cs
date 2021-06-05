using AptDealzBuyer.Utility;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostRequirementView : ContentView
    {
        #region Object
        // create objects here
        public event EventHandler isRefresh;
        #endregion

        #region Constructor
        public PostRequirementView()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
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
            //isRefresh?.Invoke(true, EventArgs.Empty);
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

        private void ImgImageUpload_Tapped(object sender, EventArgs e)
        {

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

        private void FrmSubmitRequirement_Tapped(object sender, EventArgs e)
        {

        }
        #endregion
    }
}