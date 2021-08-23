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

namespace AptDealzBuyer.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewRequirememntPage : ContentPage
    {
        #region [ Objects ]
        private string ReqType = string.Empty;
        private string RequirementId;

        Requirement mRequirement;
        private List<string> subcaregories;
        private List<Quote> mQuoteList;
        #endregion

        #region [ Constructor ]
        public ViewRequirememntPage(string RequirementId, string ReqType = "active")
        {
            try
            {
                InitializeComponent();
                this.ReqType = ReqType;
                this.RequirementId = RequirementId;
                mRequirement = new Requirement();
                mQuoteList = new List<Quote>();

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
                Common.DisplayErrorMessage("ViewRequirememntPage/Ctor: " + ex.Message);
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
            try
            {
                base.OnAppearing();
                if (ReqType == "active")
                {
                    grdPrevReq.IsVisible = false;
                    grdActiveReq.IsVisible = true;
                }
                else
                {
                    grdActiveReq.IsVisible = false;
                    grdPrevReq.IsVisible = true;
                }

                GetRequirementDetails();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ViewRequirememntPage/OnAppearing: " + ex.Message);
            }
        }

        private async Task GetRequirementDetails()
        {
            try
            {
                mRequirement = await DependencyService.Get<IRequirementRepository>().GetRequirementById(RequirementId);
                if (mRequirement != null)
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
                    if (mRequirement.ExpectedDeliveryDate != null && mRequirement.ExpectedDeliveryDate != DateTime.MinValue)
                    {
                        lblDeliveryDateValue.Text = mRequirement.ExpectedDeliveryDate.ToString("dd/MM/yyyy");
                    }
                    if (!Common.EmptyFiels(mRequirement.DeliveryLocationPinCode))
                    {
                        lblLocPinCode.Text = mRequirement.DeliveryLocationPinCode;
                    }
                    if (!Common.EmptyFiels((string)mRequirement.PreferredSourceOfSupply))
                    {
                        lblPreferredSource.Text = (string)mRequirement.PreferredSourceOfSupply;
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

                    if (mRequirement.PickupProductDirectly)
                    {
                        lblDeliveryDate.Text = "Expected Pickup Date";
                        lblPreferSeller.Text = "✓";
                    }
                    else
                    {
                        lblDeliveryDate.Text = "Expected Delivery Date";
                        lblPreferSeller.Text = "✕";
                    }

                    //List of Quote
                    mQuoteList = mRequirement.ReceivedQuotes;
                    GetListOfQuotes();

                    #region [ Address ]
                    lblBillingAddress.Text = mRequirement.BillingAddressName + "\n"
                               + mRequirement.BillingAddressBuilding + "\n"
                               + mRequirement.BillingAddressStreet + "\n"
                               + mRequirement.BillingAddressCity + "-"
                               + mRequirement.BillingAddressPinCode;

                    if (!Common.EmptyFiels(mRequirement.ShippingAddressName) || !Common.EmptyFiels(mRequirement.ShippingAddressBuilding)
                      || !Common.EmptyFiels(mRequirement.ShippingAddressStreet) || !Common.EmptyFiels(mRequirement.ShippingAddressCity)
                      || !Common.EmptyFiels(mRequirement.ShippingAddressPinCode) || !Common.EmptyFiels(mRequirement.ShippingAddressLandmark)
                      )
                    {
                        lblShippingAddress.Text = mRequirement.ShippingAddressName + "\n"
                            + mRequirement.ShippingAddressBuilding + "\n"
                            + mRequirement.ShippingAddressStreet + "\n"
                            + mRequirement.ShippingAddressCity + "-"
                            + mRequirement.ShippingAddressPinCode + "\n"
                            + mRequirement.ShippingAddressLandmark;
                        GrdShippingAddress.IsVisible = true;
                    }
                    else
                    {
                        GrdShippingAddress.IsVisible = false;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ViewRequirememntPage/GetRequirementDetails: " + ex.Message);
            }
        }

        private void GetListOfQuotes()
        {
            try
            {
                if (mQuoteList != null && mQuoteList.Count > 0)
                {
                    var AccepteedQuote = mQuoteList.Where(x => x.Status == "Accepted");
                    if (ReqType == "active")
                    {
                        lblNoRecord.IsVisible = false;
                        lstQoutes.IsVisible = true;
                        FrmSortBy.IsVisible = true;
                        lstQoutes.ItemsSource = mQuoteList.OrderByDescending(x => x.QuoteId).ToList();
                        lstQoutes.HeightRequest = mQuoteList.Count * 100;
                    }
                    else
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
                            lstAcceptedQoutes.IsVisible = false;
                            lstAcceptedQoutes.ItemsSource = null;
                            lstAcceptedQoutes.HeightRequest = 50;
                        }
                    }
                }
                else
                {
                    if (ReqType == "active")
                    {
                        lblNoRecord.IsVisible = true;
                        FrmSortBy.IsVisible = false;
                        lstQoutes.IsVisible = false;
                        lstQoutes.ItemsSource = null;
                        lstQoutes.HeightRequest = 50;
                    }
                    else
                    {
                        lblAcceptedQoutesNoRecord.IsVisible = true;
                        lstAcceptedQoutes.IsVisible = false;
                        lstAcceptedQoutes.ItemsSource = null;
                        lstAcceptedQoutes.HeightRequest = 50;

                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ViewRequirememntPage/GetListOfQuotes: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void QuoteListSorting()
        {
            try
            {
                if (ImgSort.Source.ToString().Replace("File: ", "") == Constraints.Sort_ASC)
                {
                    ImgSort.Source = Constraints.Sort_DSC;
                    lstQoutes.ItemsSource = mQuoteList.OrderByDescending(x => x.QuoteId).ToList();
                }
                else
                {
                    ImgSort.Source = Constraints.Sort_ASC;
                    lstQoutes.ItemsSource = mQuoteList.OrderBy(x => x.QuoteId).ToList();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ViewRequirememntPage/QuotesListSorting: " + ex.Message);
            }
        }

        private async Task DeleteRequirement(string requirmentId)
        {
            try
            {
                var isDelete = await DependencyService.Get<IRequirementRepository>().DeleteRequirement(requirmentId);
                if (isDelete)
                {
                    Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("ActiveRequirements", isNavigate: true));
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ViewRequirememntPage/DeleteRequirement: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async void CancelRequirement()
        {
            try
            {
                var isCancel = await App.Current.MainPage.DisplayAlert(Constraints.Alert, Constraints.AreYouSureWantCancelReq, Constraints.Yes, Constraints.No);
                if (isCancel)
                {
                    RequirementAPI requirementAPI = new RequirementAPI();
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);

                    var mResponse = await requirementAPI.CancelRequirement(mRequirement.RequirementId);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        Common.DisplaySuccessMessage(mResponse.Message);
                        Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("ActiveRequirements", isNavigate: true));
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

        #region [ Events ]
        #region [ Header Navigation ]
        private async void ImgNotification_Tapped(object sender, EventArgs e)
        {
            var Tab = (Grid)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    await Navigation.PushAsync(new NotificationPage());
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("ViewRequirememntPage/ImgNotification_Tapped: " + ex.Message);
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

        private void ImgMenu_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(image: ImgMenu);
            //Common.OpenMenu();
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
        #endregion

        private async void BtnDeleteRequirement_Tapped(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnDeleteRequirement);
                    await DeleteRequirement(RequirementId);
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("ViewRequirememntPage/BtnDeleteRequirement: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }

        }

        private void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            QuoteListSorting();
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            try
            {
                rfView.IsRefreshing = true;
                if (ReqType == "active")
                {
                    grdPrevReq.IsVisible = false;
                    grdActiveReq.IsVisible = true;
                }
                else
                {
                    grdActiveReq.IsVisible = false;
                    grdPrevReq.IsVisible = true;
                }

                await GetRequirementDetails();
                rfView.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ViewRequirememntPage/RefreshView_Refreshing: " + ex.Message);
            }
        }

        private async void GrdViewQuote_Tapped(object sender, EventArgs e)
        {
            var GridTab = (Grid)sender;
            if (GridTab.IsEnabled)
            {
                try
                {
                    GridTab.IsEnabled = false;
                    var mQuote = GridTab.BindingContext as Quote;
                    await Navigation.PushAsync(new QuoteDetailsPage(mQuote.QuoteId));
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("ViewRequirememntPage/GrdViewQuote_Tapped: " + ex.Message);
                }
                finally
                {
                    GridTab.IsEnabled = true;
                }
            }
        }

        private void CopyString_Tapped(object sender, EventArgs e)
        {
            try
            {
                string message = Constraints.CopiedRequirementId;
                Common.CopyText(lblRequirementId, message);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ViewRequirememntPage/CopyString_Tapped: " + ex.Message);
            }
        }


        //private void BtnCancelRequirement_Tapped(object sender, EventArgs e)
        //{
        //    Common.BindAnimation(button: BtnCancelRequirement);
        //    CancelRequirement();
        //}
        #endregion

        private void lstQoutes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstQoutes.SelectedItem = null;
        }

        private void lstAcceptedQoutes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstAcceptedQoutes.SelectedItem = null;
        }
    }
}