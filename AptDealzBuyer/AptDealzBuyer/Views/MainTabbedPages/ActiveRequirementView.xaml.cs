using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
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

    public partial class ActiveRequirementView : ContentView
    {
        #region [ Objects ]      
        private List<Requirement> mRequirements;
        private string filterBy = SortByField.Date.ToString();
        private string title = string.Empty;
        private bool? isAssending = false;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion

        #region [ Constructor ]
        public ActiveRequirementView()
        {
            try
            {
                InitializeComponent();
                mRequirements = new List<Requirement>();
                pageNo = 1;
                mRequirements.Clear();
                GetActiveRequirements(filterBy, title, isAssending);

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
                Common.DisplayErrorMessage("ActiveRequirementView/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]
        private async Task GetActiveRequirements(string FilterBy = "", string Title = "", bool? SortBy = null, bool isLoader = true)
        {
            try
            {
                RequirementAPI requirementAPI = new RequirementAPI();
                if (isLoader)
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                }
                var mResponse = await requirementAPI.GetAllMyActiveRequirements(FilterBy, Title, SortBy, pageNo, pageSize);
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
                Common.DisplayErrorMessage("ActiveRequirementView/GetRequirements: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void BindList(List<Requirement> mRequirementList)
        {
            try
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
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ActiveRequirementView/BindList: " + ex.Message);
            }
        }

        private async Task DeleteRequirement(string requirmentId)
        {
            try
            {
                var isDelete = await DependencyService.Get<IRequirementRepository>().DeleteRequirement(requirmentId);
                if (isDelete)
                {
                    pageNo = 1;
                    mRequirements.Clear();
                    await GetActiveRequirements(filterBy, title, isAssending);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ActiveRequirementView/DeleteRequirement: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
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
                Common.DisplayErrorMessage("ActiveRequirementView/ImgMenu_Tapped: " + ex.Message);
            }
        }

        private async void ImgNotification_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new DashboardPages.NotificationPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ActiveRequirementView/ImgNotification_Tapped: " + ex.Message);
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

        #region [ Filtering ]
        private async void FrmSortBy_Tapped(object sender, EventArgs e)
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
                mRequirements.Clear();
                await GetActiveRequirements(filterBy, title, isAssending);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ActiveRequirementView/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private async void FrmFilterBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                var sortby = new FilterPopup(filterBy, Constraints.Str_Active);
                sortby.isRefresh += async (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!Common.EmptyFiels(result))
                    {
                        filterBy = result;
                        if (filterBy == SortByField.ID.ToString())
                        {
                            lblFilterBy.Text = filterBy;
                        }
                        else
                        {
                            lblFilterBy.Text = filterBy.ToCamelCase();
                        }
                        pageNo = 1;
                        mRequirements.Clear();
                        await GetActiveRequirements(filterBy, title, isAssending);
                    }
                };
                await PopupNavigation.Instance.PushAsync(sortby);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ActiveRequirementView/FrmFilter_Tapped: " + ex.Message);
            }
        }

        private async void entrSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pageNo = 1;
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    await GetActiveRequirements(filterBy, entrSearch.Text, isAssending, false);
                }
                else
                {
                    pageNo = 1;
                    mRequirements.Clear();
                    await GetActiveRequirements(filterBy, title, isAssending);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ActiveRequirementView/entrSearch_TextChanged: " + ex.Message);
            }
        }

        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            entrSearch.Text = string.Empty;
            BindList(mRequirements);
        }
        #endregion

        #region [ Listing ]
        private async void GrdViewRequirement_Tapped(object sender, EventArgs e)
        {
            var GridExp = (Grid)sender;
            try
            {
                var mRequirement = GridExp.BindingContext as Requirement;
                await Navigation.PushAsync(new DashboardPages.ViewRequirememntPage(mRequirement.RequirementId));
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ActiveRequirementView/GrdViewRequirement_Tapped: " + ex.Message);
            }
        }

        private void ImgExpand_Tapped(object sender, EventArgs e)
        {
            try
            {
                var imgExp = (ImageButton)sender;
                var viewCell = (ViewCell)imgExp.Parent.Parent.Parent.Parent.Parent;
                if (viewCell != null)
                {
                    viewCell.ForceUpdateSize();
                }

                var mRequirement = imgExp.BindingContext as Requirement;
                if (mRequirement != null && mRequirement.ArrowImage == Constraints.Arrow_Right)
                {
                    mRequirement.ArrowImage = Constraints.Arrow_Down;
                    mRequirement.GridBg = (Application.Current.UserAppTheme == OSAppTheme.Light) ? (Color)App.Current.Resources["appColor8"] : Color.Transparent;
                    mRequirement.MoreDetail = true;
                    mRequirement.HideDetail = false;
                    mRequirement.NameFont = 15;
                }
                else
                {
                    mRequirement.ArrowImage = Constraints.Arrow_Right;
                    mRequirement.GridBg = Color.Transparent;
                    mRequirement.MoreDetail = false;
                    mRequirement.HideDetail = true;
                    mRequirement.NameFont = 13;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ActiveRequirementView/ImgExpand_Tapped: " + ex.Message);
            }
        }

        private async void ImgDelete_Tapped(object sender, EventArgs e)
        {
            var imgExp = (Image)sender;
            try
            {
                var mRequirement = imgExp.BindingContext as Requirement;
                await DeleteRequirement(mRequirement.RequirementId);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ActiveRequirementView/ImgDelete_Tapped: " + ex.Message);
            }
        }

        private async void FrmDelete_Tapped(object sender, EventArgs e)
        {
            var Tab = (Frame)sender;
            try
            {
                var mRequirement = Tab.BindingContext as Requirement;
                await DeleteRequirement(mRequirement.RequirementId);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ActiveRequirementView/FrmDelete_Tapped: " + ex.Message);
            }
        }

        private async void lstRequirements_ItemAppearing(object sender, ItemVisibilityEventArgs e)
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
                            await GetActiveRequirements(filterBy, title, isAssending, false);
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
                Common.DisplayErrorMessage("ActiveRequirementView/ItemAppearing: " + ex.Message);
                UserDialogs.Instance.HideLoading();
            }
        }

        private async void lstRequirements_Refreshing(object sender, EventArgs e)
        {
            lstRequirements.IsRefreshing = true;
            pageNo = 1;
            mRequirements.Clear();
            await GetActiveRequirements(filterBy, title, isAssending);
            lstRequirements.IsRefreshing = false;
        }
        private void lstRequirements_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstRequirements.SelectedItem = null;
        }
        #endregion

        #endregion

    }
}