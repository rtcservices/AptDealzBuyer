using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Utility;
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
    public partial class ShippingDetailsView : ContentView
    {
        #region [ Objects ]        
        public List<Order> mOrders;
        private string filterBy = SortByField.Date.ToString();
        private string title = string.Empty;
        private bool? isAssending = false;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion

        #region [ Constructor ]
        public ShippingDetailsView()
        {
            try
            {
                InitializeComponent(); mOrders = new List<Order>();
                pageNo = 1;
                GetShippedOrders(title, filterBy, isAssending);

                MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                MessagingCenter.Subscribe<string>(this, Constraints.Str_NotificationCount, (count) =>
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
                Common.DisplayErrorMessage("ShippingDetailsView/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]
        private async void GetShippedOrders(string Title = "", string FilterBy = "", bool? SortBy = null, bool isLoader = true)
        {
            try
            {
                OrderAPI orderAPI = new OrderAPI();
                if (isLoader)
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                }
                var mResponse = await orderAPI.GetShippedOrdersForBuyer(Title, FilterBy, SortBy, pageNo, pageSize);
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

                    mOrders.ForEach(x =>
                    {
                        if (x.OrderStatus <= (int)OrderStatus.Shipped && !Common.EmptyFiels(x.TrackingLink) && x.TrackingLink.IsValidURL())
                        {
                            x.OldDetail = true;
                        }
                        else
                        {
                            x.OldDetail = false;
                        }
                    });
                    BindList(mOrders);
                }
                else
                {
                    lstShippingDetails.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                    //lblNoRecord.Text = mResponse.Message;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ShippingDetailsView/GetOrders: " + ex.Message);
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
                    lstShippingDetails.IsVisible = true;
                    lblNoRecord.IsVisible = false;
                    lstShippingDetails.ItemsSource = mOrderList.ToList();
                }
                else
                {
                    lstShippingDetails.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ShippingDetailsView/BindList: " + ex.Message);
            }
        }
        #endregion

        #region [ Events ]

        #region [ Header Navigation ]
        private async void ImgMenu_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(image: ImgMenu);
                await Navigation.PushAsync(new OtherPages.SettingsPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ShippingDetailsView/ImgMenu_Tapped: " + ex.Message);
            }
        }

        private async void ImgNotification_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new DashboardPages.NotificationPage("ShippingDetailsView"));
                //await Navigation.PushAsync(new DashboardPages.NotificationPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ShippingDetailsView/ImgNotification_Tapped: " + ex.Message);
            }
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_FAQHelp));
        }

        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            await Common.BindAnimation(imageButton: ImgBack);
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));

        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));
        }
        #endregion

        #region [ Filteration ]     
        private void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                var ImgASC = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.Sort_ASC : Constraints.Sort_ASC_Dark;
                var ImgDSC = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.Sort_DSC : Constraints.Sort_DSC_Dark;

                if (ImgSort.Source.ToString().Replace("File: ", "") == ImgASC)
                {
                    ImgSort.Source = ImgDSC;
                    isAssending = false;
                }
                else
                {
                    ImgSort.Source = ImgASC;
                    isAssending = true;
                }
                pageNo = 1;
                mOrders.Clear();
                GetShippedOrders(title, filterBy, isAssending);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ShippingDetailsView/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private async void FrmFilterBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                var sortby = new FilterPopup(filterBy, Constraints.Str_Order);
                sortby.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!Common.EmptyFiels(result))
                    {
                        filterBy = result;
                        lblFilterBy.Text = filterBy;
                        pageNo = 1;
                        mOrders.Clear();
                        GetShippedOrders(title, filterBy, isAssending);
                    }
                };
                await PopupNavigation.Instance.PushAsync(sortby);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ShippingDetailsView/FrmFilterBy_Tapped: " + ex.Message);
            }
        }

        private void entrSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pageNo = 1;
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    GetShippedOrders(entrSearch.Text, filterBy, isAssending, false);
                }
                else
                {
                    pageNo = 1;
                    mOrders.Clear();
                    GetShippedOrders(title, filterBy, isAssending);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ShippingDetailsView/entrSearch_TextChanged: " + ex.Message);
            }

        }

        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            entrSearch.Text = string.Empty;
            BindList(mOrders);
        }
        #endregion

        #region [ Listing ]
        private void ImgExpand_Tapped(object sender, EventArgs e)
        {
            try
            {
                var selectGrid = (ImageButton)sender;
                var setHight = (ViewCell)selectGrid.Parent.Parent.Parent;
                if (setHight != null)
                {
                    setHight.ForceUpdateSize();
                }

                var efOrder = (Order)selectGrid.BindingContext;
                if (efOrder != null)
                {
                    foreach (var mOrder in mOrders)
                    {
                        if (mOrder.ArrowImage == Constraints.Arrow_Right)
                        {
                            mOrder.ArrowImage = Constraints.Arrow_Right;
                            mOrder.GridBg = Color.Transparent;
                            mOrder.MoreDetail = false;
                            if (mOrder.OrderStatus <= (int)OrderStatus.Shipped && !Common.EmptyFiels(mOrder.TrackingLink) && mOrder.TrackingLink.IsValidURL())
                            {
                                mOrder.OldDetail = true;
                            }
                            else
                            {
                                mOrder.OldDetail = false;
                            }
                        }
                        else
                        {
                            mOrder.ArrowImage = Constraints.Arrow_Down;
                            mOrder.GridBg = (Application.Current.UserAppTheme == OSAppTheme.Light) ? (Color)App.Current.Resources["appColor8"] : Color.Transparent;
                            mOrder.MoreDetail = true;
                            mOrder.OldDetail = false;
                        }
                    }

                    if (efOrder.ArrowImage == Constraints.Arrow_Right)
                    {
                        efOrder.ArrowImage = Constraints.Arrow_Down;
                        efOrder.GridBg = (Application.Current.UserAppTheme == OSAppTheme.Light) ? (Color)App.Current.Resources["appColor8"] : Color.Transparent;
                        efOrder.MoreDetail = true;
                        efOrder.OldDetail = false;
                    }
                    else
                    {
                        efOrder.ArrowImage = Constraints.Arrow_Right;
                        efOrder.GridBg = Color.Transparent;
                        efOrder.MoreDetail = false;
                        if (efOrder.OrderStatus <= (int)OrderStatus.Shipped && !Common.EmptyFiels(efOrder.TrackingLink) && efOrder.TrackingLink.IsValidURL())
                        {
                            efOrder.OldDetail = true;
                        }
                        else
                        {
                            efOrder.OldDetail = false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CurrentlyShippingPage/ImgExpand_Tapped: " + ex.Message);
            }
        }

        private void BtnTrack_Clicked(object sender, EventArgs e)
        {
            var ButtonExp = (Button)sender;
            try
            {
                var mOrder = ButtonExp.BindingContext as Order;
                if (mOrder != null && !Common.EmptyFiels(mOrder.TrackingLink) && mOrder.TrackingLink.IsValidURL())
                {
                    var trackinglink = "http://" + mOrder.TrackingLink.Replace("http://", "").Replace("https://", "");
                    Xamarin.Essentials.Launcher.OpenAsync(new Uri(trackinglink));
                }
                else
                {
                    Common.DisplayErrorMessage("Invalid tracking URL");
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ShippingDetailsView/BtnTrack_Tapped: " + ex.Message);
            }
        }

        private void lstShippingDetails_ItemAppearing(object sender, ItemVisibilityEventArgs e)
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
                            GetShippedOrders(title, filterBy, isAssending, false);
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
                Common.DisplayErrorMessage("ShippingDetailsView/ItemAppearing: " + ex.Message);
                UserDialogs.Instance.HideLoading();
            }
        }

        private void lstShipingDetails_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lstShippingDetails.IsRefreshing = true;
                pageNo = 1;
                mOrders.Clear();
                GetShippedOrders(title, filterBy, isAssending);
                lstShippingDetails.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ShippingDetailsView/Refreshing: " + ex.Message);
            }
        }

        private async void GrdList_Tapped(object sender, EventArgs e)
        {
            var GridExp = (Grid)sender;
            try
            {
                var mOrder = GridExp.BindingContext as Order;
                await Navigation.PushAsync(new Orders.OrderDetailsPage(mOrder.OrderId));
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderSupplyingview/GrdList_Tapped: " + ex.Message);
            }
        }
        #endregion

        #endregion
    }
}