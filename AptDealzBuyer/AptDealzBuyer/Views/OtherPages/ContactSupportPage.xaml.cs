using AptDealzBuyer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using AptDealzBuyer.Utility;
using Xamarin.Forms.Xaml;
using AptDealzBuyer.Views.MasterData;

namespace AptDealzBuyer.Views.OtherPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactSupportPage : ContentPage
    {
        #region Objects
        // create objects here
        List<MessageList> mMessageList = new List<MessageList>();
        #endregion

        #region Constructor
        public ContactSupportPage()
        {
            InitializeComponent();

        }
        #endregion

        #region Methods
        // write methods here
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindMessages();
        }

        public void BindMessages()
        {
            mMessageList.Clear();
            mMessageList.Add(new MessageList()
            {
                Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                MessageMargin = new Thickness(20, 20, 0, 0),
                MessagePosition = LayoutOptions.EndAndExpand,
                UserName = "Michal Beven",
                Time = "10:57 am",
                IsBuyer = true
            });
            mMessageList.Add(new MessageList()
            {
                Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                MessageMargin = new Thickness(0, 20, 20, 0),
                MessagePosition = LayoutOptions.StartAndExpand,
                UserName = "Customer Support",
                Time = "10:57 am",
                IsContact = true
            });
            mMessageList.Add(new MessageList()
            {
                Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                MessageMargin = new Thickness(20, 20, 0, 0),
                MessagePosition = LayoutOptions.EndAndExpand,
                UserName = "Michal Beven",
                Time = "10:57 am",
                IsBuyer = true
            });
            mMessageList.Add(new MessageList()
            {
                Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                MessageMargin = new Thickness(0, 20, 20, 0),
                MessagePosition = LayoutOptions.StartAndExpand,
                UserName = "Customer Support",
                Time = "10:57 am",
                IsContact = true
            });
            mMessageList.Add(new MessageList()
            {
                Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                MessageMargin = new Thickness(20, 20, 0, 0),
                MessagePosition = LayoutOptions.EndAndExpand,
                UserName = "Michal Beven",
                Time = "10:57 am",
                IsBuyer = true
            });
            mMessageList.Add(new MessageList()
            {
                Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                MessageMargin = new Thickness(0, 20, 20, 0),
                MessagePosition = LayoutOptions.StartAndExpand,
                UserName = "Customer Support",
                Time = "10:57 am",
                IsContact = true
            });
            flvMain.FlowItemsSource = mMessageList.ToList();
        }
        #endregion

        #region events
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
            //Navigation.PopAsync();
        }

        private void ImgSend_Tapped(object sender, EventArgs e)
        {

        }
        #endregion

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
    }
}