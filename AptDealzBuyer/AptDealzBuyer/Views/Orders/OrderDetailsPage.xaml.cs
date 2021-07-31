using System;
using Xamarin.Forms;
using AptDealzBuyer.Utility;
using Xamarin.Forms.Xaml;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Repository;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model.Request;
using System.IO;

namespace AptDealzBuyer.Views.Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailsPage : ContentPage
    {
        #region Objects
        private Order mOrder;
        private string OrderId;
        RateAndReviewAPI rateAndReviewAPI;
        #endregion

        #region Constructor
        public OrderDetailsPage(string orderId)
        {
            InitializeComponent();
            mOrder = new Order();
            rateAndReviewAPI = new RateAndReviewAPI();
            OrderId = orderId;

            MessagingCenter.Subscribe<string>(this, "NotificationCount", (count) =>
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
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetOrderDetails();
        }

        private async Task GetOrderDetails()
        {
            try
            {
                mOrder = await DependencyService.Get<IOrderRepository>().GetOrderDetails(OrderId);
                if (mOrder != null)
                {
                    if (mOrder.PickupProductDirectly)
                        lblPinCodeTitle.Text = "Product Pickup PIN Code";
                    else
                        lblPinCodeTitle.Text = "Shipping PIN Code";

                    if (mOrder.SellerContact != null)
                    {
                        lblSellerContact.Text = mOrder.SellerContact.PhoneNumber;
                    }

                    lblOrderId.Text = mOrder.OrderNo;
                    lblRequirementId.Text = mOrder.RequirementNo;
                    lblQuoteRefNo.Text = mOrder.QuoteNo;
                    lblBuyerName.Text = mOrder.BuyerContact.Name;
                    lblSellerName.Text = mOrder.SellerContact.Name;
                    lblShippingPINCode.Text = mOrder.ShippingPincode;
                    lblRequestedQuantity.Text = mOrder.RequestedQuantity + " " + mOrder.Unit;
                    lblUnitPrice.Text = "Rs " + mOrder.UnitPrice;
                    lblNetAmount.Text = "Rs " + mOrder.NetAmount;
                    lblHandlingCharges.Text = "Rs " + mOrder.HandlingCharges;
                    lblShippingCharges.Text = "Rs " + mOrder.ShippingCharges;
                    lblInsuranceCharges.Text = "Rs " + mOrder.InsuranceCharges;
                    lblOriginProduct.Text = mOrder.Country;
                    lblInvoiceNo.Text = mOrder.OrderNo;
                    lblTotalAmount.Text = "Rs " + mOrder.TotalAmount;
                    lblExpectedDate.Text = mOrder.ExpectedDelivery.ToString("dd/MM/yyyy");

                    lblOrderStatus.Text = mOrder.OrderStatusDescr;

                    lblSellerAddress.Text = mOrder.SellerAddressDetails.Building + "\n"
                                          + mOrder.SellerAddressDetails.Street + "\n"
                                          + mOrder.SellerAddressDetails.City + ", " + mOrder.SellerAddressDetails.PinCode + "\n"
                                          + mOrder.SellerAddressDetails.Landmark + ", " + mOrder.SellerAddressDetails.Country;

                    if (mOrder.OrderStatus == (int)OrderStatus.Accepted)
                    {
                        GrdCancelOrder.IsVisible = true;
                        GrdTrackRaise.IsVisible = false;
                    }
                    else if (mOrder.OrderStatus == (int)OrderStatus.ReadyForPickup)
                    {
                        GrdQRCode.IsVisible = true;
                        BtnShowQrCode.IsVisible = true;
                        GrdTrackRaise.IsVisible = false;
                    }
                    else if (mOrder.OrderStatus == (int)OrderStatus.Shipped)
                    {
                        GrdTrackRaise.IsVisible = true;
                        BtnConfirmDelivery.IsVisible = true;
                    }
                    else if (mOrder.OrderStatus == (int)OrderStatus.Completed)
                    {
                        GrdRate.IsVisible = true;
                        GrdTrackRaise.IsVisible = false;
                    }
                    else if (mOrder.OrderStatus == (int)OrderStatus.Delivered)
                    {
                        GrdTrackRaise.IsVisible = false;
                        GrdStatusButtons.IsVisible = false;
                    }
                    else
                    {
                        GrdTrackRaise.IsVisible = false;
                        GrdStatusButtons.IsVisible = false;
                    }
                }

            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/GetOrderDetails: " + ex.Message);
            }
        }

        private async Task CancelOrder()
        {
            try
            {
                await DependencyService.Get<IOrderRepository>().CancelOrder(OrderId);
                await GetOrderDetails();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/CancelOrder: " + ex.Message);
            }
        }

        private async Task RatingReview(bool isSellerRating)
        {

            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                Response mResponse = new Response();
                var mRatingReview = new RatingReview();
                mRatingReview.ReviewForOrderId = OrderId;
                mRatingReview.ReviewedForSellerId = mOrder.SellerContact.UserId;

                if (isSellerRating)
                {
                    mRatingReview.SellerRating = (decimal)rvSeller.Value;
                    mResponse = await rateAndReviewAPI.ReviewSeller(mRatingReview);
                }
                else
                {
                    mRatingReview.SellerRating = (decimal)rvProduct.Value;
                    mResponse = await rateAndReviewAPI.ReviewSellerProduct(mRatingReview);
                }

                if (mResponse != null && mResponse.Succeeded)
                {
                    Common.DisplaySuccessMessage(mResponse.Message);
                    await GetOrderDetails();
                }
                else
                {
                    if (mResponse != null && !Common.EmptyFiels(mResponse.Message))
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/GetOrderDetails: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

        }

        private async Task ShowQRCodeImage()
        {
            try
            {
                string imageBase64 = await DependencyService.Get<IOrderRepository>().GenerateQRCodeImage(OrderId);
                if (!Common.EmptyFiels(imageBase64))
                {
                    BtnShowQrCode.IsVisible = false;
                    ScanORCode.IsVisible = true;
                    //var imgstring = ImageConvertion.LoadBase64(ImageSource);
                    //ImgQRCode.Source = imgstring;
                    ImgQRCode.Source = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(imageBase64)));
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/ShowQRCodeImage: " + ex.Message);
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
            Navigation.PushAsync(new DashboardPages.NotificationPage());
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Navigation.PopAsync();
        }


        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            try
            {
                rfView.IsRefreshing = true;
                await GetOrderDetails();
                rfView.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/RefreshView_Refreshing: " + ex.Message);
            }
        }

        private async void BtnRateSeller_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnRateSeller);
            await RatingReview(true);
        }

        private async void BtnRateProduct_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnRateProduct);
            await RatingReview(false);
        }

        private void BtnRateAptDealz_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnRateAptDealz);
        }

        private void BtnRepostReqt_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnRepostReqt);
        }

        private void BtnRaiseGrievance_Tapped(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(button: BtnRaiseGrievance);
                Navigation.PushAsync(new Orders.RaiseGrievancePage(OrderId));
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/BtnRaiseGrievance_Tapped: " + ex.Message);
            }
        }

        private async void BtnCancelOrder_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnCancelOrder);
            await CancelOrder();
        }

        private void BtnTrackOrder_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnTrackOrder);
        }


        private void BtnConfirmDelivery_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnConfirmDelivery);
        }

        private async void BtnShowQrCode_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnShowQrCode);
            await ShowQRCodeImage();
        }
        #endregion

        private void CopyString_Tapped(object sender, EventArgs e)
        {
            try
            {
                var stackLayout = (StackLayout)sender;
                if (!Common.EmptyFiels(stackLayout.ClassId))
                {
                    if (stackLayout.ClassId == "OrderId")
                    {
                        string message = Constraints.CopiedOrderId;
                        Common.CopyText(lblOrderId, message);
                    }
                    else if (stackLayout.ClassId == "RequirementId")
                    {
                        string message = Constraints.CopiedRequirementId;
                        Common.CopyText(lblRequirementId, message);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/CopyString_Tapped: " + ex.Message);
            }
        }
    }
}