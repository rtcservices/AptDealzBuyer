using Acr.UserDialogs;
using AptDealzBuyer.API;
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
    public partial class GrievancesPage : ContentPage
    {
        #region Objects
        public List<Grievance> mGrievance;
        private string filterBy = "";
        private string title = string.Empty;
        private int? statusBy = null;
        private bool? sortBy = null;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion

        #region Constructor
        public GrievancesPage()
        {
            InitializeComponent();
            mGrievance = new List<Grievance>();
            pageNo = 1;
            GetGrievance(statusBy, title, filterBy, sortBy);

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
        private async void GetGrievance(int? StatusBy = null, string Title = "", string FilterBy = "", bool? SortBy = null, bool isLoader = true)
        {
            try
            {
                GrievanceAPI grievanceAPI = new GrievanceAPI();
                if (isLoader)
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                }
                var mResponse = await grievanceAPI.GetAllGrievances(StatusBy, Title, FilterBy, SortBy, pageNo, pageSize);
                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    var orders = result.ToObject<List<Grievance>>();
                    if (pageNo == 1)
                    {
                        mGrievance.Clear();
                    }

                    foreach (var mGrievances in orders)
                    {
                        if (mGrievance.Where(x => x.GrievanceId == mGrievances.GrievanceId).Count() == 0)
                            mGrievance.Add(mGrievances);
                    }
                    BindList(mGrievance);
                }
                else
                {
                    lstGrievance.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievancesPage/GetGrievance: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void BindList(List<Grievance> mGrievanceList)
        {
            try
            {
                if (mGrievanceList != null && mGrievanceList.Count > 0)
                {
                    lstGrievance.IsVisible = true;
                    lblNoRecord.IsVisible = false;
                    lstGrievance.ItemsSource = mGrievanceList.ToList();
                }
                else
                {
                    lstGrievance.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievancesPage/BindList: " + ex.Message);
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
            Common.BindAnimation(imageButton: ImgBack);
            Navigation.PopAsync();
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
        #endregion

        private void FrmAdd_Tapped(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new DashboardPages.RaiseGrievancesPage());
            Navigation.PushAsync(new MainTabbedPages.MainTabbedPage("RaiseGrievances", isNavigate: true));
        }

        private void ImgSearch_Tapped(object sender, EventArgs e)
        {

        }

        #region [ Filtering ]

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
                GetGrievance(statusBy, title, filterBy, sortBy);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievancesPage/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private void FrmStatus_Tapped(object sender, EventArgs e)
        {
            try
            {
                var statusPopup = new StatusPopup(statusBy, "Grievances");
                statusPopup.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!Common.EmptyFiels(result))
                    {
                        lblStatus.Text = result;
                        statusBy = Common.GetGrievanceStatus(result);
                        pageNo = 1;
                        GetGrievance(statusBy, title, filterBy, sortBy);
                    }
                };
                PopupNavigation.Instance.PushAsync(statusPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievancesPage/FrmStatusBy_Tapped: " + ex.Message);
            }
        }

        private void FrmFilterBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                var sortby = new FilterPopup(filterBy, "Grievances");
                sortby.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!Common.EmptyFiels(result))
                    {
                        filterBy = result;
                        lblFilterBy.Text = result;
                        pageNo = 1;
                        GetGrievance(statusBy, title, filterBy, sortBy);
                    }
                };
                PopupNavigation.Instance.PushAsync(sortby);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievancesPage/FrmFilterBy_Tapped: " + ex.Message);
            }
        }

        private void entrSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pageNo = 1;
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    GetGrievance(statusBy, entrSearch.Text, filterBy, sortBy, false);
                }
                else
                {
                    GetGrievance(statusBy, filterBy, title, sortBy);
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
            BindList(mGrievance);
        }
        #endregion

        #region [ Listing ]
        private void GrdViewGrievances_Tapped(object sender, EventArgs e)
        {
            var GridExp = (Grid)sender;
            var mGrievance = GridExp.BindingContext as Grievance;
            Navigation.PushAsync(new GrievanceDetailsPage(mGrievance.GrievanceId));
        }

        private void lstGrievance_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            try
            {
                if (this.mGrievance.Count < 10)
                    return;
                if (this.mGrievance.Count == 0)
                    return;

                var lastrequirement = this.mGrievance[this.mGrievance.Count - 1];
                var lastAppearing = (Grievance)e.Item;
                if (lastAppearing != null)
                {
                    if (lastrequirement == lastAppearing)
                    {
                        var totalAspectedRow = pageSize * pageNo;
                        pageNo += 1;

                        if (this.mGrievance.Count() >= totalAspectedRow)
                        {
                            GetGrievance(statusBy, title, filterBy, sortBy, false);
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
                Common.DisplayErrorMessage("GrievancesPage/ItemAppearing: " + ex.Message);
                UserDialogs.Instance.HideLoading();
            }
        }

        private void lstGrievance_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lstGrievance.IsRefreshing = true;
                pageNo = 1;
                mGrievance.Clear();
                GetGrievance(statusBy, title, filterBy, sortBy);
                lstGrievance.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievancesPage/Refreshing: " + ex.Message);
            }
        }

        private void lstGrievance_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstGrievance.SelectedItem = null;
        }
        #endregion
        #endregion
    }
}