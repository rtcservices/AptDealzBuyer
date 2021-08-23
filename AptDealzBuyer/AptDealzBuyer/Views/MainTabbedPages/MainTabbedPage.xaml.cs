using AptDealzBuyer.API;
using AptDealzBuyer.Interfaces;
using AptDealzBuyer.Utility;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : ContentPage
    {
        #region [ Objects ]
        private string selectedView;
        private bool isNavigate = false;
        #endregion

        #region [ Constructor ]
        public MainTabbedPage(string OpenView, bool isNavigate = false)
        {
            InitializeComponent();
            this.isNavigate = isNavigate;
            selectedView = OpenView;
            BindViews(selectedView);
            GetProfile();
        }

        #endregion

        #region [ Methods ]
        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Dispose();
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            try
            {
                if (!Common.EmptyFiels(selectedView))
                {
                    if (selectedView == "ActiveRequirements" || selectedView == "PreviousRequirements"
                        || selectedView == "Order" || selectedView == "ShippingDetails"
                        || selectedView == "Profile" || selectedView == "AboutAptDealz"
                        || selectedView == "TermsPolicies" || selectedView == "FAQHelp"
                        )
                    {
                        isNavigate = true;
                        selectedView = "Home";
                        BindViews("Home");
                    }
                    else if (selectedView == "RaiseGrievances")
                    {
                        Navigation.PopAsync();
                    }
                }

                if (!isNavigate)
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

                isNavigate = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/OnBackButtonPressed: " + ex.Message);
            }
            return true;
        }

        private async Task GetProfile()
        {
            try
            {
                ProfileAPI profileAPI = new ProfileAPI();
                var mResponse = await profileAPI.GetMyProfileData();
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        Common.mBuyerDetail = jObject.ToObject<Model.Request.BuyerDetails>();
                    }
                }
                else
                {
                    if (mResponse != null)
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/GetProfile: " + ex.Message);
            }
        }

        private void UnselectTab()
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

        private void BindViews(string view)
        {
            try
            {
                UnselectTab();
                if (view == "Home")
                {
                    imgHome.Source = "iconHomeActive.png";
                    lblHome.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new HomeView());
                }
                else if (view == "PreviousRequirements")
                {
                    imgRequirements.Source = "iconRequirementsActive.png";
                    lblRequirements.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new PreviousRequirementView());
                }
                else if (view == "ActiveRequirements")
                {
                    imgRequirements.Source = "iconRequirementsActive.png";
                    lblRequirements.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new ActiveRequirementView());
                }
                else if (view == "Order")
                {
                    imgOrders.Source = "iconOrdersActive.png";
                    lblOrders.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new OrderView());
                }
                else if (view == "RaiseGrievances")
                {
                    grdMain.Children.Add(new OrderView(true));
                }
                else if (view == "ShippingDetails")
                {
                    imgOrders.Source = "iconOrdersActive.png";
                    lblOrders.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new ShippingDetailsView());
                }
                else if (view == "Profile")
                {
                    imgAccount.Source = "iconAccountActive.png";
                    lblAccount.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new AccountView());
                }
                else if (view == "AboutAptDealz")
                {
                    grdMain.Children.Add(new AboutView());
                }
                else if (view == "TermsPolicies")
                {
                    grdMain.Children.Add(new TermsAndPoliciesView());
                }
                else if (view == "FAQHelp")
                {
                    grdMain.Children.Add(new FaqHelpView());
                }
                else
                {
                    imgHome.Source = "iconHomeActive.png";
                    lblHome.TextColor = (Color)App.Current.Resources["Orange"];
                    grdMain.Children.Add(new HomeView());
                }

                selectedView = view;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/BindViews: " + ex.Message);
            }
        }
        #endregion

        #region [ Events ]
        private void Tab_Tapped(object sender, EventArgs e)
        {
            var grid = (Grid)sender;
            if (grid.IsEnabled)
            {
                try
                {
                    grid.IsEnabled = false;
                    if (!Common.EmptyFiels(grid.ClassId))
                    {
                        if (grid.ClassId == "Home")
                        {
                            BindViews("Home");
                        }
                        else if (grid.ClassId == "Requirements")
                        {
                            this.isNavigate = true;
                            BindViews("ActiveRequirements");
                        }
                        else if (grid.ClassId == "Order")
                        {
                            this.isNavigate = true;
                            BindViews("Order");
                        }
                        else if (grid.ClassId == "Profile")
                        {
                            this.isNavigate = true;
                            BindViews("Profile");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("MainTabbedPage/Tab_Tapped: " + ex.Message);
                }
                finally
                {
                    grid.IsEnabled = true;
                }
            }
        }
        #endregion
    }
}