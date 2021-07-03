using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuccessPopup : PopupPage
    {
        #region Objects        
        public event EventHandler isRefresh;
        #endregion

        #region Constructor
        public SuccessPopup(string ReqMessage)
        {
            InitializeComponent();
            lblMessage.Text = ReqMessage;
        }
        #endregion

        #region Methods

        #endregion

        #region Events
        private void FrmHome_Tapped(object sender, EventArgs e)
        {
            isRefresh?.Invoke(true, EventArgs.Empty);
            PopupNavigation.Instance.PopAsync();
        }
        #endregion
    }
}