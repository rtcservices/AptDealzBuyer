using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Utility;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
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
        Quote mQuote;
        private string QuoteId;
        private bool isRevealContact = false;
        private bool isAccepteedQuote = false;
        #endregion

        #region Constructor
        public QuoteDetailsPage(string quoteId, bool isRevealContact, bool isAccepteedQuote)
        {
            InitializeComponent();
            quoteAPI = new QuoteAPI();
            QuoteId = quoteId;
            mQuote = new Quote();
            this.isRevealContact = isRevealContact;
            this.isAccepteedQuote = isAccepteedQuote;
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
                BtnRejectQuote.IsEnabled = false;
            }
            else
            {
                BtnAcceptQuote.IsEnabled = true;
                BtnRejectQuote.IsEnabled = true;
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
                lblUnitPrice.Text = "Rs " + mQuote.UnitPrice.ToString();
                lblCountryOrigin.Text = mQuote.Country;
                lblTotalAmount.Text = Convert.ToString(mQuote.TotalQuoteAmount);
                lblDate.Text = mQuote.ValidityDate.Date.ToString("dd-MM-yyyy");

                if (!Common.EmptyFiels(mQuote.ShippingPinCode))
                {
                    lblShippingPINCode.Text = mQuote.ShippingPinCode;
                }
                if (!Common.EmptyFiels(mQuote.RequestedQuantity.ToString()))
                {
                    lblRequestedQuantity.Text = mQuote.RequestedQuantity.ToString() + " Pieces";
                }
                if (!Common.EmptyFiels(mQuote.NetAmount.ToString()))
                {
                    lblNetAmount.Text = "Rs " + mQuote.NetAmount.ToString();
                }
                if (!Common.EmptyFiels(mQuote.HandlingCharges.ToString()))
                {
                    lblHandlingCharges.Text = "Rs " + mQuote.HandlingCharges.ToString();
                }
                if (!Common.EmptyFiels(mQuote.ShippingCharges.ToString()))
                {
                    lblShippingCharges.Text = "Rs " + mQuote.ShippingCharges.ToString();
                }
                if (!Common.EmptyFiels(mQuote.InsuranceCharges.ToString()))
                {
                    lblInsuranceCharges.Text = "Rs " + mQuote.InsuranceCharges.ToString();
                }

                if (isRevealContact)
                    BtnRevealContact.IsEnabled = false;
                else
                    BtnRevealContact.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailsPage/BindQuoteDetails: " + ex.Message);
            }
        }

        void OrderPayment()
        {
            var contactPopup = new PopupPages.PaymentPopup(mQuote.TotalQuoteAmount.ToString());
            contactPopup.isRefresh += async (s1, e1) =>
            {
                bool isPay = (bool)s1;
                if (isPay)
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
                        Common.DisplayErrorMessage("QuoteDetailsPage/OpenPaymentPopup: " + ex.Message);
                    }
                    finally
                    {
                        UserDialogs.Instance.HideLoading();
                    }
                }
                else
                {
                    Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }
            };
            PopupNavigation.Instance.PushAsync(contactPopup);
        }

        async void AcceptQuote()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await quoteAPI.AcceptQuote(QuoteId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    //Common.DisplaySuccessMessage(mResponse.Message);
                    OrderPayment();
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
            RevealSellerContact();
        }

        private void BtnAcceptQuote_Clicked(object sender, EventArgs e)
        {
            AcceptQuote();
        }

        private void BtnRejectQuote_Clicked(object sender, EventArgs e)
        {
            RejectQuote();
            Navigation.PopAsync();
        }

        private void BtnBackToQuote_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        #endregion
    }
}