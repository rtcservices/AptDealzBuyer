using System;
using Xamarin.Forms;
using AptDealzBuyer.Utility;
using Xamarin.Forms.Xaml;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Repository;

namespace AptDealzBuyer.Views.Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailsPage : ContentPage
    {
        #region Objects
        private Order mOrder;
        private string OrderId;
        private bool isOrderCancelled;
        #endregion

        #region Constructor
        public OrderDetailsPage(string orderId, bool isCancelled = false)
        {
            InitializeComponent();
            mOrder = new Order();
            OrderId = orderId;
            isOrderCancelled = isCancelled;           
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetOrderDetails();
        }
        public async void GetOrderDetails()
        {
            try
            {
                if (!isOrderCancelled)
                {
                    lblCancelledOrder.IsVisible = false;
                    AllDetails.IsVisible = true;
                    mOrder = await DependencyService.Get<IOrderRepository>().GetOrderDetails(OrderId);
                    if (mOrder != null)
                    {
                        if (mOrder.PickupProductDirectly)
                            lblPinCodeTitle.Text = "Product Pickup PIN Code";
                        else
                            lblPinCodeTitle.Text = "Shipping PIN Code";

                        lblRequirementId.Text = mOrder.RequirementNo;
                        lblQuoteRefNo.Text = mOrder.QuoteNo;
                        lblBuyerName.Text = mOrder.Buyer;
                        lblSellerName.Text = mOrder.Seller;
                        lblShippingPINCode.Text = mOrder.ShippingPincode;
                        lblRequestedQuantity.Text = mOrder.RequestedQuantity + mOrder.Unit;
                        lblUnitPrice.Text = "Rs " + mOrder.UnitPrice;
                        lblNetAmount.Text = "Rs " + mOrder.NetAmount;
                        lblHandlingCharges.Text = "Rs " + mOrder.HandlingCharges;
                        lblShippingCharges.Text = "Rs " + mOrder.ShippingCharges;
                        lblInsuranceCharges.Text = "Rs " + mOrder.InsuranceCharges;
                        lblOriginProduct.Text = mOrder.Country;
                        lblInvoiceNo.Text = mOrder.OrderNo;
                        lblTotalAmount.Text = "Rs " + mOrder.TotalAmount;
                        lblExpectedDate.Text = mOrder.ExpectedDelivery.ToString("dd/MM/yyyy");
                        if (mOrder.SellerContact != null)
                        {
                            lblSellerContact.Text = mOrder.SellerContact.PhoneNumber;
                        }
                        lblOrderStatus.Text = mOrder.OrderStatusEnum;
                    }
                }
                else
                {
                    AllDetails.IsVisible = false;
                    lblCancelledOrder.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/GetOrderDetails: " + ex.Message);
            }
        }

        async void CancelOrder()
        {
            try
            {
                await DependencyService.Get<IOrderRepository>().CancelOrder(OrderId);
                GetOrderDetails();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/CancelOrder: " + ex.Message);
            }
        }
        #endregion

        #region Events
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(image: ImgMenu);
            //Common.OpenMenu();
        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void BtnCancelOrder_Tapped(object sender, EventArgs e)
        {
            CancelOrder();
        }

        private void FrmTrackOrder_Tapped(object sender, EventArgs e)
        {

        }

        private void FrmRaiseGrievance_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Orders.RaiseGrievancePage());
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
        #endregion
    }
}