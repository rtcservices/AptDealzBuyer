﻿using AptDealzBuyer.Model;
using AptDealzBuyer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class HomeView : ContentView
    {
        #region Constructor
        public HomeView()
        {
            InitializeComponent();
            BindMenus();

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
        private void BindMenus()
        {
            try
            {
                List<HomeMenu> HomeMenus;
                HomeMenus = new List<HomeMenu>()
                {
                    new HomeMenu{ MenuId=1, MenuImage="imgActiveRequirements.png", UiName="Active\nRequirements", MenuName="ActiveRequirements"},
                    new HomeMenu{ MenuId=2, MenuImage="imgPostRequirements.png", UiName="Post New\nRequirement", MenuName="PostNewRequirements"},
                    new HomeMenu { MenuId = 3, MenuImage = "imgPreviousRequirements.png", UiName = "Previous\nRequirements", MenuName = "PreviousRequirements" },
                    new HomeMenu { MenuId = 4, MenuImage = "imgOrderHistory.png", UiName = "Order\nHistory", MenuName = "Order" },
                    new HomeMenu { MenuId = 5, MenuImage = "imgShippingDetails.png", UiName = "Shipping\nDetails", MenuName = "ShippingDetails" },
                    new HomeMenu { MenuId = 6, MenuImage = "imgNotifications.png", UiName = "Notifications", MenuName = "Notifications" },
                    new HomeMenu { MenuId = 7, MenuImage = "imgProfile.png", UiName = "Profile", MenuName = "Profile" },
                    new HomeMenu { MenuId = 8, MenuImage = "imgGrievances.png", UiName = "Grievances", MenuName = "Grievances" },
                    new HomeMenu { MenuId = 9, MenuImage = "imgContactSupport.png", UiName = "Contact\nSupport", MenuName = "ContactSupport" },
                    new HomeMenu { MenuId = 10, MenuImage = "imgAboutAptDealz.png", UiName = "About\nAptDealz", MenuName = "AboutAptDealz" },
                    new HomeMenu { MenuId = 11, MenuImage = "iconTandP.png", UiName = "Terms & Policies", MenuName = "TermsPolicies" },
                    new HomeMenu { MenuId = 12, MenuImage = "imgFAQHelp.png", UiName = "FAQ & Help", MenuName = "FAQHelp" },
                    new HomeMenu { MenuId = 13, MenuImage = "imgWeSupport.png", UiName = "We Support", MenuName = "WeSupport" },
                };

                flvMenus.FlowItemsSource = HomeMenus.ToList();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("HomeView/BindMenus: " + ex.Message);
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
            Navigation.PushAsync(new DashboardPages.NotificationPage());
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void BtnMenu_Tapped(object sender, EventArgs e)
        {
            try
            {
                var stk = (Frame)sender;
                var menuName = stk.BindingContext as HomeMenu;

                if (menuName != null && menuName.MenuName == "PostNewRequirements")
                {
                    Navigation.PushAsync(new DashboardPages.PostNewRequiremntPage());
                }
                else if (menuName != null && menuName.MenuName == "Notifications")
                {
                    Navigation.PushAsync(new DashboardPages.NotificationPage());
                }
                else if (menuName != null && menuName.MenuName == "ContactSupport")
                {
                    Navigation.PushAsync(new OtherPages.ContactSupportPage());
                }
                else if (menuName != null && menuName.MenuName == "Grievances")
                {
                    Navigation.PushAsync(new DashboardPages.GrievancesPage());
                }
                else if (menuName != null && menuName.MenuName == "WeSupport")
                {
                    Navigation.PushAsync(new OtherPages.WeSupportPage());
                }
                else if (menuName != null && menuName.MenuName != null)
                {
                    Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(menuName.MenuName, isNavigate: true));
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("HomeView/BtnMenu_Tapped: " + ex.Message);
            }
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
        #endregion
    }
}