using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.MasterData
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDataPage : MasterDetailPage
    {
        public MasterDataPage()
        {
            InitializeComponent();
            try
            {
                BindNavigation();

                var backgroundWorker = new BackgroundWorker();
                backgroundWorker.DoWork += delegate
                {
                    if (App.stoppableTimer == null)
                    {
                        App.stoppableTimer = new StoppableTimer(TimeSpan.FromMinutes(1), () =>
                        {
                            GetNotificationCount();
                        });
                    }
                    App.stoppableTimer.Start();
                };
                backgroundWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MasterDataPage/Constructor: " + ex.Message);
            }
        }

        void BindNavigation()
        {
            try
            {
                Common.MasterData = this;
                Common.MasterData.Master = new MenuPage();

                if (!Settings.IsNotification)
                {
                    Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage(Constraints.Str_Home));
                }
                else
                {
                    if (Common.mBuyerDetail != null && !Common.EmptyFiels(Common.mBuyerDetail.BuyerId) && !Common.EmptyFiels(Common.Token))
                    {
                        Common.MasterData.Detail = new NavigationPage(new Views.DashboardPages.NotificationPage("MasterdetailPage"));
                    }
                    else
                    {
                        Common.MasterData.Detail = new Views.SplashScreen.Spalshscreen();
                    }
                    Settings.IsNotification = false;
                }

                MasterBehavior = MasterBehavior.Popover;
                Common.MasterData.IsGestureEnabled = false;
                Common.MasterData.IsPresented = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MasterDataPage/BindNavigation: " + ex.Message);
            }
        }

        private async void GetNotificationCount()
        {
            try
            {
                var notificationCount = await DependencyService.Get<INotificationRepository>().GetNotificationCount();
                if (!Common.EmptyFiels(notificationCount))
                {
                    Common.NotificationCount = notificationCount;
                    MessagingCenter.Send<string>(Common.NotificationCount, Constraints.Str_NotificationCount);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MasterDataPage/GetNotificationCount: " + ex.Message);
            }
        }
    }
}