using AptDealzBuyer.Utility;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowImagePopup : PopupPage
    {
        #region [ Objects ]      
        public event EventHandler isRefresh;
        string ImageUrl = string.Empty;
        #endregion

        #region [ Constructor ]
        public ShowImagePopup(string imageUrl)
        {
            InitializeComponent();
            ImageUrl = imageUrl;

        }
        #endregion
      
        #region [ Method ]
        protected override bool OnBackgroundClicked()
        {
            base.OnBackgroundClicked();
            return false;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ImgProduct.Source = ImageUrl;
        }
        #endregion

        #region [ Event ]
        private void ImgClose_Clicked(object sender, EventArgs e)
        {
            try
            {
                isRefresh?.Invoke(true, EventArgs.Empty);
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ShowImagePopup/ImgClose_Clicked: " + ex.Message);
            }
        } 
        #endregion
    }
}