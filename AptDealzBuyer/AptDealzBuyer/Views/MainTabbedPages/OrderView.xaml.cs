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
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderView : ContentView
    {
        #region Objects       
        public List<Order> mOrders;
        private string filterBy = "";
        private string title = string.Empty;
        private int? statusBy = null;
        private bool? sortBy = null;
        private bool isGrievance = false;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion

        #region Constructor
        public OrderView(bool isGrievance = false)
        {
            InitializeComponent();
            mOrders = new List<Order>();
            pageNo = 1;
            this.isGrievance = isGrievance;
            GetOrders(statusBy, title, filterBy, sortBy);

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
        private async void GetOrders(int? StatusBy = null, string Title = "", string FilterBy = "", bool? SortBy = null, bool isLoader = true)
        {
            try
            {
                OrderAPI orderAPI = new OrderAPI();
                if (isLoader)
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                }
                var mResponse = await orderAPI.GetOrdersForBuyer(StatusBy, Title, FilterBy, SortBy, pageNo, pageSize);
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
                        if (isGrievance)
                        {
                            lblHeader.Text = "Raise Grievances";

                            mOrder.isSelectGrievance = true;
                            mOrder.OrderActionVisibility = false;
                            mOrder.OrderTrackVisibility = false;

                        }
                        else
                        {
                            if (Common.EmptyFiels(mOrder.OrderAction))
                                mOrder.OrderActionVisibility = false;
                            else
                                mOrder.OrderActionVisibility = true;

                            if (mOrder.OrderStatus >= (int)OrderStatus.ReadyForPickup && !Common.EmptyFiels(mOrder.TrackingLink))
                                mOrder.OrderTrackVisibility = true;
                            else
                                mOrder.OrderTrackVisibility = false;
                        }

                        if (mOrders.Where(x => x.OrderId == mOrder.OrderId).Count() == 0)
                            mOrders.Add(mOrder);
                    }
                    BindList(mOrders);
                }
                else
                {
                    lstOrders.IsVisible = false;
                    lblNoRecord.IsVisible = true;
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

        private void BindList(List<Order> mOrderList)
        {
            try
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
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderView/BindList: " + ex.Message);
            }
        }

        #endregion

        #region Events

        #region [ Header Navigation ]
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
            try
            {
                Common.BindAnimation(imageButton: ImgBack);
                if (isGrievance)
                    Navigation.PopAsync();
                else
                    Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderView/ImgBack_Tapped: " + ex.Message);
            }
        }
        #endregion

        #region [ Filteration ]
        private void FrmStatus_Tapped(object sender, EventArgs e)
        {
            try
            {
                var statusPopup = new OrderStatusPopup(statusBy);
                statusPopup.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!Common.EmptyFiels(result))
                    {
                        lblStatus.Text = result.ToCamelCase();
                        statusBy = Common.GetOrderStatus(result);
                        pageNo = 1;
                        GetOrders(statusBy, title, filterBy, sortBy);
                    }
                };
                PopupNavigation.Instance.PushAsync(statusPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderView/FrmStatusBy_Tapped: " + ex.Message);
            }
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
                GetOrders(statusBy, title, filterBy, sortBy);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderView/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private void FrmFilterBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                var sortby = new FilterPopup(filterBy, "Order");
                sortby.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!Common.EmptyFiels(result))
                    {
                        filterBy = result;
                        lblFilterBy.Text = filterBy;
                        pageNo = 1;
                        GetOrders(statusBy, title, filterBy, sortBy);
                    }
                };
                PopupNavigation.Instance.PushAsync(sortby);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderView/CustomEntry_Unfocused: " + ex.Message);
            }
        }
        #endregion

        private void GrdViewOrderDetails_Tapped(object sender, EventArgs e)
        {
            try
            {
                var GridExp = (Grid)sender;
                var mOrder = GridExp.BindingContext as Order;
                Navigation.PushAsync(new Orders.OrderDetailsPage(mOrder.OrderId));
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderView/GrdViewOrderDetails: " + ex.Message);
            }
        }

        private void BtnOrderAction_Tapped(object sender, EventArgs e)
        {
            var ButtonExp = (Button)sender;
            var mOrder = ButtonExp.BindingContext as Order;
            if (mOrder.OrderAction == "Reorder")
            {
                //perform Action
            }
            else if (mOrder.OrderAction == "Show QR Code")
            {

            }
            else
            {
                //perform Action
            }
        }

        private void BtnTrack_Tapped(object sender, EventArgs e)
        {
            try
            {
                var ButtonExp = (Button)sender;
                var mOrder = ButtonExp.BindingContext as Order;
                if (mOrder.TrackingLink.Length > 10)
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
                Common.DisplayErrorMessage("OrderView/BtnTrack_Tapped: " + ex.Message);
            }
        }

        private void entrSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pageNo = 1;
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    GetOrders(statusBy, entrSearch.Text, filterBy, sortBy, false);
                }
                else
                {
                    GetOrders(statusBy, filterBy, title, sortBy);
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
                            GetOrders(statusBy, title, filterBy, sortBy, false);
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
                Common.DisplayErrorMessage("OrderView/ItemAppearing: " + ex.Message);
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
                GetOrders(statusBy, title, filterBy, sortBy);
                lstOrders.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderView/Refreshing: " + ex.Message);
            }
        }

        private void BtnSelect_Tapped(object sender, EventArgs e)
        {
            try
            {
                var ButtonExp = (Button)sender;
                var mOrder = ButtonExp.BindingContext as Order;
                Navigation.PushAsync(new Orders.RaiseGrievancePage(mOrder.OrderId));
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderView/BtnSelect_Tapped: " + ex.Message);
            }
        }
        #endregion
    }
}