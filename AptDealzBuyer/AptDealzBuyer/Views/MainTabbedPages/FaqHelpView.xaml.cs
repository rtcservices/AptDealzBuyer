﻿using AptDealzBuyer.Model;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.MasterData;
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
        #region Objects
        // create objects here
        public event EventHandler isRefresh;
        public List<FaqM> FaqMs = new List<FaqM>();
        #endregion

        #region Constructor
        public FaqHelpView()
        {
            InitializeComponent();
            BindFaq();
        }
        #endregion

        #region Methods
        public void BindFaq()
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
        #endregion

        #region Events
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

        private void ImgExpand_Tapped(object sender, EventArgs e)
        {
            var imgExp = (Grid)sender;
            var viewCell = (ViewCell)imgExp.Parent.Parent;
            if (viewCell != null)
            {
                viewCell.ForceUpdateSize();
            }
            var faqModel = imgExp.BindingContext as FaqM;
            if (faqModel != null && faqModel.ArrowImage == Constraints.Arrow_Down)
            {
                faqModel.ArrowImage = Constraints.Arrow_Up;
                faqModel.ShowFaqDesc = true;
            }
            else
            {
                faqModel.ArrowImage = Constraints.Arrow_Down;
                faqModel.ShowFaqDesc = false;
            }
        }
        #endregion
    }
}