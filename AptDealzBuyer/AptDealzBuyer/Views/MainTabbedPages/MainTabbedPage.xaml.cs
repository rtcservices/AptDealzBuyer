using AptDealzBuyer.Interfaces;
using AptDealzBuyer.Utility;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : ContentPage
    {
        #region Objects
        private string selectedView;
        #endregion

        #region Constructor
        public MainTabbedPage(string OpenView)
        {
            InitializeComponent();
            selectedView = OpenView;
            BindViews(selectedView);
        }
        #endregion

        #region Methods
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            try
            {
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await DisplayAlert(Constraints.Alert, Constraints.DoYouWantToExit, Constraints.Yes, Constraints.No);
                        if (result)
                        {
                            Xamarin.Forms.DependencyService.Get<ICloseAppOnBackButton>().CloseApp();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/OnBackButtonPressed: " + ex.Message);
            }
            return true;
        }

        public void UnselectTab()
        {
            grdMain.Children.Clear();
            imgHome.Source = "iconHome.png";
            imgRequirements.Source = "iconRequirements.png";
            imgOrders.Source = "iconOrders.png";
            imgAccount.Source = "iconAccount.png";
            lblHome.TextColor = (Color)App.Current.Resources["Black"];
            lblRequirements.TextColor = (Color)App.Current.Resources["Black"];
            lblOrders.TextColor = (Color)App.Current.Resources["Black"];
            lblAccount.TextColor = (Color)App.Current.Resources["Black"];
        }

        public void BindViews(string view)
        {
            if (view == "Home")
            {
                UnselectTab();
                imgHome.Source = "iconHomeActive.png";
                lblHome.TextColor = (Color)App.Current.Resources["Orange"];
                grdMain.Children.Add(new HomeView());
            }
            else if (view == "PreviousRequirements")
            {
                UnselectTab();
                imgRequirements.Source = "iconRequirementsActive.png";
                lblRequirements.TextColor = (Color)App.Current.Resources["Orange"];
                grdMain.Children.Add(new PreviousRequirementView());
            }
            else if (view == "ActiveRequirements")
            {
                UnselectTab();
                imgRequirements.Source = "iconRequirementsActive.png";
                lblRequirements.TextColor = (Color)App.Current.Resources["Orange"];
                grdMain.Children.Add(new ActiveRequirementView());
            }
            else if (view == "Orders")
            {
                UnselectTab();
                imgOrders.Source = "iconOrdersActive.png";
                lblOrders.TextColor = (Color)App.Current.Resources["Orange"];
                grdMain.Children.Add(new OrderView());
            }
            else if (view == "OrderHistory")
            {
                UnselectTab();
                imgOrders.Source = "iconOrdersActive.png";
                lblOrders.TextColor = (Color)App.Current.Resources["Orange"];
                grdMain.Children.Add(new OrderView());
            }
            else if (view == "ShippingDetails")
            {
                UnselectTab();
                imgOrders.Source = "iconOrdersActive.png";
                lblOrders.TextColor = (Color)App.Current.Resources["Orange"];
                grdMain.Children.Add(new OrderView());
            }
            else if (view == "Profile")
            {
                UnselectTab();
                imgAccount.Source = "iconAccountActive.png";
                lblAccount.TextColor = (Color)App.Current.Resources["Orange"];
                grdMain.Children.Add(new AccountView());
            }
            else if (view == "AboutAptDealz")
            {
                UnselectTab();
                grdMain.Children.Add(new AboutView());
            }
            else if (view == "TermsPolicies")
            {
                UnselectTab();
                grdMain.Children.Add(new TermsAndPoliciesView());
            }
            else if (view == "FAQHelp")
            {
                UnselectTab();
                grdMain.Children.Add(new FaqHelpView());
            }
            else
            {
                UnselectTab();
                imgHome.Source = "iconHomeActive.png";
                lblHome.TextColor = (Color)App.Current.Resources["Orange"];
                grdMain.Children.Add(new HomeView());
            }
        }
        #endregion

        #region Events
        private void StkHome_Tapped(object sender, EventArgs e)
        {
            BindViews("Home");
        }

        private void StkRequirements_Tapped(object sender, EventArgs e)
        {
            BindViews("ActiveRequirements");
        }

        private void StkOrders_Tapped(object sender, EventArgs e)
        {
            BindViews("Orders");
        }

        private void StkAccount_Tapped(object sender, EventArgs e)
        {
            BindViews("Profile");
        }
        #endregion
    }
}