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

    public partial class ActiveRequirementView : ContentView
    {
        #region Objects
        // create objects here
        public event EventHandler isRefresh;
        public List<RequirementM> RequirementMs = new List<RequirementM>();
        #endregion

        #region Constructor
        public ActiveRequirementView()
        {
            InitializeComponent();
            BindRequirements();
        }
        #endregion

        #region Methods
        // write methods here
        public void BindRequirements()
        {
            lstRequirements.ItemsSource = null;
            string reqdesc = "Lorem Ipsum is simply dummy text.";
            string catdesc = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old.";
            RequirementMs = new List<RequirementM>()
            {
                new RequirementM{ RequirementNo="REQ#123", RequirementDes=reqdesc, CatDescription=catdesc, ReqDate="02-05-2021", QuoteCount=2 },
                new RequirementM{ RequirementNo="REQ#128", RequirementDes=reqdesc, CatDescription=catdesc, ReqDate="10-05-2021", QuoteCount=5 },
                new RequirementM{ RequirementNo="REQ#132", RequirementDes=reqdesc, CatDescription=catdesc, ReqDate="18-05-2021", QuoteCount=1 },
                new RequirementM{ RequirementNo="REQ#141", RequirementDes=reqdesc, CatDescription=catdesc, ReqDate="22-05-2021", QuoteCount=6 },
                new RequirementM{ RequirementNo="REQ#149", RequirementDes=reqdesc, CatDescription=catdesc, ReqDate="27-05-2021", QuoteCount=4 },
                new RequirementM{ RequirementNo="REQ#155", RequirementDes=reqdesc, CatDescription=catdesc, ReqDate="03-06-2021", QuoteCount=3 },
                new RequirementM{ RequirementNo="REQ#163", RequirementDes=reqdesc, CatDescription=catdesc, ReqDate="11-06-2021", QuoteCount=2 },
            };

            lstRequirements.ItemsSource = RequirementMs.ToList();
        }
        #endregion

        #region Events
        // create events here
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(image: ImgMenu);
            try
            {
                if (Common.MasterData != null)
                {
                    Common.MasterData.IsGestureEnabled = true;
                    Common.MasterData.IsPresented = true;
                }
            }
            catch (Exception ex)
            {
                //Common.DisplayErrorMessage("HomeView/ImgMenu_Tapped: " + ex.Message);
            }
        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(image: ImgBack);
            App.Current.MainPage = new MasterDataPage();
        }

        private void ImgSearch_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new PopupPages.SearchPopup());
        }

        private void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            var sortby = new SortByPopup("ID", "Date", "No of Quotes");
            sortby.isRefresh += (s1, e1) =>
              {
                  //get result from popup
              };
            PopupNavigation.Instance.PushAsync(sortby);
        }

        private void ImgExpand_Tapped(object sender, EventArgs e)
        {
            var imgExp = (Image)sender;
            var viewCell = (ViewCell)imgExp.Parent.Parent.Parent.Parent;
            if (viewCell != null)
            {
                viewCell.ForceUpdateSize();
            }
            var reqModel = imgExp.BindingContext as RequirementM;
            if (reqModel != null && reqModel.ArrowImage == Constraints.Arrow_Right)
            {
                reqModel.ArrowImage = Constraints.Arrow_Down;
                reqModel.Layout = LayoutOptions.StartAndExpand;
                reqModel.ShowDelete = false;
                reqModel.ShowCategory = true;
            }
            else
            {
                reqModel.ArrowImage = Constraints.Arrow_Right;
                reqModel.Layout = LayoutOptions.CenterAndExpand;
                reqModel.ShowDelete = true;
                reqModel.ShowCategory = false;
            }
        }

        private void GrdViewRequirement_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DashboardPages.ViewRequirememntPage("active"));
        }
        #endregion
    }
}