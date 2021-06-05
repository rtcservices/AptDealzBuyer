using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPopup : PopupPage
    {
        public SearchPopup()
        {
            InitializeComponent();
        }

        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            entrSearch.Text = string.Empty;
            PopupNavigation.Instance.PopAsync();
        }

        private void entrSearch_Unfocused(object sender, FocusEventArgs e)
        {

        }
    }
}