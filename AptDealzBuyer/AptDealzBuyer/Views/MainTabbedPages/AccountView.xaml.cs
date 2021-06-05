using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.MasterData;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountView : ContentView
    {
        #region Objects          
        ProfileAPI profileAPI;
        List<Country> countries;
        BuyerDetails mBuyerDetail;
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
                countries = App.countries;

                if (countries == null || countries.Count == 0)
                    await GetCountries();

                pkNationality.ItemsSource = countries.Select(x => x.Name).ToList();
                await GetProfile();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/BindProperties: " + ex.Message);
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
                var mResponse = await profileAPI.GetCountry((int)App.Current.Resources["PageNumber"], (int)App.Current.Resources["PageSize"]);
                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    countries = result.ToObject<List<Country>>();
                }
                else
                {
                    Common.DisplayErrorMessage(mResponse.Message);
                }
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
                        }
                    }
                }
                else
                {
                    Common.DisplayErrorMessage(mResponse.Message);
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
                    pkNationality.SelectedItem = countries.Where(x => x.CountryId == mBuyerDetails.CountryId).FirstOrDefault().Name;
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
            mBuyerDetail.ProfilePhoto = Common.RelativePath;
            mBuyerDetail.PhoneNumber = txtPhoneNumber.Text;

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
            if (pkNationality.SelectedIndex > -1)
            {
                mBuyerDetail.CountryId = countries.Where(x => x.Name == pkNationality.SelectedItem.ToString()).FirstOrDefault()?.CountryId;
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
                else if (pkNationality.SelectedIndex == -1)
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

        async void UplodeFile()
        {
            try
            {
                var base64String = Convert.ToBase64String(ImageConvertion.ProfileImageByte);
                var fileName = Guid.NewGuid().ToString() + ".png";

                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                FileUpload mFileUpload = new FileUpload();
                mFileUpload.FileUploadCategory = (int)Common.FileUploadCategory.ProfilePicture;
                mFileUpload.Base64String = base64String;
                mFileUpload.FileName = fileName;

                var mResponse = await profileAPI.FileUpload(mFileUpload);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        var mBuyerFile = jObject.ToObject<Model.Reponse.BuyerFileDocument>();
                        if (mBuyerFile != null)
                        {
                            Common.FileUri = mBuyerFile.fileUri;
                            Common.RelativePath = mBuyerFile.relativePath;

                            //Common.DisplaySuccessMessage(mResponse.Message);
                        }
                    }
                }
                else
                {
                    Common.DisplayErrorMessage(mResponse.Message);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/UplodeFile: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
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
        #endregion

        #region Events       
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(image: ImgMenu);
            Common.OpenMenu();
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
            Common.BindAnimation(image: ImgCamera);
            ImageConvertion.BuyerProfileImage = imgUser;
            await ImageConvertion.SelectImage();

            if (ImageConvertion.ProfileImageByte != null)
            {
                UplodeFile();
            }
        }
        #endregion

    }
}