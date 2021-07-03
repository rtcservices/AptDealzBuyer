using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Utility;
using Newtonsoft.Json.Linq;
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
        private string ReqType = string.Empty;
        private string ReqId;
        Requirement mRequirement;
        private List<string> subcaregories;
        private bool sortBy = true;
        private bool isAccepteedQuote = false;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion

        #region Constructor
        public ViewRequirememntPage(string ReqType, string ReqId)
        {
            InitializeComponent();
            this.ReqType = ReqType;
            this.ReqId = ReqId;
            pageNo = 1;
            mRequirement = new Requirement();
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ReqType == "active")
            {
                GetQuotes(sortBy);
                grdPrevReq.IsVisible = false;
                grdActiveReq.IsVisible = true;
            }
            else
            {
                GetQuotes(sortBy);
                grdActiveReq.IsVisible = false;
                grdPrevReq.IsVisible = true;
            }
            GetRequirementsById();
        }

        async void GetQuotes(bool SortBy)
        {
            try
            {
                QuoteAPI quoteAPI = new QuoteAPI();
                UserDialogs.Instance.ShowLoading(Constraints.Loading);

                var mResponse = await quoteAPI.GetQuote(ReqId, "", SortBy, pageNo, pageSize);
                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    var mQuote = result.ToObject<List<Quote>>();
                    var AccepteedQuote = mQuote.Where(x => x.Status == "Accepted");
                    isAccepteedQuote = AccepteedQuote.Count() > 0;

                    if (ReqType == "active")
                    {
                        if (mQuote != null)
                        {
                            lblNoRecord.IsVisible = false;
                            lstQoutes.IsVisible = true;
                            FrmSortBy.IsVisible = true;
                            lstQoutes.ItemsSource = mQuote.ToList();
                            lstQoutes.HeightRequest = mQuote.Count * 100;
                        }
                        else
                        {
                            lblNoRecord.IsVisible = true;
                            if (mResponse.Message != null)
                            {
                                lblNoRecord.Text = mResponse.Message;
                            }
                            FrmSortBy.IsVisible = false;
                            lstQoutes.IsVisible = false;
                            lstQoutes.ItemsSource = null;
                        }
                    }
                    else
                    {
                        if (mQuote != null)
                        {
                            var mAcceptedQuoteList = AccepteedQuote.ToList();

                            if (mAcceptedQuoteList != null && mAcceptedQuoteList.Count > 0)
                            {
                                lblAcceptedQoutesNoRecord.IsVisible = false;
                                lstAcceptedQoutes.IsVisible = true;
                                lstAcceptedQoutes.ItemsSource = mAcceptedQuoteList.ToList();
                                lstAcceptedQoutes.HeightRequest = mAcceptedQuoteList.Count * 100;
                            }
                            else
                            {
                                lblAcceptedQoutesNoRecord.IsVisible = true;
                                if (mResponse.Message != null)
                                {
                                    lblAcceptedQoutesNoRecord.Text = mResponse.Message;
                                }
                                lstAcceptedQoutes.IsVisible = false;
                                lstAcceptedQoutes.ItemsSource = null;
                            }
                        }
                        else
                        {
                            lblAcceptedQoutesNoRecord.IsVisible = true;
                            if (mResponse.Message != null)
                            {
                                lblAcceptedQoutesNoRecord.Text = mResponse.Message;
                            }
                            lstAcceptedQoutes.IsVisible = false;
                            lstAcceptedQoutes.ItemsSource = null;
                        }
                    }

                }
                else
                {
                    if (ReqType == "active")
                    {
                        lblNoRecord.IsVisible = true;
                        if (mResponse.Message != null)
                        {
                            lblNoRecord.Text = mResponse.Message;
                        }
                        FrmSortBy.IsVisible = false;
                        lstQoutes.IsVisible = false;
                        lstQoutes.ItemsSource = null;
                    }
                    else
                    {
                        lblAcceptedQoutesNoRecord.IsVisible = true;
                        if (mResponse.Message != null)
                        {
                            lblAcceptedQoutesNoRecord.Text = mResponse.Message;
                        }
                        lstAcceptedQoutes.IsVisible = false;
                        lstAcceptedQoutes.ItemsSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ViewRequirememntPage/BindQuoteList: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
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
                            GetRequirementDetails();
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

        void GetRequirementDetails()
        {
            if (!Common.EmptyFiels(mRequirement.ProductImage))
            {
                imgProductImage.Source = mRequirement.ProductImage;
            }
            else
            {
                imgProductImage.Source = "iconProductBanner.png";
            }

            lblDescription.Text = mRequirement.ProductDescription;

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
                lblQuantity.Text = mRequirement.Quantity + " " + mRequirement.Unit;
            }
            if (!Common.EmptyFiels(mRequirement.TotalPriceEstimation.ToString()))
            {
                lblEstimatePrice.Text = "Rs " + mRequirement.TotalPriceEstimation.ToString();
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

            //if (!Common.EmptyFiels(mRequirement.Quotes.ToString()))
            //{
            //    lblQuoteNo.Text = mRequirement.Quotes.ToString();
            //}
            //else
            //{
            //    lblQuoteNo.Text = "0 Quote";
            //}

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
            lblBillingAddress.Text = mRequirement.BillingAddressName + "\n" + mRequirement.BillingAddressBuilding + "\n" + mRequirement.BillingAddressStreet + "\n" + mRequirement.BillingAddressCity + "-" + mRequirement.BillingAddressPinCode;
            lblShippingAddress.Text = mRequirement.ShippingAddressName + "\n" + mRequirement.ShippingAddressBuilding + "\n" + mRequirement.ShippingAddressStreet + "\n" + mRequirement.ShippingAddressCity + "-" + mRequirement.ShippingAddressPinCode + "\n" + mRequirement.ShippingAddressLandmark;
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
            Common.BindAnimation(button: FrmCancelRequirement);
            CancelRequirement();
        }

        private void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            //var sortby = new SortByPopup(filterBy, "ViewReq");
            //sortby.isRefresh += (s1, e1) =>
            //{
            //    string result = s1.ToString();
            //    if (!string.IsNullOrEmpty(result))
            //    {
            //        filterBy = result;
            //        BindQuoteList(sortBy);
            //    }
            //};
            //PopupNavigation.Instance.PushAsync(sortby);
            try
            {
                if (ImgSort.Source.ToString().Replace("File: ", "") == Constraints.Sort_ASC)
                {
                    ImgSort.Source = Constraints.Sort_DSC;
                    sortBy = false;
                }
                else
                {
                    ImgSort.Source = Constraints.Sort_ASC;
                    sortBy = true;
                }

                pageNo = 1;
                GetQuotes(sortBy);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ActiveRequirementView/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private void GrdViewQuote_Tapped(object sender, EventArgs e)
        {
            var GridExp = (Grid)sender;
            var mQuote = GridExp.BindingContext as Quote;
            Navigation.PushAsync(new QuoteDetailsPage(mQuote.QuoteId, mQuote.IsSellerContactRevealed, isAccepteedQuote));
        }
        #endregion
    }
}