using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Utility;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuoteDetailsPage : ContentPage
    {
        #region Objects
        private QuoteAPI quoteAPI;
        private Quote mQuote;
        private string QuoteId;
        private bool isRevealContact = false;
        private bool isAccepteedQuote = false;
        private bool isPickupProductDirectly = false;
        #endregion

        #region Constructor
        public QuoteDetailsPage(string quoteId, bool isRevealContact, bool isAccepteedQuote, bool isPickupProductDirectly)
        {
            InitializeComponent();
            quoteAPI = new QuoteAPI();
            QuoteId = quoteId;
            mQuote = new Quote();
            this.isRevealContact = isRevealContact;
            this.isAccepteedQuote = isAccepteedQuote;
            this.isPickupProductDirectly = isPickupProductDirectly;
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetQuoteById();
            if (isAccepteedQuote)
            {
                BtnAcceptQuote.IsEnabled = false;
                //BtnRejectQuote.IsEnabled = false;
            }
            else
            {
                BtnAcceptQuote.IsEnabled = true;
                //BtnRejectQuote.IsEnabled = true;
            }

            if (isPickupProductDirectly)
            {
                lblShippingPinCode.Text = "Product Pickup PIN Code";
            }
            else
            {
                lblShippingPinCode.Text = "Shipping PIN Code";
            }
        }

        async void GetQuoteById()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await quoteAPI.GetQuoteById(QuoteId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        mQuote = jObject.ToObject<Quote>();
                        if (mQuote != null)
                        {
                            BindQuoteDetails();
                        }
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
                Common.DisplayErrorMessage("QuoteDetailsPage/GetQuoteById: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        void BindQuoteDetails()
        {
            try
            {
                lblRequirementId.Text = mQuote.RequirementNo;
                lblQuoteRefNo.Text = mQuote.QuoteNo;
                lblBuyerId.Text = mQuote.BuyerId;
                lblSellerId.Text = mQuote.SellerId;
                lblUnitPrice.Text = "Rs " + mQuote.UnitPrice;
                lblCountryOrigin.Text = mQuote.Country;
                lblTotalAmount.Text = "Rs " + mQuote.TotalQuoteAmount;


                if (!Common.EmptyFiels(mQuote.ShippingPinCode))
                {
                    lblShippingPINCode.Text = mQuote.ShippingPinCode;
                }
                if (!Common.EmptyFiels(mQuote.RequestedQuantity.ToString()))
                {
                    lblRequestedQuantity.Text = mQuote.RequestedQuantity + " " + mQuote.Unit;
                }
                if (!Common.EmptyFiels(mQuote.NetAmount.ToString()))
                {
                    lblNetAmount.Text = "Rs " + mQuote.NetAmount;
                }
                if (!Common.EmptyFiels(mQuote.HandlingCharges.ToString()))
                {
                    lblHandlingCharges.Text = "Rs " + mQuote.HandlingCharges;
                }
                if (!Common.EmptyFiels(mQuote.ShippingCharges.ToString()))
                {
                    lblShippingCharges.Text = "Rs " + mQuote.ShippingCharges;
                }
                if (!Common.EmptyFiels(mQuote.InsuranceCharges.ToString()))
                {
                    lblInsuranceCharges.Text = "Rs " + mQuote.InsuranceCharges;
                }
                if (!Common.EmptyFiels(mQuote.Comments))
                {
                    lblComments.Text = mQuote.Comments;
                }

                if (isRevealContact)
                    BtnRevealContact.IsEnabled = false;
                else
                    BtnRevealContact.IsEnabled = true;

                if (mQuote.Days.Contains("Expired"))
                {
                    BtnAcceptQuote.IsEnabled = false;
                    lblDate.Text = mQuote.ValidityDate.Date.ToString("dd/MM/yyyy") + " ( Expired )";
                    //BtnRejectQuote.IsEnabled = true;
                }
                else
                {
                    lblDate.Text = mQuote.ValidityDate.Date.ToString("dd/MM/yyyy");
                    //BtnRejectQuote.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailsPage/BindQuoteDetails: " + ex.Message);
            }
        }

        void QuotePayment()
        {
            var contactPopup = new PopupPages.PaymentPopup(mQuote.TotalQuoteAmount.ToString());
            contactPopup.isRefresh += (s1, e1) =>
           {
               bool isPay = (bool)s1;
               if (isPay)
               {
                   //if (DeviceInfo.Platform == DevicePlatform.Android)
                   //{
                   //    OpenRazorPayQuote();
                   //}
                   //else
                   //{
                   PaidQuote();
                   //}
               }
               else
               {
                   isAccepteedQuote = false;
               }
           };
            PopupNavigation.Instance.PushAsync(contactPopup);
        }

        void OpenRazorPayQuote()
        {
            try
            {
                RazorPayload payload = new RazorPayload();
                payload.amount = Convert.ToInt64(mQuote.TotalQuoteAmount * 100);
                //payload.amount = 2000;
                payload.currency = (string)App.Current.Resources["Currency"];
                payload.receipt = mQuote.QuoteId;

                MessagingCenter.Send<RazorPayload>(payload, "PayNow");

                MessagingCenter.Subscribe<RazorResponse>(this, "PaidResponse", async (razorResponse) =>
                {
                    if (razorResponse != null && razorResponse.isPaid)
                    {
                        PaidQuote();
                    }
                });
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailsPage/OpenPaymentPopup: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        async void PaidQuote()
        {
            try
            {
                CreateOrder mCreateOrder = new CreateOrder();
                mCreateOrder.QuoteId = mQuote.QuoteId;
                mCreateOrder.PaidAmount = mQuote.TotalQuoteAmount;
                mCreateOrder.PaymentStatus = (int)Utility.PaymentStatus.Success;

                OrderAPI orderAPI = new OrderAPI();
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await orderAPI.CreateOrder(mCreateOrder);
                if (mResponse != null && mResponse.Succeeded)
                {
                    await AcceptQuote(mResponse.Message);
                    OnAppearing();
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
                Common.DisplayErrorMessage("QuoteDetailsPage/PaidQuote: " + ex.Message);
            }
        }


        async Task AcceptQuote(string SuucessfullPaymentMsg)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await quoteAPI.AcceptQuote(QuoteId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    Common.DisplaySuccessMessage(SuucessfullPaymentMsg);
                    isAccepteedQuote = true;
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
                Common.DisplayErrorMessage("QuoteDetailsPage/AcceptQuote: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        async void RejectQuote()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await quoteAPI.RejectQuote(QuoteId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    Common.DisplaySuccessMessage(mResponse.Message);
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
                Common.DisplayErrorMessage("QuoteDetailsPage/RejectQuote: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        async void RevealSellerContact()
        {
            try
            {
                if (BtnRevealContact.Text == "Reveal Contact")
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    var mResponse = await quoteAPI.RevealSellerContact(QuoteId, (int)PaymentStatus.Failed);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        var jObject = (JObject)mResponse.Data;
                        if (jObject != null)
                        {
                            var mSellerContact = jObject.ToObject<RevealSellerContact>();
                            if (mSellerContact != null)
                            {
                                var successPopup = new PopupPages.SuccessPopup(Constraints.ContactRevealed);
                                await PopupNavigation.Instance.PushAsync(successPopup);

                                BtnRevealContact.Text = mSellerContact.PhoneNumber;
                            }
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
                else
                {
                    PhoneDialer.Open(BtnRevealContact.Text);
                }
            }
            catch (ArgumentNullException)
            {
                Common.DisplayErrorMessage(Constraints.Number_was_null);
            }
            catch (FeatureNotSupportedException)
            {
                Common.DisplayErrorMessage(Constraints.Phone_Dialer_Not_Support);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailsPage/RevealSellerContact: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
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
            Common.BindAnimation(imageButton: ImgBack);
            Navigation.PopAsync();
        }

        private void BtnRevealContact_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnRevealContact);
            RevealSellerContact();
        }

        private void BtnAcceptQuote_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnAcceptQuote);
            QuotePayment();
        }

        //private void BtnRejectQuote_Clicked(object sender, EventArgs e)
        //{
        //    Common.BindAnimation(button: BtnRejectQuote);
        //    RejectQuote();
        //    Navigation.PopAsync();
        //}

        private void BtnBackToQuote_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnBackToQuote);
            Navigation.PopAsync();
        }
        #endregion

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
    }
}