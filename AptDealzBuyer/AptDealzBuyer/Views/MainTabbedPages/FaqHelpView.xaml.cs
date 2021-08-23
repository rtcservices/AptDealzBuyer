using AptDealzBuyer.Model;
using AptDealzBuyer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FaqHelpView : ContentView
    {
        #region [ Objects ]       
        public List<FaqM> FaqMs = new List<FaqM>();
        #endregion

        #region [ Constructor ]
        public FaqHelpView()
        {
            try
            {
                InitializeComponent();
                BindFaq();

                MessagingCenter.Unsubscribe<string>(this, "NotificationCount"); MessagingCenter.Subscribe<string>(this, "NotificationCount", (count) =>
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
                Common.DisplayErrorMessage("FaqHelpView/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]
        private void BindFaq()
        {
            try
            {
                lstFaq.ItemsSource = null;
                FaqMs = new List<FaqM>()
            {
                new FaqM{ FaqTitle="How do I post requirement?", FaqDesc="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text."},
                new FaqM{ FaqTitle="How do I view the quotes receive", FaqDesc="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text."},
                new FaqM{ FaqTitle="Do I have to pay to submit require", FaqDesc="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text."},
                new FaqM{ FaqTitle="How long will my requirement be", FaqDesc="Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text."}
            };
                lstFaq.ItemsSource = FaqMs.ToList();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FaqHelpView/BindFaq: " + ex.Message);
            }
        }
        #endregion

        #region [ Events ]
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(image: ImgMenu);
            //Common.OpenMenu();
        }

        private async void ImgNotification_Tapped(object sender, EventArgs e)
        {
            var Tab = (Grid)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    await Navigation.PushAsync(new DashboardPages.NotificationPage());
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("FaqHelpView/ImgNotification_Tapped: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }

        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private void ImgExpand_Tapped(object sender, EventArgs e)
        {
            try
            {
                var imgExp = (Grid)sender;
                var viewCell = (ViewCell)imgExp.Parent.Parent;
                if (viewCell != null)
                {
                    viewCell.ForceUpdateSize();
                }
                var faqModel = imgExp.BindingContext as FaqM;
                if (faqModel != null && faqModel.ArrowImage == Constraints.GreenArrow_Down)
                {
                    faqModel.ArrowImage = Constraints.GreenArrow_Up;
                    faqModel.ShowFaqDesc = true;
                }
                else
                {
                    faqModel.ArrowImage = Constraints.GreenArrow_Down;
                    faqModel.ShowFaqDesc = false;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FaqHelpView/ImgExpand_Tapped: " + ex.Message);
            }
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
        #endregion

        private void lstFaq_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstFaq.SelectedItem = null;
        }
    }
}