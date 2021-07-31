using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.DashboardPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GrievanceDetailsPage : ContentPage
    {
        #region Objects
        private Grievance mGrievance;
        private string GrievanceId;
        #endregion

        #region Constructor
        public GrievanceDetailsPage(string GrievanceId)
        {
            InitializeComponent();
            this.GrievanceId = GrievanceId;

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
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetGrievancesDetails();
        }

        private async Task GetGrievancesDetails()
        {
            try
            {
                mGrievance = await DependencyService.Get<IGrievanceRepository>().GetGrievancesDetails(GrievanceId);
                if (mGrievance != null)
                {
                    lblGrievanceId.Text = mGrievance.GrievanceNo;
                    lblOrderId.Text = mGrievance.OrderNo;
                    lblOrderDate.Text = mGrievance.OrderDate.ToString("dd/MM/yyyy");
                    lblGrievanceDate.Text = mGrievance.Created.ToString("dd/MM/yyyy");
                    lblBuyeName.Text = mGrievance.GrievanceFromUserName;
                    lblGrievanceType.Text = mGrievance.GrievanceTypeDescr.ToCamelCase();
                    lblStatus.Text = mGrievance.StatusDescr.ToCamelCase();
                    if (Common.EmptyFiels(mGrievance.IssueDescription))
                    {
                        lblDescription.Text = "No description found";
                    }
                    else
                    {
                        lblDescription.Text = mGrievance.IssueDescription;
                    }

                    if (mGrievance.GrievanceResponses != null && mGrievance.GrievanceResponses.Count > 0)
                    {
                        foreach (var grievanceResponses in mGrievance.GrievanceResponses)
                        {
                            if (grievanceResponses.ResponseFromUserId != Settings.UserId)
                            {
                                //User Data
                                grievanceResponses.IsContact = false;
                                grievanceResponses.IsUser = true;
                            }

                            string baseURL = (string)App.Current.Resources["BaseURL"];
                            grievanceResponses.ResponseFromUserProfileImage = baseURL + grievanceResponses.ResponseFromUserProfileImage.Replace(baseURL, "");
                        }
                        lstResponse.IsVisible = true;
                        lblNoRecord.IsVisible = false;
                        lstResponse.ItemsSource = mGrievance.GrievanceResponses.ToList();
                        lstResponse.HeightRequest = mGrievance.GrievanceResponses.Count * 100;
                    }
                    else
                    {
                        lstResponse.ItemsSource = null;
                        lstResponse.IsVisible = false;
                        lblNoRecord.IsVisible = true;
                    }

                    AttachDocumentList();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievanceDetailsPage/GetGrievancesDetails: " + ex.Message);
            }
        }

        private async Task SubmitGrievanceResponse()
        {
            try
            {
                GrievanceAPI grievanceAPI = new GrievanceAPI();
                UserDialogs.Instance.ShowLoading(Constraints.Loading);


                if (!Common.EmptyFiels(txtMessage.Text))
                {
                    var mResponse = await grievanceAPI.SubmitGrievanceResponseFromBuyer(GrievanceId, txtMessage.Text);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        txtMessage.Text = string.Empty;
                        if ((bool)mResponse.Data)
                            Common.DisplaySuccessMessage(mResponse.Message);
                        else
                            Common.DisplayErrorMessage(mResponse.Message);
                        await GetGrievancesDetails();
                    }
                    else
                    {
                        if (mResponse != null && !Common.EmptyFiels(mResponse.Message))
                            Common.DisplayErrorMessage(mResponse.Message);
                        else
                            Common.DisplayErrorMessage(Constraints.Something_Wrong);
                    }
                }
                else
                {
                    BoxMessage.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                    Common.DisplayErrorMessage(Constraints.Required_Response);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievanceDetailsPage/SubmitGrievanceResponse: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

        }

        private void AttachDocumentList()
        {
            try
            {
                if (mGrievance.Documents != null && mGrievance.Documents.Count > 0)
                {
                    lblAttachDocument.IsVisible = false;
                    lstDocument.ItemsSource = mGrievance.Documents.ToList();
                    lstDocument.IsVisible = true;
                }
                else
                {
                    lblAttachDocument.IsVisible = true;
                    lstDocument.ItemsSource = null;
                    lstDocument.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievanceDetailsPage/AttachDocumentList: " + ex.Message);
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

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Navigation.PopAsync();
        }

        private async void BtnSubmit_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnSubmit);
            await SubmitGrievanceResponse();
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            rfView.IsRefreshing = true;
            await GetGrievancesDetails();
            rfView.IsRefreshing = false;
        }

        private void lstResponse_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            lstResponse.SelectedItem = null;
        }

        private void txtMessage_Unfocused(object sender, FocusEventArgs e)
        {
            if (!Common.EmptyFiels(txtMessage.Text))
            {
                BoxMessage.BackgroundColor = (Color)App.Current.Resources["LightGray"];
            }
        }
        #endregion

        private void CopyString_Tapped(object sender, EventArgs e)
        {
            try
            {
                var stackLayout = (StackLayout)sender;
                if (!Common.EmptyFiels(stackLayout.ClassId))
                {
                    if (stackLayout.ClassId == "GrievanceId")
                    {
                        string message = Constraints.CopiedGrievanceId;
                        Common.CopyText(lblGrievanceId, message);
                    }
                    else if (stackLayout.ClassId == "OrderId")
                    {
                        string message = Constraints.CopiedOrderId;
                        Common.CopyText(lblOrderId, message);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievanceDetailsPage/CopyString_Tapped: " + ex.Message);
            }
        }
    }
}