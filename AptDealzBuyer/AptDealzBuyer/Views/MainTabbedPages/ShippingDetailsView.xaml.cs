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
    public partial class ShippingDetailsView : ContentView
    {
        #region Objects       
        public List<Order> mOrders;
        private string filterBy = "";
        private string title = string.Empty;
        private bool? sortBy = null;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion

        #region Constructor
        public ShippingDetailsView()
        {
            InitializeComponent(); mOrders = new List<Order>();
            pageNo = 1;
            GetShippedOrders(title, filterBy, sortBy);

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

        #region Events
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

                var response = (Order)selectGrid.BindingContext;
                if (response != null)
                {
                    foreach (var selectedImage in mOrders)
                    {
                        if (selectedImage.ArrowImage == Constraints.Arrow_Right)
                        {
                            selectedImage.ArrowImage = Constraints.Arrow_Right;
                            selectedImage.GridBg = Color.Transparent;
                            selectedImage.MoreDetail = false;
                            selectedImage.OldDetail = true;
                        }
                        else
                        {
                            selectedImage.ArrowImage = Constraints.Arrow_Down;
                            selectedImage.GridBg = (Color)App.Current.Resources["LightGray"];
                            selectedImage.MoreDetail = true;
                            selectedImage.OldDetail = false;
                        }
                    }
                    if (response.ArrowImage == Constraints.Arrow_Right)
                    {
                        response.ArrowImage = Constraints.Arrow_Down;
                        response.GridBg = (Color)App.Current.Resources["LightGray"];
                        response.MoreDetail = true;
                        response.OldDetail = false;
                    }
                    else
                    {
                        response.ArrowImage = Constraints.Arrow_Right;
                        response.GridBg = Color.Transparent;
                        response.MoreDetail = false;
                        response.OldDetail = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CurrentlyShippingPage/ImgExpand_Tapped: " + ex.Message);
            }
        }

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
            Common.BindAnimation(imageButton: ImgBack);
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));

        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
        #endregion

        #region [ Filteration ]     
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
                GetShippedOrders(title, filterBy, sortBy);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ShippingDetailsView/FrmSortBy_Tapped: " + ex.Message);
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
                        GetShippedOrders(title, filterBy, sortBy);
                    }
                };
                PopupNavigation.Instance.PushAsync(sortby);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ShippingDetailsView/CustomEntry_Unfocused: " + ex.Message);
            }
        }

        private void entrSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pageNo = 1;
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    GetShippedOrders(entrSearch.Text, filterBy, sortBy, false);
                }
                else
                {
                    GetShippedOrders(filterBy, title, sortBy);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ShippingDetailsView/CustomEntry_Unfocused: " + ex.Message);
            }

        }

        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            entrSearch.Text = string.Empty;
            BindList(mOrders);
        }
        #endregion

        #region [ Listing ]
        private void BtnTrack_Clicked(object sender, EventArgs e)
        {
            try
            {
                var ButtonExp = (Button)sender;
                var mOrder = ButtonExp.BindingContext as Order;
                if (mOrder.TrackingLink != null && mOrder.TrackingLink.Length > 10)
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
                Common.DisplayErrorMessage("ShippingDetailsView/BtnTrack_Tapped: " + ex.Message);
            }
        }

        private void lstShipingDetails_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstShippingDetails.SelectedItem = null;
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
                            GetShippedOrders(title, filterBy, sortBy, false);
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
                GetShippedOrders(title, filterBy, sortBy);
                lstShippingDetails.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ShippingDetailsView/Refreshing: " + ex.Message);
            }

        }

        private void GrdList_Tapped(object sender, EventArgs e)
        {
            try
            {
                var GridExp = (Grid)sender;
                var mOrder = GridExp.BindingContext as Order;
                Navigation.PushAsync(new Orders.OrderDetailsPage(mOrder.OrderId));
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