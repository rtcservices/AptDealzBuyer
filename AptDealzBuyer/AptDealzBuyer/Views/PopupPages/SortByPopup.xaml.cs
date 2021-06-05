using AptDealzBuyer.Utility;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SortByPopup : PopupPage
    {
        #region Objects
        // create objects here
        public event EventHandler isRefresh;
        string firstType = string.Empty;
        string secondType = string.Empty;
        string ThirdType = string.Empty;
        #endregion

        #region Constructor
        public SortByPopup(string firstType, string secondType, string ThirdType)
        {
            InitializeComponent();
            this.firstType = firstType;
            this.secondType = secondType;
            this.ThirdType = ThirdType;
        }
        #endregion

        #region Methos
        // write methods here
        protected override void OnAppearing()
        {
            base.OnAppearing();

            lblFirstType.Text = firstType;
            lblSecondType.Text = secondType;
            lblThirdType.Text = ThirdType;

        }
        #endregion

        #region Events
        // create events here
        private void StkFirstType_Tapped(object sender, EventArgs e)
        {
            imgSecondType.Source = Constraints.Redio_UnSelected;
            imgThirdType.Source = Constraints.Redio_UnSelected;
            imgFirstType.Source = Constraints.Radio_Selected;
            isRefresh?.Invoke(firstType, EventArgs.Empty);
            PopupNavigation.Instance.PopAsync();
        }

        private void StkSecondType_Tapped(object sender, EventArgs e)
        {
            imgFirstType.Source = Constraints.Redio_UnSelected;
            imgThirdType.Source = Constraints.Redio_UnSelected;
            imgSecondType.Source = Constraints.Radio_Selected;
            isRefresh?.Invoke(secondType, EventArgs.Empty);
            PopupNavigation.Instance.PopAsync();
        }

        private void StkThirdType_Tapped(object sender, EventArgs e)
        {
            imgFirstType.Source = Constraints.Redio_UnSelected;
            imgSecondType.Source = Constraints.Redio_UnSelected;
            imgThirdType.Source = Constraints.Radio_Selected;
            isRefresh?.Invoke(ThirdType, EventArgs.Empty);
            PopupNavigation.Instance.PopAsync();
        }
        #endregion
    }
}