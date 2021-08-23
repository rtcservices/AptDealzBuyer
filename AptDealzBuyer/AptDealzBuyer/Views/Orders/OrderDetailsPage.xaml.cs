using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.DashboardPages;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailsPage : ContentPage
    {
        #region [ Objects ]
        private Order mOrder;
        private string OrderId;
        RateAndReviewAPI rateAndReviewAPI;
        #endregion

        #region [ Constructor ]
        public OrderDetailsPage(string orderId)
        {
            try
            {
                InitializeComponent();
                mOrder = new Order();
                rateAndReviewAPI = new RateAndReviewAPI();
                OrderId = orderId;

                MessagingCenter.Unsubscribe<string>(this, "NotificationCount"); MessagingCenter.Subscribe<string>(this, "NotificationCount", (count) =>
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
                Common.DisplayErrorMessage("OrderDetailsPage/Ctor: " + ex.Message);
            }
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
                    #region [ Details ]
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
                    lblRequirementTitle.Text = mOrder.Title;
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
                    #endregion

                    #region [ Address ]
                    lblSellerAddress.Text = mOrder.SellerAddressDetails.Building + "\n"
                                                      + mOrder.SellerAddressDetails.Street + "\n"
                                                      + mOrder.SellerAddressDetails.City + ", " + mOrder.SellerAddressDetails.PinCode + "\n"
                                                      + mOrder.SellerAddressDetails.Landmark + ", " + mOrder.SellerAddressDetails.Country;

                    if (mOrder.ShippingAddressDetails != null &&
                        (!Common.EmptyFiels(mOrder.ShippingAddressDetails.Building) ||
                        !Common.EmptyFiels(mOrder.ShippingAddressDetails.Street) ||
                        !Common.EmptyFiels(mOrder.ShippingAddressDetails.City) ||
                        !Common.EmptyFiels(mOrder.ShippingAddressDetails.PinCode) ||
                        !Common.EmptyFiels(mOrder.ShippingAddressDetails.Landmark) ||
                        !Common.EmptyFiels(mOrder.ShippingAddressDetails.State) ||
                        mOrder.ShippingAddressDetails.Country > 0))
                    {
                        lblShippingAddress.Text = mOrder.ShippingAddressDetails.Building + "\n"
                                                          + mOrder.ShippingAddressDetails.Street + ", " + mOrder.ShippingAddressDetails.City + "\n"
                                                          + mOrder.ShippingAddressDetails.State + " " + mOrder.ShippingAddressDetails.PinCode + "\n"
                                                          + mOrder.ShippingAddressDetails.Landmark + ", " + mOrder.ShippingAddressDetails.Country;
                    }
                    else
                    {
                        lblShippingAddress.Text = "No shipping address found";
                    }
                    #endregion

                    #region [ Status ]
                    if (mOrder.OrderStatus == (int)OrderStatus.Accepted)
                    {
                        //visible only Cancel Order
                        GrdTrackOrderAndRaiseGrvInMainGrid.IsVisible = false;
                        BtnShowQrCodeInMainGrid.IsVisible = false;
                        GrdScanQRCodeImageInMainGrid.IsVisible = false;

                        GrdSubStatusButtons.IsVisible = true;
                        GrdCancelOrderInSubGrid.IsVisible = true;
                        GrdWarningAndRaiseComplainInSubGrid.IsVisible = false;
                        BtnConfirmDeliveryInSubGrid.IsVisible = false;
                        GrdRateInSubGrid.IsVisible = false;
                    }
                    else if (mOrder.OrderStatus == (int)OrderStatus.Pending)
                    {
                        //Nothing to show
                        GrdTrackOrderAndRaiseGrvInMainGrid.IsVisible = false;
                        GrdScanQRCodeImageInMainGrid.IsVisible = false;
                        BtnShowQrCodeInMainGrid.IsVisible = false;

                        GrdSubStatusButtons.IsVisible = false;
                    }
                    else if (mOrder.OrderStatus == (int)OrderStatus.ReadyForPickup)
                    {
                        //visible Warning Msg,Raise Complain button ,Qr Code image   
                        GrdTrackOrderAndRaiseGrvInMainGrid.IsVisible = false;
                        BtnShowQrCodeInMainGrid.IsVisible = true;

                        GrdSubStatusButtons.IsVisible = true;
                        GrdCancelOrderInSubGrid.IsVisible = false;
                        GrdWarningAndRaiseComplainInSubGrid.IsVisible = true;
                        BtnConfirmDeliveryInSubGrid.IsVisible = false;
                        GrdRateInSubGrid.IsVisible = false;
                    }
                    else if (mOrder.OrderStatus == (int)OrderStatus.Shipped)
                    {
                        //visible Track order and Raise grv buttons
                        GrdTrackOrderAndRaiseGrvInMainGrid.IsVisible = true;
                        BtnShowQrCodeInMainGrid.IsVisible = false;
                        GrdScanQRCodeImageInMainGrid.IsVisible = false;

                        GrdSubStatusButtons.IsVisible = false;
                    }
                    else if (mOrder.OrderStatus == (int)OrderStatus.Completed)
                    {
                        //Visible Rating / Repost Req and Raise Grv Buttons   
                        GrdTrackOrderAndRaiseGrvInMainGrid.IsVisible = false;
                        BtnShowQrCodeInMainGrid.IsVisible = false;
                        GrdScanQRCodeImageInMainGrid.IsVisible = false;

                        GrdSubStatusButtons.IsVisible = true;
                        GrdRateInSubGrid.IsVisible = true;
                        GrdCancelOrderInSubGrid.IsVisible = false;
                        GrdWarningAndRaiseComplainInSubGrid.IsVisible = false;
                        BtnConfirmDeliveryInSubGrid.IsVisible = false;
                    }
                    else if (mOrder.OrderStatus == (int)OrderStatus.Delivered)
                    {
                        //Nothing ot visible
                        GrdTrackOrderAndRaiseGrvInMainGrid.IsVisible = false;
                        GrdScanQRCodeImageInMainGrid.IsVisible = false;
                        BtnShowQrCodeInMainGrid.IsVisible = false;

                        GrdSubStatusButtons.IsVisible = false;
                    }
                    else if (mOrder.OrderStatus == (int)OrderStatus.CancelledFromBuyer)
                    {
                        //Nothing ot visible
                        GrdTrackOrderAndRaiseGrvInMainGrid.IsVisible = false;
                        GrdScanQRCodeImageInMainGrid.IsVisible = false;
                        BtnShowQrCodeInMainGrid.IsVisible = false;

                        GrdSubStatusButtons.IsVisible = false;
                    }
                    else if ((mOrder.OrderStatus == (int)OrderStatus.Shipped ||
                         mOrder.OrderStatus == (int)OrderStatus.Delivered ||
                         mOrder.OrderStatus == (int)OrderStatus.Completed) &&
                         (mOrder.IsDeliveryConfirmedFromBuyer == false))
                    {
                        //Visible only ConfirmDelivery Button
                        GrdTrackOrderAndRaiseGrvInMainGrid.IsVisible = false;
                        BtnShowQrCodeInMainGrid.IsVisible = false;
                        GrdScanQRCodeImageInMainGrid.IsVisible = false;

                        GrdSubStatusButtons.IsVisible = true;
                        BtnConfirmDeliveryInSubGrid.IsVisible = true;
                        GrdRateInSubGrid.IsVisible = false;
                        GrdCancelOrderInSubGrid.IsVisible = false;
                        GrdWarningAndRaiseComplainInSubGrid.IsVisible = false;
                    }
                    else
                    {
                        GrdTrackOrderAndRaiseGrvInMainGrid.IsVisible = false;
                        GrdScanQRCodeImageInMainGrid.IsVisible = false;
                        BtnShowQrCodeInMainGrid.IsVisible = false;

                        GrdSubStatusButtons.IsVisible = false;
                    }
                    #endregion
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

        private async Task ConfirmDelivery()
        {
            try
            {
                await DependencyService.Get<IOrderRepository>().ConfirmDelivery(OrderId);
                await GetOrderDetails();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/ConfirmDelivery: " + ex.Message);
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
                    BtnShowQrCodeInMainGrid.IsVisible = false;
                    GrdScanQRCodeImageInMainGrid.IsVisible = true;
                    ImgQRCode.Source = Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(imageBase64)));
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/ShowQRCodeImage: " + ex.Message);
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
                    Common.DisplayErrorMessage("OrderDetailsPage/ImgNotification_Tapped: " + ex.Message);
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
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnRateSeller);
                    await RatingReview(true);
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("OrderDetailsPage/BtnRateSeller_Clicked: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private async void BtnRateProduct_Clicked(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnRateProduct);
                    await RatingReview(false);
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("OrderDetailsPage/BtnRateProduct_Clicked: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private void BtnRateAptDealz_Clicked(object sender, EventArgs e)
        {

            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnRateAptDealz);
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("OrderDetailsPage/BtnRateAptDealz_Clicked: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private void BtnRepostReqt_Clicked(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnRepostReqt);
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("OrderDetailsPage/BtnRepostReqt_Clicked: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private async void BtnRaiseGrievance_Tapped(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnRaiseGrievance);
                    await Navigation.PushAsync(new Orders.RaiseGrievancePage(OrderId));
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("OrderDetailsPage/BtnRaiseGrievance_Tapped: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private async void BtnCancelOrder_Tapped(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnCancelOrder);
                    await CancelOrder();
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("OrderDetailsPage/BtnCancelOrder_Tapped: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }

        }

        private void BtnTrackOrder_Tapped(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnTrackOrder);
                    if (mOrder != null && mOrder.TrackingLink != null && mOrder.TrackingLink.Length > 10)
                    {
                        Xamarin.Essentials.Launcher.OpenAsync(new Uri(mOrder.TrackingLink));
                    }
                    else
                    {
                        Common.DisplayErrorMessage("Invalid tracking URL");
                    }
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("OrderDetailsPage/BtnTrack_Tapped: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private async void BtnConfirmDelivery_Clicked(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnConfirmDeliveryInSubGrid);
                    await ConfirmDelivery();
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("OrderDetailsPage/BtnConfirmDelivery: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private async void BtnShowQrCode_Clicked(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnShowQrCodeInMainGrid);
                    await ShowQRCodeImage();
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("OrderDetailsPage/BtnShowQrCode_Clicked: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private async void CopyString_Tapped(object sender, EventArgs e)
        {
            var stackLayout = (StackLayout)sender;
            if (stackLayout.IsEnabled)
            {
                try
                {
                    stackLayout.IsEnabled = false;
                    if (!Common.EmptyFiels(stackLayout.ClassId))
                    {
                        if (stackLayout.ClassId == "OrderId")
                        {
                            string message = Constraints.CopiedOrderId;
                            Common.CopyText(lblOrderId, message);
                        }
                        else if (stackLayout.ClassId == "RequirementId")
                        {
                            if (!Common.EmptyFiels(mOrder.RequirementId))
                            {
                                string message = Constraints.CopiedRequirementId;
                                Common.CopyText(lblRequirementId, message);
                                await Navigation.PushAsync(new ViewRequirememntPage(mOrder.RequirementId));
                            }
                        }
                        else if (stackLayout.ClassId == "QuoteRefNo")
                        {
                            if (!Common.EmptyFiels(mOrder.QuoteId))
                            {
                                string message = Constraints.CopiedQuoteRefNo;
                                Common.CopyText(lblQuoteRefNo, message);
                                await Navigation.PushAsync(new QuoteDetailsPage(mOrder.QuoteId));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("OrderDetailsPage/CopyString_Tapped: " + ex.Message);
                }
                finally
                {
                    stackLayout.IsEnabled = true;
                }
            }

        }
        #endregion
    }
}