using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.PopupPages;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewRequirememntPage : ContentPage
    {
        #region Objects
        private List<ReceivedQuote> ReceivedQuotes = new List<ReceivedQuote>();
        private string ReqType = string.Empty;
        private string ReqId;
        Requirement mRequirement;
        string filterBy = Utility.RequirementSortBy.quotationId.ToString();
        private List<string> subcaregories;
        #endregion

        #region Constructor
        public ViewRequirememntPage(string ReqType, string ReqId)
        {
            InitializeComponent();
            this.ReqType = ReqType;
            this.ReqId = ReqId;
            mRequirement = new Requirement();
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ReqType == "active")
            {
                BindReceivedQuote();
                grdPrevReq.IsVisible = false;
                grdActiveReq.IsVisible = true;
            }
            else
            {
                grdActiveReq.IsVisible = false;
                grdPrevReq.IsVisible = true;
            }
            GetRequirementsById();
        }

        void BindReceivedQuote()
        {
            lstQoutes.ItemsSource = null;
            ReceivedQuotes = new List<ReceivedQuote>()
            {
                new ReceivedQuote{ QuoteNo="QUO#123", QuotePrice="Rs 2143", Validity="Validity : 5 days"},
                new ReceivedQuote{ QuoteNo="QUO#456", QuotePrice="Rs 2143", Validity="Validity : 4 days"},
                new ReceivedQuote{ QuoteNo="QUO#789", QuotePrice="Rs 2143", Validity="Validity : 2 days"},
                new ReceivedQuote{ QuoteNo="QUO#012", QuotePrice="Rs 2143", Validity="Validity : 6 days"},
                new ReceivedQuote{ QuoteNo="QUO#345", QuotePrice="Rs 2143", Validity="Validity : 3 days"},
                new ReceivedQuote{ QuoteNo="QUO#678", QuotePrice="Rs 2143", Validity="Validity : 7 days"},
                new ReceivedQuote{ QuoteNo="QUO#901", QuotePrice="Rs 2143", Validity="Validity : 9 days"},
                new ReceivedQuote{ QuoteNo="QUO#234", QuotePrice="Rs 2143", Validity="Validity : 8 days"},
            };
            lstQoutes.ItemsSource = ReceivedQuotes.ToList();
            lstQoutes.HeightRequest = 4 * 100;
        }

        public async void GetRequirementsById()
        {
            try
            {
                RequirementAPI requirementAPI = new RequirementAPI();
                UserDialogs.Instance.ShowLoading(Constraints.Loading);

                var mResponse = await requirementAPI.GetRequirementById(ReqId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        mRequirement = jObject.ToObject<Requirement>();
                        if (mRequirement != null)
                        {
                            BindProperties(mRequirement);
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
                Common.DisplayErrorMessage("ViewRequirememntPage/GetRequirementsById: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        void BindProperties(Requirement mRequirement)
        {
            if (!Common.EmptyFiels(mRequirement.ProductImage))
            {
                imgProductImage.Source = App.Current.Resources["BaseURL"].ToString() + mRequirement.ProductImage;
            }
            else
            {
                imgProductImage.Source = "iconProductBanner.png";
            }

            if (!Common.EmptyFiels(mRequirement.Title))
            {
                lblTitle.Text = mRequirement.Title;
            }
            if (!Common.EmptyFiels(mRequirement.RequirementNo))
            {
                lblRequirementId.Text = mRequirement.RequirementNo;
            }
            if (!Common.EmptyFiels(mRequirement.Category))
            {
                lblCategory.Text = mRequirement.Category;
            }
            if (mRequirement.SubCategories != null)
            {
                subcaregories = mRequirement.SubCategories;
                lblSubCategory.Text = string.Join(",", subcaregories);
            }
            if (!Common.EmptyFiels(mRequirement.Quantity.ToString()))
            {
                lblQuantity.Text = mRequirement.Quantity.ToString();
            }
            if (!Common.EmptyFiels(mRequirement.TotalPriceEstimation.ToString()))
            {
                lblEstimatePrice.Text = "RS " + mRequirement.TotalPriceEstimation.ToString();
            }
            if (!Common.EmptyFiels(mRequirement.ExpectedDeliveryDate.ToString()))
            {
                lblDeliveryDate.Text = mRequirement.ExpectedDeliveryDate.Date.ToString("dd.MM.yyyy");
            }
            if (!Common.EmptyFiels(mRequirement.DeliveryLocationPinCode))
            {
                lblLocPinCode.Text = mRequirement.DeliveryLocationPinCode;
            }
            if (!Common.EmptyFiels(mRequirement.PreferredSourceOfSupply))
            {
                lblPreferredSource.Text = mRequirement.PreferredSourceOfSupply;
            }

            if (!Common.EmptyFiels(mRequirement.Quotes.ToString()))
            {
                lblQuoteNo.Text = mRequirement.Quotes.ToString();
            }
            else
            {
                lblQuoteNo.Text = "0 Quote";
            }

            if (mRequirement.NeedInsuranceCoverage)
            {
                lblNeedInsurance.Text = "✓";
            }
            else
            {
                lblNeedInsurance.Text = "✕";
            }

            if (mRequirement.PreferInIndiaProducts)
            {
                lblPreferInIndiaProducts.Text = "✓";
            }
            else
            {
                lblPreferInIndiaProducts.Text = "✕";
            }

            //Address
            lblBillingAddress.Text = mRequirement.BillingAddressName + " " + mRequirement.BillingAddressBuilding + "\n" + mRequirement.BillingAddressStreet + "\n" + mRequirement.BillingAddressCity + "-" + mRequirement.BillingAddressPinCode;
            lblShippingAddress.Text = mRequirement.ShippingAddressName + " " + mRequirement.ShippingAddressBuilding + "\n" + mRequirement.ShippingAddressStreet + "\n" + mRequirement.ShippingAddressCity + "-" + mRequirement.ShippingAddressPinCode + "\n" + mRequirement.ShippingAddressLandmark;
        }

        async void CancelRequirement()
        {
            try
            {
                var isCancel = await App.Current.MainPage.DisplayAlert(Constraints.Alert, Constraints.AreYouSureWantCancel, Constraints.Yes, Constraints.No);
                if (isCancel)
                {
                    RequirementAPI requirementAPI = new RequirementAPI();
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);

                    //var mResponse = await requirementAPI.UpdateStatusRequirement(mRequirement.RequirementId, (int)RequirementStatus.Cancelled);
                    var mResponse = await requirementAPI.CancelRequirement(mRequirement.RequirementId);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        Common.DisplaySuccessMessage(mResponse.Message);
                        await Navigation.PushAsync(new MainTabbedPages.MainTabbedPage("ActiveRequirements"));
                    }
                    else
                    {
                        if (mResponse != null)
                            Common.DisplayErrorMessage(mResponse.Message);
                        else
                            Common.DisplayErrorMessage(Constraints.Something_Wrong);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ViewRequirememntPage/CancelRequirement: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion

        #region Events
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

        private void ImgMenu_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(image: ImgMenu);
            //Common.OpenMenu();
        }

        private void FrmCancelRequirement_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(frame: FrmCancelRequirement);
            CancelRequirement();
        }

        private void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            var sortby = new SortByPopup(filterBy, "ViewReq");
            sortby.isRefresh += (s1, e1) =>
            {
                string result = s1.ToString();
                if (!string.IsNullOrEmpty(result))
                {
                    filterBy = result;
                    //Bind Quotes List
                }
            };
            PopupNavigation.Instance.PushAsync(sortby);
        }

        private void GrdViewQuote_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DashboardPages.QuoteDetailsPage());
        }
        #endregion
    }
}