using AptDealzBuyer.Utility;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrevReqPopup : PopupPage
    {
        #region Objects
        // create objects here
        public event EventHandler isRefresh;
        string title = string.Empty;
        string firstType = string.Empty;
        string secondType = string.Empty;
        #endregion

        #region Constructor
        public PrevReqPopup(string title, string firstType, string secondType)
        {
            InitializeComponent();
            this.title = title;
            this.firstType = firstType;
            this.secondType = secondType;

            lblTitle.Text = this.title;
            lblFirstType.Text = this.firstType;
            lblSecondType.Text = this.secondType;
        }
        #endregion

        #region Events
        private void StkFirstType_Tapped(object sender, EventArgs e)
        {
            imgSecondType.Source = Constraints.Redio_UnSelected;
            imgFirstType.Source = Constraints.Radio_Selected;
            isRefresh?.Invoke(firstType, EventArgs.Empty);
            PopupNavigation.Instance.PopAsync();
        }

        private void StkSecondType_Tapped(object sender, EventArgs e)
        {
            imgFirstType.Source = Constraints.Redio_UnSelected;
            imgSecondType.Source = Constraints.Radio_Selected;
            isRefresh?.Invoke(secondType, EventArgs.Empty);
            PopupNavigation.Instance.PopAsync();
        }
        #endregion
    }
}