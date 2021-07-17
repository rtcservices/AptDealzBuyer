using AptDealzBuyer.Utility;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPopup : PopupPage
    {
        #region Objects       
        public event EventHandler isRefresh;
        private string PageName;
        #endregion

        #region Constructor        
        public FilterPopup(string SortBy, string SortPageName)
        {
            InitializeComponent();
            PageName = SortPageName;
            BindSource(SortBy);
        }
        #endregion

        #region Methos

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindLabel();
        }

        void BindLabel()
        {
            if (PageName == "Active")
            {
                lblFirstType.Text = "ID";
                lblSecondType.Text = "Date";
                lblThirdType.Text = "No of Quotes";
                lblFourType.Text = "Amount";
            }
            else
            {
                lblFirstType.Text = "ID";
                lblSecondType.Text = "Date";
                lblThirdType.Text = "No of Quotes";
                lblFourType.Text = "Amount";
            }
        }

        void BindSource(string viewSource)
        {
            if (!string.IsNullOrEmpty(viewSource))
            {
                if (viewSource == RequirementSortBy.ID.ToString())
                {
                    ClearSource();
                    imgFirstType.Source = Constraints.Radio_Selected;
                }
                else if (viewSource == RequirementSortBy.Date.ToString())
                {
                    ClearSource();
                    imgSecondType.Source = Constraints.Radio_Selected;
                }
                else if (viewSource == RequirementSortBy.Quotes.ToString())
                {
                    ClearSource();
                    imgThirdType.Source = Constraints.Radio_Selected;
                }
                else if (viewSource == RequirementSortBy.TotalPriceEstimation.ToString())
                {
                    ClearSource();
                    imgFourType.Source = Constraints.Radio_Selected;
                }
                else
                {
                    ClearSource();
                    imgFirstType.Source = Constraints.Radio_Selected;
                }
            }
        }

        void ClearSource()
        {
            imgFirstType.Source = Constraints.Redio_UnSelected;
            imgSecondType.Source = Constraints.Redio_UnSelected;
            imgThirdType.Source = Constraints.Redio_UnSelected;
            imgFourType.Source = Constraints.Redio_UnSelected;
        }
        #endregion

        #region Events
        private void StkFirstType_Tapped(object sender, EventArgs e)
        {
            BindSource(RequirementSortBy.ID.ToString());
            isRefresh?.Invoke(RequirementSortBy.ID.ToString(), null);
            PopupNavigation.Instance.PopAsync();
        }

        private void StkSecondType_Tapped(object sender, EventArgs e)
        {
            BindSource(RequirementSortBy.Date.ToString());
            isRefresh?.Invoke(RequirementSortBy.Date.ToString(), null);
            PopupNavigation.Instance.PopAsync();
        }

        private void StkThirdType_Tapped(object sender, EventArgs e)
        {
            BindSource(RequirementSortBy.Quotes.ToString());
            isRefresh?.Invoke(RequirementSortBy.Quotes.ToString(), null);
            PopupNavigation.Instance.PopAsync();
        }

        private void StkFourType_Tapped(object sender, EventArgs e)
        {
            BindSource(RequirementSortBy.TotalPriceEstimation.ToString());
            isRefresh?.Invoke(RequirementSortBy.TotalPriceEstimation.ToString(), null);
            PopupNavigation.Instance.PopAsync();
        }
        #endregion
    }
}