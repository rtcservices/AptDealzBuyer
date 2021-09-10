using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RaiseGrievancePage : ContentPage
    {
        #region [ Objects ]
        private string relativePath = string.Empty;
        private string ErrorMessage = string.Empty;
        private List<string> mComplaintTypeList;
        private List<string> documentList;
        private string OrderId;
        #endregion

        #region [ Constructor ]
        public RaiseGrievancePage(string OrderId)
        {
            try
            {
                InitializeComponent();
                this.OrderId = OrderId;
                mComplaintTypeList = new List<string>();
                documentList = new List<string>();

                CapitalizeWord();
                BindComplaintType();
                GetOrderDetails();

                MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount); MessagingCenter.Subscribe<string>(this, Constraints.Str_NotificationCount, (count) =>
                {
                    if (!Common.EmptyFiels(Common.NotificationCount))
                    {
                        lblNotificationCount.Text = count;
                        frmNotification.IsVisible = true;
                    }
                    else
                    {
                        frmNotification.IsVisible = false;
                        lblNotificationCount.Text = string.Empty;
                    }
                });
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RaiseGrievancePage/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]
        private void CapitalizeWord()
        {
            txtDescription.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
            txtSolution.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
        }
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

        public void BindComplaintType()
        {
            try
            {
                mComplaintTypeList.Add(GrievancesType.OrderRelated.ToString().ToCamelCase());
                mComplaintTypeList.Add(GrievancesType.DelayedDelivery.ToString().ToCamelCase());
                mComplaintTypeList.Add(GrievancesType.PaymentRelated.ToString().ToCamelCase());
                pkType.ItemsSource = mComplaintTypeList.ToList();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RaiseGrievancePage/OrderStatusList: " + ex.Message);
            }
        }

        private async Task GetOrderDetails()
        {
            try
            {
                var mOrder = await DependencyService.Get<IOrderRepository>().GetOrderDetails(OrderId);
                if (mOrder != null)
                {
                    lblRequirementId.Text = mOrder.RequirementNo;
                    lblQuoteNo.Text = mOrder.QuoteNo;
                    lblInvoiceNo.Text = mOrder.OrderNo;

                    lblSellerName.Text = mOrder.SellerContact.Name;
                    lblShippingPINCode.Text = mOrder.ShippingPincode;
                    lblQuantity.Text = mOrder.RequestedQuantity + " " + mOrder.Unit;
                    lblUnitPrice.Text = "Rs " + mOrder.UnitPrice;
                    lblNetAmount.Text = "Rs " + mOrder.NetAmount;
                    lblHandlingCharges.Text = "Rs " + mOrder.HandlingCharges;
                    lblShippingCharges.Text = "Rs " + mOrder.ShippingCharges;
                    lblInsuranceCharges.Text = "Rs " + mOrder.InsuranceCharges;

                    lblTotalAmount.Text = "Rs " + mOrder.TotalAmount;
                    lblOrderStatus.Text = mOrder.OrderStatusDescr;
                }

            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RaiseGrievancePage/GetGrievancesDetails: " + ex.Message);
            }
        }

        private int GetGrievanceType(string grievanceType)
        {
            grievanceType = grievanceType.Replace(" ", "");
            switch (grievanceType)
            {
                case "DelayedDelivery":
                    return (int)GrievancesType.DelayedDelivery;
                case "OrderRelated":
                    return (int)GrievancesType.OrderRelated;
                case "PaymentRelated":
                    return (int)GrievancesType.PaymentRelated;
                default:
                    return 0;
            }
        }

        private RaiseGrievance FillGrievance()
        {
            try
            {
                RaiseGrievance mRaiseGrievance = new RaiseGrievance();
                mRaiseGrievance.OrderId = OrderId;
                if (pkType.SelectedIndex != -1)
                {
                    mRaiseGrievance.GrievanceType = GetGrievanceType(pkType.SelectedItem.ToString());
                }
                else
                {
                    FrmType.BorderColor = (Color)App.Current.Resources["appColor3"];
                    ErrorMessage = Constraints.Required_ComplainType;
                    return null;
                }

                if (documentList != null && documentList.Count > 0)
                {
                    mRaiseGrievance.Documents = documentList;
                }
                if (!Common.EmptyFiels(txtDescription.Text))
                {
                    mRaiseGrievance.IssueDescription = txtDescription.Text;
                }
                if (!Common.EmptyFiels(txtSolution.Text))
                {
                    mRaiseGrievance.PreferredSolution = txtSolution.Text;
                }
                return mRaiseGrievance;
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    ErrorMessage = ex.Message;
                }
                return null;
            }
        }

        public async Task CreateGrievance()
        {
            try
            {
                GrievanceAPI grievanceAPI = new GrievanceAPI();
                UserDialogs.Instance.ShowLoading(Constraints.Loading);

                var mRaiseGrievance = FillGrievance();
                if (mRaiseGrievance != null)
                {
                    var mResponse = await grievanceAPI.CreateGrievanceFromBuyer(mRaiseGrievance);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        Common.DisplaySuccessMessage(mResponse.Message);
                        Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));
                    }
                    else
                    {
                        if (mResponse != null && !Common.EmptyFiels(mResponse.Message))
                            Common.DisplayErrorMessage(mResponse.Message);
                        else
                            Common.DisplayErrorMessage(Constraints.Something_Wrong);
                    }
                }
                else
                {
                    if (ErrorMessage == null)
                    {
                        ErrorMessage = Constraints.Something_Wrong;
                    }
                    Common.DisplayErrorMessage(ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RaiseGrievancePage/CreateGrievance: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion

        #region [ Events ]     
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(image: ImgMenu);
            //Common.OpenMenu();
        }

        private async void ImgNotification_Tapped(object sender, EventArgs e)
        {
            var Tab = (Grid)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    await Navigation.PushAsync(new DashboardPages.NotificationPage());
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("RaiseGrievancePage/ImgNotification_Tapped: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            await Navigation.PopAsync();
        }

        private void ImgType_Tapped(object sender, EventArgs e)
        {
            pkType.Focus();
        }

        private async void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnSubmit);
                    await CreateGrievance();
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("RaiseGrievancePage/BtnSubmit_Clicked: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private async void UploadProductImage_Tapped(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(imageButton: ImgUplode);
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                await FileSelection.FilePickup();
                relativePath = await DependencyService.Get<IFileUploadRepository>().UploadFile((int)FileUploadCategory.ProfileDocuments);

                if (!Common.EmptyFiels(relativePath))
                {
                    ImgProductImage.Source = relativePath;
                    if (documentList == null)
                        documentList = new List<string>();

                    documentList.Add(relativePath);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RaiseGrievancePage/UploadProductImage: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));
        }

        private void Picker_Unfocused(object sender, FocusEventArgs e)
        {
            try
            {
                var picker = (Picker)sender;
                if (picker.SelectedIndex != -1)
                {
                    FrmType.BorderColor = (Color)App.Current.Resources["appColor8"];
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RaiseGrievancePage/Picker_Unfocused: " + ex.Message);
            }
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            rfView.IsRefreshing = true;
            await GetOrderDetails();
            rfView.IsRefreshing = false;
        }
        #endregion
    }
}