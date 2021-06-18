using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.MasterData;
using System;
using System.Collections.Generic;
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
        private List<Country> countries;
        private BuyerDetails mBuyerDetail;
        private string relativePath;
        bool isFirstLoad = true;
        #endregion

        #region Constructor
        public AccountView()
        {
            InitializeComponent();
            txtFullName.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
            BindProperties();
        }
        #endregion

        #region Methods       
        async void BindProperties()
        {
            try
            {
                profileAPI = new ProfileAPI();
                UserDialogs.Instance.ShowLoading(Constraints.Loading);

                if (countries == null || countries.Count == 0)
                    await GetCountries();

                //pkNationality.ItemsSource = countries.Select(x => x.Name).ToList();
                await GetProfile();
            }
            catch (Exception)
            {
                //Common.DisplayErrorMessage("AccountView/BindProperties: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        async Task GetCountries()
        {
            try
            {
                countries = await profileAPI.GetCountry();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/GetCountries: " + ex.Message);
            }
        }

        async Task GetProfile()
        {
            try
            {
                var mResponse = await profileAPI.GetMyProfileData();
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        mBuyerDetail = jObject.ToObject<Model.Request.BuyerDetails>();
                        if (mBuyerDetail != null)
                        {
                            BindProfileDetails(mBuyerDetail);
                            //txtFullName.Focus();
                            //pkNationality.Unfocus();
                            //pkNationality.IsSuggestionListOpen = false;
                        }
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
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/GetProfile: " + ex.Message);
            }
        }

        void BindProfileDetails(BuyerDetails mBuyerDetails)
        {
            try
            {
                txtFullName.Text = mBuyerDetails.FullName;
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
                if (!Common.EmptyFiels(mBuyerDetails.PinCode))
                {
                    txtPinCode.Text = mBuyerDetails.PinCode;
                }
                if (mBuyerDetails.CountryId > 0 && countries != null && countries.Count() > 0)
                {
                    pkNationality.Text = countries.Where(x => x.CountryId == mBuyerDetails.CountryId).FirstOrDefault().Name;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/BindProfileDetails: " + ex.Message);
            }
        }

        Model.Request.BuyerDetails FillProfileDetails()
        {
            mBuyerDetail.UserId = mBuyerDetail.BuyerId;
            mBuyerDetail.FullName = txtFullName.Text;
            mBuyerDetail.PhoneNumber = txtPhoneNumber.Text;

            if (!Common.EmptyFiels(relativePath))
            {
                string baseURL = (string)App.Current.Resources["BaseURL"];
                mBuyerDetail.ProfilePhoto = relativePath.Replace(baseURL, "");
                //mBuyerDetail.ProfilePhoto = relativePath;
            }
            if (!Common.EmptyFiels(txtBuildingNumber.Text))
            {
                mBuyerDetail.Building = txtBuildingNumber.Text;
            }
            if (!Common.EmptyFiels(txtStreet.Text))
            {
                mBuyerDetail.Street = txtStreet.Text;
            }
            if (!Common.EmptyFiels(txtCity.Text))
            {
                mBuyerDetail.City = txtCity.Text;
            }
            if (!Common.EmptyFiels(pkNationality.Text))
            {
                mBuyerDetail.CountryId = countries.Where(x => x.Name == pkNationality.Text.ToString()).FirstOrDefault()?.CountryId;
            }
            if (!Common.EmptyFiels(txtLandmark.Text))
            {
                mBuyerDetail.Landmark = txtLandmark.Text;
            }
            if (!Common.EmptyFiels(txtPinCode.Text))
            {
                mBuyerDetail.PinCode = txtPinCode.Text;
            }
            return mBuyerDetail;
        }

        bool Validations()
        {
            bool result = false;
            try
            {
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
                else
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/Validations: " + ex.Message);
            }
            return result;
        }

        async void UpdateProfile()
        {
            try
            {
                if (Validations())
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    var mBuyerDetails = FillProfileDetails();

                    var mResponse = await profileAPI.SaveProfile(mBuyerDetails);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        var updateId = mResponse.Data;
                        if (updateId != null)
                        {
                            Common.DisplaySuccessMessage(mResponse.Message);
                        }
                    }
                    else
                    {
                        if (mResponse == null)
                            return;

                        Common.DisplayErrorMessage(mResponse.Message);
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

        async void DoLogout()
        {
            try
            {
                var isClose = await App.Current.MainPage.DisplayAlert(Constraints.Logout, Constraints.AreYouSureWantLogout, Constraints.Yes, Constraints.No);
                if (isClose)
                {
                    AuthenticationAPI authenticationAPI = new AuthenticationAPI();
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

                    App.Current.MainPage = new NavigationPage(new SplashScreen.WelcomePage(true));
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/DoLogout: " + ex.Message);
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

        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(image: ImgBack);
            App.Current.MainPage = new MasterDataPage();
        }

        private void FrmUpdate_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(frame: FrmUpdate);
            UpdateProfile();
        }

        private void FrmDeactivate_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OtherPages.DeactivateAccountPage());
        }

        private async void ImgCamera_Tapped(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(image: ImgCamera);
                ImageConvertion.SelectedImagePath = imgUser;
                ImageConvertion.SetNullSource((int)FileUploadCategory.ProfilePicture);
                await ImageConvertion.SelectImage();

                if (ImageConvertion.SelectedImageByte != null)
                {
                    relativePath = await DependencyService.Get<IFileUploadRepository>().UploadFile((int)FileUploadCategory.ProfilePicture);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/ImgCamera_Tapped: " + ex.Message);
            }
        }

        private void Logout_Tapped(object sender, EventArgs e)
        {
            DoLogout();
        }

        int i = 0;
        private void AutoSuggestBox_TextChanged(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxTextChangedEventArgs e)
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
                mCountriesData = new ObservableCollection<string>(countries.Where(x => x.Name.ToLower().Contains(pkNationality.Text.ToLower())).Select(x => x.Name));
            }
            else
            {
                mCountriesData = new ObservableCollection<string>(countries.Select(x => x.Name));
            }
        }

        private void AutoSuggestBox_QuerySubmitted(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxQuerySubmittedEventArgs e)
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

        private void AutoSuggestBox_SuggestionChosen(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            pkNationality.Text = e.SelectedItem.ToString();
        }
        #endregion
    }
}