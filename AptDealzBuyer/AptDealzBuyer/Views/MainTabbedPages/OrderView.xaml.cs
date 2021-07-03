using AptDealzBuyer.Model;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.MasterData;
using AptDealzBuyer.Views.PopupPages;
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
        public List<OrderM> mOrders = new List<OrderM>();
        private string filterBy = Utility.RequirementSortBy.ID.ToString();
        private bool sortBy = true;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion

        #region Constructor
        public OrderView()
        {
            InitializeComponent();
            GetOrders();
        }
        #endregion

        #region Methods
        // write methods here
        public void GetOrders()
        {
            lstOrders.ItemsSource = null;
            mOrders = new List<OrderM>()
            {
                new OrderM{ OrdNo="INV#123", OrdPrice="Rs 2143", OrdStatus="Shipped", OrdDate="12-05-2021"},
                new OrderM{ OrdNo="INV#456", OrdPrice="Rs 2143", OrdStatus="Accepted", OrdDate="12-05-2021"},
                new OrderM{ OrdNo="INV#789", OrdPrice="Rs 2143", OrdStatus="Completed", OrdDate="12-05-2021"},
                new OrderM{ OrdNo="INV#012", OrdPrice="Rs 2143", OrdStatus="Ready for pickup", OrdDate="12-05-2021"},
                new OrderM{ OrdNo="INV#345", OrdPrice="Rs 2143", OrdStatus="Shipped", OrdDate="12-05-2021"},
                new OrderM{ OrdNo="INV#678", OrdPrice="Rs 2143", OrdStatus="Accepted", OrdDate="12-05-2021"},
                new OrderM{ OrdNo="INV#901", OrdPrice="Rs 2143", OrdStatus="Completed", OrdDate="12-05-2021"},
                new OrderM{ OrdNo="INV#234", OrdPrice="Rs 2143", OrdStatus="Shipped", OrdDate="12-05-2021"},
            };
            lstOrders.ItemsSource = mOrders.ToList();
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

        private void ImgSearch_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new PopupPages.SearchPopup());
        }

        private void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            //var sortby = new PrevReqPopup("Sort By", "ID", "Quotes");
            //sortby.isRefresh += (s1, e1) =>
            //{
            //    //get result from popup
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
                //GetOrders(filterBy, sortBy);
                GetOrders();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderView/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private void FrmStatus_Tapped(object sender, EventArgs e)
        {
            var sortby = new PrevReqPopup("Status", "All", "Date");
            sortby.isRefresh += (s1, e1) =>
            {
                //get result from popup
            };
            PopupNavigation.Instance.PushAsync(sortby);
        }

        private void GrdViewOrderDetails_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Orders.OrderDetailsPage());
        }

        private void FrmStatusActions_Tapped(object sender, EventArgs e)
        {

        }

        private void FrmFilterBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                var sortby = new SortByPopup(filterBy, "Active");
                sortby.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!string.IsNullOrEmpty(result))
                    {
                        pageNo = 1;
                        filterBy = result;
                        GetOrders();
                        //GetOrders(filterBy, sortBy);
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
                    var ReqSearch = mOrders.Where(x =>
                                                        x.OrdNo.ToLower().Contains(entrSearch.Text.ToLower())).ToList();
                    if (ReqSearch != null && ReqSearch.Count > 0)
                    {
                        lstOrders.IsVisible = true;
                        FrmFilterBy.IsVisible = true;
                        FrmSortBy.IsVisible = true;
                        //lblNoRecord.IsVisible = false;
                        lstOrders.ItemsSource = ReqSearch.ToList();
                    }
                    else
                    {
                        lstOrders.IsVisible = false;
                        FrmFilterBy.IsVisible = false;
                        FrmSortBy.IsVisible = false;
                        lstOrders.IsVisible = true;
                    }
                }
                else
                {
                    GetOrders();
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
            //BindList();
        }
        #endregion
    }
}