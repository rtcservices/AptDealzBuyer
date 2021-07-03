using AptDealzBuyer.Model;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.PopupPages;
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
        // create objects here
        public List<GrievanceM> GrievanceMs = new List<GrievanceM>();
        private string filterBy = Utility.RequirementSortBy.ID.ToString();
        private bool sortBy = true;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion

        #region Constructor
        public GrievancesPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        // write methods here
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetGrievance();
        }

        public void GetGrievance()
        {
            lstGrievance.ItemsSource = null;
            GrievanceMs = new List<GrievanceM>()
            {
                new GrievanceM{ GrNo="GR#123", GrTitle="Need 5 Canon A210 All In one Printers", GrStatus="Open"},
                new GrievanceM{ GrNo="GR#156", GrTitle="Buy 20 tyres of Maruti Alto K10 2020", GrStatus="Closed"},
                new GrievanceM{ GrNo="GR#134", GrTitle="Need 5 Canon A210 All In one Printers", GrStatus="Open"},
                new GrievanceM{ GrNo="GR#127", GrTitle="Buy 20 tyres of Maruti Alto K10 2020", GrStatus="Closed"},
                new GrievanceM{ GrNo="GR#124", GrTitle="Need 5 Canon A210 All In one Printers", GrStatus="Open"},
            };
            lstGrievance.ItemsSource = GrievanceMs.ToList();
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
            Navigation.PopAsync();
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
                GetGrievance();
                //GetGrievance(filterBy, sortBy);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievancesPage/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private void FrmAdd_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DashboardPages.RaiseGrievancesPage());
        }

        private void FrmStatus_Tapped(object sender, EventArgs e)
        {
            var sortby = new PrevReqPopup("Status", "Open", "Close");
            sortby.isRefresh += (s1, e1) =>
            {
                //get result from popup
            };
            PopupNavigation.Instance.PushAsync(sortby);
        }

        private void GrdViewGrievances_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DashboardPages.GrievanceDetailsPage());
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
                        GetGrievance();
                        //GetGrievance(filterBy, sortBy);
                    }
                };
                PopupNavigation.Instance.PushAsync(sortby);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievancesPage/CustomEntry_Unfocused: " + ex.Message);
            }
        }
        #endregion
    }
}