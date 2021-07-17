using AptDealzBuyer.Utility;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatusPopup : PopupPage
    {
        #region Objects
        public event EventHandler isRefresh;
        private string PageName;
        #endregion

        #region Constructor
        public StatusPopup(string SortBy, string SortPageName)
        {
            InitializeComponent();
            PageName = SortPageName;
            BindSource(SortBy);
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindLabel();
        }

        void BindLabel()
        {
            if (PageName == "OrderSupplying")
            {
                StkThirdType.IsVisible = true;
                lblFirstType.Text = "All";
                lblSecondType.Text = "Accepted";
                lblThirdType.Text = "Submitted";
            }
            else if (PageName == "Previous")
            {
                StkThirdType.IsVisible = false;
                lblFirstType.Text = "ID";
                lblSecondType.Text = "Quotes";
            }
            else if (PageName == "ViewReq")
            {
                StkThirdType.IsVisible = true;
                lblFirstType.Text = "ID";
                lblSecondType.Text = "Amount";
                lblThirdType.Text = "Validity";
            }
            else
            {
                StkThirdType.IsVisible = true;
                lblFirstType.Text = "All";
                lblSecondType.Text = "Accepted";
                lblThirdType.Text = "Submitted";
            }
        }

        void BindSource(string viewSource)
        {
            if (!string.IsNullOrEmpty(viewSource))
            {
                if (viewSource == QuoteStatus.All.ToString())
                {
                    ClearSource();
                    imgFirstType.Source = Constraints.Radio_Selected;
                }
                else if (viewSource == QuoteStatus.Accepted.ToString())
                {
                    ClearSource();
                    imgSecondType.Source = Constraints.Radio_Selected;
                }
                else if (viewSource == QuoteStatus.Submitted.ToString())
                {
                    ClearSource();
                    imgThirdType.Source = Constraints.Radio_Selected;
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
        }
        #endregion

        #region Events
        private void StkFirstType_Tapped(object sender, EventArgs e)
        {

            BindSource(QuoteStatus.All.ToString());
            isRefresh?.Invoke(QuoteStatus.All.ToString(), null);

            PopupNavigation.Instance.PopAsync();
        }

        private void StkSecondType_Tapped(object sender, EventArgs e)
        {

            BindSource(QuoteStatus.Accepted.ToString());
            isRefresh?.Invoke(QuoteStatus.Accepted.ToString(), null);

            PopupNavigation.Instance.PopAsync();
        }

        private void StkThirdType_Tapped(object sender, EventArgs e)
        {

            BindSource(QuoteStatus.Submitted.ToString());
            isRefresh?.Invoke(QuoteStatus.Submitted.ToString(), null);

            PopupNavigation.Instance.PopAsync();
        }
        #endregion
    }
}