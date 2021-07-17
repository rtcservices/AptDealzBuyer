using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPopup : PopupPage
    {
        #region Objects
        // create objects here
        public event EventHandler isRefresh;
        #endregion

        #region Constructor
        public PaymentPopup(string Amount)
        {
            InitializeComponent();
            //lblMessage.Text = "Make a payment of Rs " + Amount + " to reveal the Seller contacts";
            lblMessage.Text = "Make a payment of Rs " + Amount + " to Accept Quote";
            this.CloseWhenBackgroundIsClicked = false;
        }
        #endregion

        #region Methods

        #endregion

        #region Events        
        private void FrmPay_Tapped(object sender, EventArgs e)
        {
            isRefresh?.Invoke(true, EventArgs.Empty);
            PopupNavigation.Instance.PopAsync();
        }

        private void FrmBack_Tapped(object sender, EventArgs e)
        {
            isRefresh?.Invoke(false, EventArgs.Empty);
            PopupNavigation.Instance.PopAsync();
        }
        #endregion
    }
}