using AptDealzBuyer.Model;
using AptDealzBuyer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.OtherPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeSupportPage : ContentPage
    {
        #region Objecst
        // create objects here
        public List<CarousellImage> mCarousellImages = new List<CarousellImage>();
        #endregion

        #region Constructor
        public WeSupportPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        // write methods here
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindCarousallData();
        }

        public void BindCarousallData()
        {
            mCarousellImages = new List<CarousellImage>()
            {
                new CarousellImage{ImageName="imgMakeInIndia.png"},
                new CarousellImage{ImageName="imgMakeInIndia.png"},
                new CarousellImage{ImageName="imgMakeInIndia.png"},
            };
            indicaters.ItemsSource = cvWelcome.ItemsSource = mCarousellImages.ToList();
        }
        #endregion

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
            Navigation.PopAsync();
        }
    }
}