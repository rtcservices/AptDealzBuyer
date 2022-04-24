using AptDealzBuyer.Interfaces;
using AptDealzBuyer.Repository;
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
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //MessagingCenter.Send<string>(Common.NotificationCount, Constraints.Str_NotificationCount);
        }

        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //MessagingCenter.Send<string>(Common.NotificationCount, Constraints.Str_NotificationCount);
            Dispose();
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            try
            {
                if (!Common.EmptyFiels(selectedView))
                {
                    if (selectedView == Constraints.Str_Requirements || selectedView == Constraints.Str_PreviousRequirements
                        || selectedView == Constraints.Str_Order || selectedView == Constraints.Str_ShippingDetails
                        || selectedView == Constraints.Str_Profile || selectedView == Constraints.Str_AboutAptDealz
                        || selectedView == Constraints.Str_TermsPolicies || selectedView == Constraints.Str_FAQHelp
                        )
                    {
                        isNavigate = true;
                        selectedView = Constraints.Str_Home;
                        BindViews(Constraints.Str_Home);
                    }
                    else if (selectedView == Constraints.Str_RaiseGrievances)
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
                await DependencyService.Get<IProfileRepository>().GetMyProfileData();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/GetProfile: " + ex.Message);
            }
        }

        private Color BindTextColor()
        {
            return (Application.Current.UserAppTheme == OSAppTheme.Light) ? (Color)App.Current.Resources["appColor4"] : (Color)App.Current.Resources["appColor6"];
        }

        private void UnselectTab()
        {
            grdMain.Children.Clear();

            imgHome.Source = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.Img_Home : Constraints.Img_HomeDark;
            imgRequirements.Source = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.Img_Requirements : Constraints.Img_RequirementsDark;
            imgOrders.Source = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.Img_Orders : Constraints.Img_OrdersDark;
            imgAccount.Source = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.Img_Account : Constraints.Img_AccountDark;

            lblHome.TextColor = lblRequirements.TextColor = lblOrders.TextColor = lblAccount.TextColor = BindTextColor();
        }

        private void BindViews(string view)
        {
            try
            {
                UnselectTab();
                if (view == Constraints.Str_Home)
                {
                    imgHome.Source = Constraints.Img_HomeActive;
                    lblHome.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new HomeView());
                }
                else if (view == Constraints.Str_PreviousRequirements)
                {
                    imgRequirements.Source = Constraints.Img_RequirementsActive;
                    lblRequirements.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new PreviousRequirementView());
                }
                else if (view == Constraints.Str_Requirements)
                {
                    imgRequirements.Source = Constraints.Img_RequirementsActive;
                    lblRequirements.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new ActiveRequirementView());
                }
                else if (view == Constraints.Str_Order)
                {
                    imgOrders.Source = Constraints.Img_OrdersActive;
                    lblOrders.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new OrderView());
                }
                else if (view == Constraints.Str_RaiseGrievances)
                {
                    grdMain.Children.Add(new OrderView(true));
                }
                else if (view == Constraints.Str_ShippingDetails)
                {
                    imgOrders.Source = Constraints.Img_OrdersActive;
                    lblOrders.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new ShippingDetailsView());
                }
                else if (view == Constraints.Str_Profile)
                {
                    imgAccount.Source = Constraints.Img_AccountActive;
                    lblAccount.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new AccountView());
                }
                else if (view == Constraints.Str_AboutAptDealz)
                {
                    grdMain.Children.Add(new AboutView());
                }
                else if (view == Constraints.Str_TermsPolicies)
                {
                    grdMain.Children.Add(new TermsAndPoliciesView());
                }
                else if (view == Constraints.Str_FAQHelp)
                {
                    grdMain.Children.Add(new FaqHelpView());
                }
                else
                {
                    imgHome.Source = Constraints.Img_HomeActive;
                    lblHome.TextColor = (Color)App.Current.Resources["appColor5"];
                    grdMain.Children.Add(new HomeView());
                }

                selectedView = view;
                MessagingCenter.Send<string>(string.IsNullOrWhiteSpace(Common.NotificationCount) ? "" : Common.NotificationCount, Constraints.Str_NotificationCount);
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
            try
            {
                if (!Common.EmptyFiels(grid.ClassId))
                {
                    if (grid.ClassId == Constraints.Str_Home)
                    {
                        if (selectedView != Constraints.Str_Home)
                        {
                            BindViews(Constraints.Str_Home);
                        }
                    }
                    else if (grid.ClassId == Constraints.Str_Requirements)
                    {
                        if (selectedView != Constraints.Str_Requirements)
                        {
                            this.isNavigate = true;
                            BindViews(Constraints.Str_Requirements);
                        }
                    }
                    else if (grid.ClassId == Constraints.Str_Order)
                    {
                        if (selectedView != Constraints.Str_Order)
                        {
                            this.isNavigate = true;
                            BindViews(Constraints.Str_Order);
                        }
                    }
                    else if (grid.ClassId == Constraints.Str_Profile)
                    {
                        if (selectedView != Constraints.Str_Profile)
                        {
                            this.isNavigate = true;
                            BindViews(Constraints.Str_Profile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MainTabbedPage/Tab_Tapped: " + ex.Message);
            }
        }
        #endregion
    }
}