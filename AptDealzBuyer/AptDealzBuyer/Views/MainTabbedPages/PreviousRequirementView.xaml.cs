using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model.Request;
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
    public partial class PreviousRequirementView : ContentView
    {
        #region Objects         
        private List<Requirement> mRequirements;
        private string filterBy = "";
        private string title = string.Empty;
        private bool? sortBy = null;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion

        #region Constructor
        public PreviousRequirementView()
        {
            InitializeComponent();
            mRequirements = new List<Requirement>();
            pageNo = 1;
            GetPreviousRequirements(filterBy, title, sortBy, true);
        }
        #endregion

        #region Methods
        public async void GetPreviousRequirements(string FilterBy = "", string Title = "", bool? SortBy = null, bool isLoader = false)
        {
            try
            {
                RequirementAPI requirementAPI = new RequirementAPI();
                if (isLoader)
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                }

                var mResponse = await requirementAPI.GetMyPreviousRequirements(FilterBy, Title, SortBy, pageNo, pageSize);
                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    var requirements = result.ToObject<List<Requirement>>();
                    if (pageNo == 1)
                    {
                        mRequirements.Clear();
                    }

                    foreach (var mRequirement in requirements)
                    {
                        if (mRequirements.Where(x => x.RequirementId == mRequirement.RequirementId).Count() == 0)
                            mRequirements.Add(mRequirement);
                    }
                    BindList(mRequirements);
                }
                else
                {
                    lstRequirements.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                    if (mResponse != null && mResponse.Message != null)
                    {
                        lblNoRecord.Text = mResponse.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PreviousRequirementView/GetRequirements: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        void BindList(List<Requirement> mRequirementList)
        {
            if (mRequirementList != null && mRequirementList.Count > 0)
            {
                lstRequirements.IsVisible = true;
                lblNoRecord.IsVisible = false;
                lstRequirements.ItemsSource = mRequirementList.ToList();
            }
            else
            {
                lstRequirements.IsVisible = false;
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
                GetPreviousRequirements(filterBy, title, sortBy, true);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PreviousRequirementView/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private void ImgExpand_Tapped(object sender, EventArgs e)
        {
            try
            {
                var imgExp = (ImageButton)sender;
                var viewCell = (ViewCell)imgExp.Parent.Parent.Parent;
                if (viewCell != null)
                {
                    viewCell.ForceUpdateSize();
                }
                var mRequirement = imgExp.BindingContext as Requirement;
                if (mRequirement != null && mRequirement.ArrowImage == Constraints.Arrow_Right)
                {
                    mRequirement.ArrowImage = Constraints.Arrow_Down;
                    mRequirement.GridBg = (Color)App.Current.Resources["LightGray"];
                    mRequirement.MoreDetail = true;
                }
                else
                {
                    mRequirement.ArrowImage = Constraints.Arrow_Right;
                    mRequirement.GridBg = Color.Transparent;
                    mRequirement.MoreDetail = false;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PreviousRequirementView/ImgExpand_Tapped: " + ex.Message);
            }
        }

        private void GrdViewPrevRequirement_Tapped(object sender, EventArgs e)
        {
            var GridExp = (Grid)sender;
            var mRequirement = GridExp.BindingContext as Requirement;
            Navigation.PushAsync(new DashboardPages.ViewRequirememntPage("previous", mRequirement.RequirementId));
        }

        private void entrSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pageNo = 1;
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    GetPreviousRequirements(filterBy, entrSearch.Text, sortBy, false);
                }
                else
                {
                    GetPreviousRequirements(filterBy, title, sortBy, true);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PreviousRequirementView/CustomEntry_Unfocused: " + ex.Message);
            }
        }

        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            entrSearch.Text = string.Empty;
            BindList(mRequirements);
        }

        private void lstRequirements_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            try
            {
                if (this.mRequirements.Count < 10)
                    return;
                if (this.mRequirements.Count == 0)
                    return;

                var lastrequirement = this.mRequirements[this.mRequirements.Count - 1];
                var lastAppearing = (Requirement)e.Item;
                if (lastAppearing != null)
                {
                    if (lastrequirement == lastAppearing)
                    {
                        var totalAspectedRow = pageSize * pageNo;
                        pageNo += 1;

                        if (this.mRequirements.Count() >= totalAspectedRow)
                        {
                            GetPreviousRequirements(filterBy, title, sortBy, true);
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
                Common.DisplayErrorMessage("PreviousRequirementView/ItemAppearing: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void lstRequirements_Refreshing(object sender, EventArgs e)
        {
            lstRequirements.IsRefreshing = true;
            pageNo = 1;
            mRequirements.Clear();
            GetPreviousRequirements(filterBy, title, sortBy, true);
            lstRequirements.IsRefreshing = false;
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
                        GetPreviousRequirements(filterBy, title, sortBy, true);
                    }
                };
                PopupNavigation.Instance.PushAsync(sortby);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PreviousRequirementView/CustomEntry_Unfocused: " + ex.Message);
            }
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
        #endregion
    }
}