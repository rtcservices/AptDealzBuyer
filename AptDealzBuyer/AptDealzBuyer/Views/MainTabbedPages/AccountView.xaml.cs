﻿using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Extention;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountView : ContentView, INotifyPropertyChanged
    {
        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private ObservableCollection<string> _mCountriesData;
        public ObservableCollection<string> mCountriesData
        {
            get { return _mCountriesData; }
            set
            {
                _mCountriesData = value;
                OnPropertyChanged("mCountriesData");
            }
        }
        #endregion

        #region Objects          
        private ProfileAPI profileAPI;
        private BuyerDetails mBuyerDetail;
        private string relativePath;
        bool isFirstLoad = true;
        private bool isUpdatPhoto = false;
        #endregion

        #region Constructor
        public AccountView()
        {
            InitializeComponent();
            txtFullName.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
            BtnUpdate.IsEnabled = false;
            BindProperties();

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
        private async void BindProperties()
        {
            try
            {
                profileAPI = new ProfileAPI();
                if (Common.mCountries == null || Common.mCountries.Count == 0)
                    await GetCountries();

                await Task.Run(() =>
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                });
                await GetProfile();
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/BindProperties: " + ex.Message);
                UserDialogs.Instance.HideLoading();
            }
        }

        private async Task GetCountries()
        {
            try
            {
                Common.mCountries = await profileAPI.GetCountry();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/GetCountries: " + ex.Message);
            }
        }

        private async Task GetProfile()
        {
            try
            {
                if (Common.mBuyerDetail == null || Common.EmptyFiels(Common.mBuyerDetail.BuyerId))
                {
                    var mResponse = await profileAPI.GetMyProfileData();
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                        if (jObject != null)
                        {
                            mBuyerDetail = jObject.ToObject<Model.Request.BuyerDetails>();
                        }
                    }
                    else
                    {
                        if (mResponse != null)
                            Common.DisplayErrorMessage(mResponse.Message);
                        else
                            Common.DisplayErrorMessage(Constraints.Something_Wrong);
                    }
                }
                else
                {
                    mBuyerDetail = Common.mBuyerDetail;
                }

                GetProfileDetails(mBuyerDetail);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/GetProfile: " + ex.Message);
            }
        }

        private void GetProfileDetails(BuyerDetails mBuyerDetails)
        {
            try
            {
                lblBuyerId.Text = mBuyerDetails.BuyerNo;
                txtFullName.Text = mBuyerDetails.FullName;
                txtEmailAddress.Text = mBuyerDetails.Email;
                txtPhoneNumber.Text = mBuyerDetails.PhoneNumber;

                if (!Common.EmptyFiels(mBuyerDetails.ProfilePhoto))
                {
                    string baseURL = (string)App.Current.Resources["BaseURL"];
                    mBuyerDetails.ProfilePhoto = baseURL + mBuyerDetails.ProfilePhoto.Replace(baseURL, "");
                    imgUser.Source = mBuyerDetails.ProfilePhoto;
                }
                else
                {
                    imgUser.Source = "iconUserAccount.png";
                }
                if (!Common.EmptyFiels(mBuyerDetails.Building))
                {
                    txtBuildingNumber.Text = mBuyerDetails.Building;
                }
                if (!Common.EmptyFiels(mBuyerDetails.Street))
                {
                    txtStreet.Text = mBuyerDetails.Street;
                }
                if (!Common.EmptyFiels(mBuyerDetails.City))
                {
                    txtCity.Text = mBuyerDetails.City;
                }
                if (!Common.EmptyFiels(mBuyerDetails.Landmark))
                {
                    txtLandmark.Text = mBuyerDetails.Landmark;
                }
                if (!Common.EmptyFiels(mBuyerDetails.State))
                {
                    txtState.Text = mBuyerDetails.State;
                }
                if (!Common.EmptyFiels(mBuyerDetails.PinCode))
                {
                    txtPinCode.Text = mBuyerDetails.PinCode;
                }
                if (mBuyerDetails.CountryId > 0 && Common.mCountries != null && Common.mCountries.Count() > 0)
                {
                    pkNationality.Text = Common.mCountries.Where(x => x.CountryId == mBuyerDetails.CountryId).FirstOrDefault().Name;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/BindProfileDetails: " + ex.Message);
            }
        }

        private void HasUpdateProfileDetail()
        {
            try
            {
                bool isUpdate = false;
                if (mBuyerDetail == null)
                    isUpdate = true;

                if (mBuyerDetail.FullName != txtFullName.Text) // Alex != Alex1
                    isUpdate = true;
                else if (mBuyerDetail.PhoneNumber != txtPhoneNumber.Text)
                    isUpdate = true;
                else if (!Common.EmptyFiels(mBuyerDetail.Nationality) && mBuyerDetail.Nationality != pkNationality.Text)
                    isUpdate = true;
                else if (mBuyerDetail.Building != txtBuildingNumber.Text)
                    isUpdate = true;
                else if (mBuyerDetail.Street != txtStreet.Text)
                    isUpdate = true;
                else if (mBuyerDetail.City != txtCity.Text)
                    isUpdate = true;
                else if (mBuyerDetail.PinCode != txtPinCode.Text)
                    isUpdate = true;
                else if (mBuyerDetail.Landmark != txtLandmark.Text)
                    isUpdate = true;
                else if (mBuyerDetail.State != txtState.Text)
                    isUpdate = true;
                else if (isUpdatPhoto)
                    isUpdate = true;
                else
                    isUpdate = false;

                BtnUpdate.IsEnabled = isUpdate;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/HasUpdateProfileDetail: " + ex.Message);
            }

        }

        private Model.Request.BuyerDetails UpdateProfileDetails()
        {
            try
            {
                mBuyerDetail.UserId = mBuyerDetail.BuyerId;
                mBuyerDetail.FullName = txtFullName.Text;
                mBuyerDetail.Email = txtEmailAddress.Text;
                mBuyerDetail.PhoneNumber = txtPhoneNumber.Text;

                if (!Common.EmptyFiels(relativePath))
                {
                    string baseURL = (string)App.Current.Resources["BaseURL"];
                    mBuyerDetail.ProfilePhoto = relativePath.Replace(baseURL, "");
                }
                mBuyerDetail.Building = txtBuildingNumber.Text;
                mBuyerDetail.Street = txtStreet.Text;
                mBuyerDetail.City = txtCity.Text;
                if (!Common.EmptyFiels(pkNationality.Text))
                {
                    mBuyerDetail.CountryId = Common.mCountries.Where(x => x.Name.ToLower() == pkNationality.Text.ToLower().ToString()).FirstOrDefault()?.CountryId;
                }
                mBuyerDetail.Landmark = txtLandmark.Text;
                mBuyerDetail.State = txtState.Text;
                mBuyerDetail.PinCode = txtPinCode.Text;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/UpdateProfileDetails: " + ex.Message);
            }
            return mBuyerDetail;
        }

        private bool Validations()
        {
            bool isValid = false;
            try
            {
                if (Common.EmptyFiels(txtFullName.Text) || Common.EmptyFiels(txtPhoneNumber.Text) || Common.EmptyFiels(pkNationality.Text))
                {
                    RequiredFields();
                    isValid = false;
                }

                if (Common.EmptyFiels(txtFullName.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_FullName);
                }
                else if (Common.EmptyFiels(txtPhoneNumber.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_PhoneNumber);
                }
                else if (!txtPhoneNumber.Text.IsValidPhone())
                {
                    Common.DisplayErrorMessage(Constraints.InValid_PhoneNumber);
                }
                else if (Common.EmptyFiels(pkNationality.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Nationality);
                }
                else if (Common.mCountries.Where(x => x.Name.ToLower() == pkNationality.Text.ToLower()).Count() == 0)
                {
                    Common.DisplayErrorMessage(Constraints.InValid_Nationality);
                }
                else
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/Validations: " + ex.Message);
            }
            return isValid;
        }

        private void RequiredFields()
        {
            try
            {
                Common.DisplayErrorMessage(Constraints.Required_All);

                if (Common.EmptyFiels(txtFullName.Text))
                {
                    BoxFullName.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtPhoneNumber.Text))
                {
                    BoxPhoneNumber.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(pkNationality.Text))
                {
                    BoxNationality.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/RequiredFields: " + ex.Message);
            }
        }

        private void FieldsTrim()
        {
            try
            {
                txtFullName.Text = txtFullName.Text.Trim();
                txtPhoneNumber.Text = txtPhoneNumber.Text.Trim();
                if (!Common.EmptyFiels(txtBuildingNumber.Text))
                {
                    txtBuildingNumber.Text = txtBuildingNumber.Text.Trim();
                }
                if (!Common.EmptyFiels(txtStreet.Text))
                {
                    txtStreet.Text = txtStreet.Text.Trim();
                }
                if (!Common.EmptyFiels(txtCity.Text))
                {
                    txtCity.Text = txtCity.Text.Trim();
                }
                if (!Common.EmptyFiels(txtLandmark.Text))
                {
                    txtLandmark.Text = txtLandmark.Text.Trim();
                }
                if (!Common.EmptyFiels(txtState.Text))
                {
                    txtState.Text = txtState.Text.Trim();
                }
                if (!Common.EmptyFiels(txtPinCode.Text))
                {
                    txtPinCode.Text = txtPinCode.Text.Trim();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/FieldsTrim: " + ex.Message);
            }
        }

        private async void UpdateProfile()
        {
            try
            {
                if (Validations())
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    if (!await PinCodeValidation())
                        return;

                    FieldsTrim();
                    var mBuyerDetails = UpdateProfileDetails();

                    var mResponse = await profileAPI.SaveProfile(mBuyerDetails);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        var updateId = mResponse.Data;
                        if (updateId != null)
                        {
                            var successPopup = new PopupPages.SuccessPopup(mResponse.Message);
                            await PopupNavigation.Instance.PushAsync(successPopup);
                            BtnUpdate.IsEnabled = false;
                        }
                    }
                    else
                    {
                        if (mResponse != null)
                            Common.DisplayErrorMessage(mResponse.Message);
                        else
                            Common.DisplayErrorMessage(Constraints.Something_Wrong);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/UpdateProfile: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async void DoLogout()
        {
            try
            {
                var isClose = await App.Current.MainPage.DisplayAlert(Constraints.Logout, Constraints.AreYouSureWantLogout, Constraints.Yes, Constraints.No);
                if (isClose)
                {
                    AuthenticationAPI authenticationAPI = new AuthenticationAPI();
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    var mResponse = await authenticationAPI.Logout(Settings.RefreshToken, Settings.LoginTrackingKey);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        Common.DisplaySuccessMessage(mResponse.Message);
                    }
                    else
                    {
                        if (mResponse != null && !mResponse.Message.Contains("TrackingKey"))
                            Common.DisplayErrorMessage(mResponse.Message);
                    }

                    Settings.EmailAddress = string.Empty;
                    Settings.UserToken = string.Empty;
                    Settings.RefreshToken = string.Empty;
                    Settings.LoginTrackingKey = string.Empty;

                    App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/DoLogout: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void UnfocussedFields(Entry entry = null, ExtAutoSuggestBox autoSuggestBox = null)
        {
            try
            {
                if (entry != null)
                {
                    if (entry.ClassId == "FullName")
                    {
                        BoxFullName.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }

                    else if (entry.ClassId == "PhoneNumber")
                    {
                        BoxPhoneNumber.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                }

                if (autoSuggestBox != null)
                {
                    if (autoSuggestBox.ClassId == "Nationality")
                    {
                        BoxNationality.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                }

                HasUpdateProfileDetail();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/UnfocussedFields: " + ex.Message);
            }
        }

        private async Task<bool> PinCodeValidation()
        {
            bool isValid = false;
            try
            {
                if (!Common.EmptyFiels(txtPinCode.Text))
                {
                    txtPinCode.Text = txtPinCode.Text.Trim();
                    isValid = await DependencyService.Get<IProfileRepository>().ValidPincode(txtPinCode.Text);
                    if (isValid)
                    {
                        BoxPincode.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else
                    {
                        BoxPincode.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                    }
                }
                else
                {
                    BoxPincode.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    isValid = true;
                }
                HasUpdateProfileDetail();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/PinCodeValidation: " + ex.Message);
            }
            return isValid;
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
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnUpdate);
            UpdateProfile();
        }

        private void BtnDeactivate_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnDeactivate);
            Navigation.PushAsync(new OtherPages.DeactivateAccountPage());
        }

        private async void ImgCamera_Tapped(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(image: ImgCamera);
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                ImageConvertion.SelectedImagePath = imgUser;
                ImageConvertion.SetNullSource((int)FileUploadCategory.ProfilePicture);
                await ImageConvertion.SelectImage();

                if (ImageConvertion.SelectedImageByte != null)
                {
                    relativePath = await DependencyService.Get<IFileUploadRepository>().UploadFile((int)FileUploadCategory.ProfilePicture);
                    isUpdatPhoto = true;
                    HasUpdateProfileDetail();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/ImgCamera_Tapped: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void Logout_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnLogout);
            DoLogout();
        }

        int i = 0;
        private void AutoSuggestBox_TextChanged(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxTextChangedEventArgs e)
        {
            try
            {
                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    if (isFirstLoad || i < 2)
                    {
                        isFirstLoad = false;
                        pkNationality.IsSuggestionListOpen = false;
                        i++;
                        return;
                    }
                }

                if (mCountriesData == null)
                    mCountriesData = new ObservableCollection<string>();

                mCountriesData.Clear();
                if (!string.IsNullOrEmpty(pkNationality.Text))
                {
                    mCountriesData = new ObservableCollection<string>(Common.mCountries.Where(x => x.Name.ToLower().Contains(pkNationality.Text.ToLower())).Select(x => x.Name));
                }
                else
                {
                    mCountriesData = new ObservableCollection<string>(Common.mCountries.Select(x => x.Name));
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/AutoSuggestBox_TextChanged: " + ex.Message);
            }
        }

        private void AutoSuggestBox_QuerySubmitted(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            try
            {
                if (e.ChosenSuggestion != null)
                {
                    pkNationality.Text = e.ChosenSuggestion.ToString();
                }
                else
                {
                    // User hit Enter from the search box. Use args.QueryText to determine what to do.
                    pkNationality.Unfocus();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/AutoSuggestBox_QuerySubmitted: " + ex.Message);
            }
        }

        private void AutoSuggestBox_SuggestionChosen(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            pkNationality.Text = e.SelectedItem.ToString();
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = (ExtEntry)sender;
            UnfocussedFields(entry: entry);
        }

        private void AutoSuggestBox_Unfocused(object sender, FocusEventArgs e)
        {
            var autoSuggestBox = (ExtAutoSuggestBox)sender;
            if (!Common.EmptyFiels(autoSuggestBox.Text))
            {
                UnfocussedFields(autoSuggestBox: autoSuggestBox);
            }
        }

        private async void txtPinCode_Unfocused(object sender, FocusEventArgs e)
        {
            await PinCodeValidation();
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            try
            {
                rfView.IsRefreshing = true;
                BtnUpdate.IsEnabled = false;
                BindProperties();
                rfView.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/RefreshView_Refreshing: " + ex.Message);
            }
        }

        private void StkBuyerId_Tapped(object sender, EventArgs e)
        {
            try
            {
                string message = Constraints.CopiedBuyerId;
                Common.CopyText(lblBuyerId, message);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/StkBuyerId_Tapped: " + ex.Message);
            }
        }
        #endregion
    }
}