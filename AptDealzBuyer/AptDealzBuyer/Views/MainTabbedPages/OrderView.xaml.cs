using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.MasterData;
using AptDealzBuyer.Views.PopupPages;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderView : ContentView
    {
        #region Objects       
        public List<Order> mOrders = new List<Order>();
        private string filterBy = "";
        private string title = string.Empty;
        private int? statusBy = null;
        private bool? sortBy = null;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion

        #region Constructor
        public OrderView()
        {
            InitializeComponent();
            pageNo = 1;
            GetOrders(statusBy, filterBy, sortBy);
        }
        #endregion

        #region Methods
        public async void GetOrders(int? StatusBy = null, string FilterBy = "", bool? SortBy = null)
        {
            try
            {
                OrderAPI orderAPI = new OrderAPI();
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await orderAPI.GetOrdersForBuyer(StatusBy, FilterBy, SortBy, pageNo, pageSize);
                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    var orders = result.ToObject<List<Order>>();
                    if (pageNo == 1)
                    {
                        mOrders.Clear();
                    }

                    foreach (var mOrder in orders)
                    {
                        if (mOrders.Where(x => x.OrderId == mOrder.OrderId).Count() == 0)
                            mOrders.Add(mOrder);
                    }
                    BindList(mOrders);
                }
                else
                {
                    lstOrders.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                    lblNoRecord.Text = mResponse.Message;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderView/GetOrders: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        void BindList(List<Order> mOrderList)
        {
            if (mOrderList != null && mOrderList.Count > 0)
            {
                lstOrders.IsVisible = true;
                lblNoRecord.IsVisible = false;
                lstOrders.ItemsSource = mOrderList.ToList();
            }
            else
            {
                lstOrders.IsVisible = false;
                lblNoRecord.IsVisible = true;
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
            App.Current.MainPage = new MasterDataPage();
        }

        private void FrmSortBy_Tapped(object sender, EventArgs e)
        {
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
                GetOrders(statusBy, filterBy, sortBy);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderView/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private async void FrmStatus_Tapped(object sender, EventArgs e)
        {
            try
            {
                StatusPopup statusPopup = new StatusPopup(filterBy, "OrderSupplying");
                statusPopup.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!Common.EmptyFiels(result))
                    {
                        //BindList
                    }
                };
                await PopupNavigation.Instance.PushAsync(statusPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/FrmStatusBy_Tapped: " + ex.Message);
            }
        }

        private void GrdViewOrderDetails_Tapped(object sender, EventArgs e)
        {
            var GridExp = (Grid)sender;
            var mOrder = GridExp.BindingContext as Order;
            if (mOrder.OrderStatusDescr.Contains("Cancelled"))
            {
                Navigation.PushAsync(new Orders.OrderDetailsPage(mOrder.OrderId, true));
            }
            else
            {
                Navigation.PushAsync(new Orders.OrderDetailsPage(mOrder.OrderId));
            }
        }

        private void FrmStatusActions_Tapped(object sender, EventArgs e)
        {

        }

        private void FrmFilterBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                var sortby = new FilterPopup(filterBy, "Active");
                sortby.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!Common.EmptyFiels(result))
                    {
                        filterBy = result;
                        if (filterBy == RequirementSortBy.TotalPriceEstimation.ToString())
                        {
                            lblFilterBy.Text = "Amount";
                        }
                        else
                        {
                            lblFilterBy.Text = filterBy;
                        }
                        pageNo = 1;
                        GetOrders(statusBy, filterBy, sortBy);
                    }
                };
                PopupNavigation.Instance.PushAsync(sortby);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderView/CustomEntry_Unfocused: " + ex.Message);
            }
        }

        private void entrSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    var ReqSearch = mOrders.Where(x => x.OrderNo.ToLower().Contains(entrSearch.Text.ToLower())).ToList();
                    BindList(ReqSearch);
                }
                else
                {
                    BindList(mOrders);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderView/CustomEntry_Unfocused: " + ex.Message);
            }

        }

        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            entrSearch.Text = string.Empty;
            BindList(mOrders);
        }
        #endregion

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private void lstOrders_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            try
            {
                if (this.mOrders.Count < 10)
                    return;
                if (this.mOrders.Count == 0)
                    return;

                var lastrequirement = this.mOrders[this.mOrders.Count - 1];
                var lastAppearing = (Order)e.Item;
                if (lastAppearing != null)
                {
                    if (lastrequirement == lastAppearing)
                    {
                        var totalAspectedRow = pageSize * pageNo;
                        pageNo += 1;

                        if (this.mOrders.Count() >= totalAspectedRow)
                        {
                            GetOrders(statusBy, filterBy, sortBy);
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                    }
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/ItemAppearing: " + ex.Message);
                UserDialogs.Instance.HideLoading();
            }
        }

        private void lstOrders_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lstOrders.IsRefreshing = true;
                pageNo = 1;
                mOrders.Clear();
                GetOrders(statusBy, filterBy, sortBy);
                lstOrders.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/Refreshing: " + ex.Message);
            }
        }
    }
}