using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Interfaces;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPage
    {
        #region Objects
        private bool isChecked = false;
        #endregion

        #region Constructor
        public SignupPage()
        {
            InitializeComponent();
            txtFullName.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (isChecked)
                imgCheck.Source = Constraints.CheckBox_Checked;
            else
                imgCheck.Source = Constraints.CheckBox_UnChecked;
        }

        bool Validations()
        {
            bool isValid = false;
            try
            {
                if (Common.EmptyFiels(txtFullName.Text) || Common.EmptyFiels(txtEmailAddress.Text) || Common.EmptyFiels(txtPhoneNumber.Text))
                {
                    RequiredFields();
                    isValid = false;
                }

                if (Common.EmptyFiels(txtFullName.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_FullName);
                }
                else if (Common.EmptyFiels(txtEmailAddress.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Email);
                }
                else if (!txtEmailAddress.Text.IsValidEmail())
                {
                    Common.DisplayErrorMessage(Constraints.InValid_Email);
                }
                else if (Common.EmptyFiels(txtPhoneNumber.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_PhoneNumber);
                }
                else if (!txtPhoneNumber.Text.IsValidPhone())
                {
                    Common.DisplayErrorMessage(Constraints.InValid_PhoneNumber);
                }
                else if (!isChecked)
                {
                    Common.DisplayErrorMessage(Constraints.Agree_T_C);
                }
                else
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SignupPage/Validations: " + ex.Message);
            }
            return isValid;
        }

        void RequiredFields()
        {
            try
            {
                Common.DisplayErrorMessage(Constraints.Required_All);
                if (Common.EmptyFiels(txtFullName.Text))
                {
                    BoxFullName.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtEmailAddress.Text))
                {
                    BoxEmailAddress.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtPhoneNumber.Text))
                {
                    BoxPhoneNumber.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SignupPage/RequiredFields: " + ex.Message);
            }
        }

        void FieldsTrim()
        {
            try
            {
                txtFullName.Text = txtFullName.Text.Trim();
                txtEmailAddress.Text = txtEmailAddress.Text.Trim();
                txtPhoneNumber.Text = txtPhoneNumber.Text.Trim();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SignupPage/FieldsTrim: " + ex.Message);
            }
        }

        Model.Request.Register FillRegister()
        {
            Register mRegister = new Register();
            try
            {
                mRegister.FullName = txtFullName.Text;
                mRegister.Email = txtEmailAddress.Text;
                mRegister.PhoneNumber = txtPhoneNumber.Text;
                mRegister.FirebaseVerificationId = Settings.firebaseVerificationId;
                mRegister.Latitude = App.latitude;
                mRegister.Longitude = App.longitude;
            }
            catch (Exception)
            {
                return null;
            }

            return mRegister;
        }

        async void RegisterUser()
        {
            try
            {
                if (Validations())
                {
                    FieldsTrim();
                    RegisterAPI registerAPI = new RegisterAPI();
                    UniquePhoneNumber mUniquePhoneNumber = new UniquePhoneNumber();
                    mUniquePhoneNumber.PhoneNumber = txtPhoneNumber.Text;

                    UniqueEmail mUniqueEmail = new UniqueEmail();
                    mUniqueEmail.Email = txtEmailAddress.Text;

                    var mRegister = FillRegister();
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    Response mResponse = new Response();
                    mResponse = await registerAPI.IsUniqueEmail(mUniqueEmail);

                    if (mResponse != null && mResponse.Succeeded)
                    {
                        var UniqueEmail = (bool)mResponse.Data;
                        if (UniqueEmail)
                        {
                            mResponse = await registerAPI.IsUniquePhoneNumber(mUniquePhoneNumber);
                            var UniquePhone = (bool)mResponse.Data;
                            if (UniquePhone)
                            {
                                var keyValuePairs = await SendOTP(mRegister.PhoneNumber);
                                var keyValue = keyValuePairs.FirstOrDefault();
                                if (keyValue.Key)
                                {
                                    if (keyValue.Value == Constraints.OTPSent)
                                    {
                                        var _verificationId = Xamarin.Forms.DependencyService.Get<IFirebaseAuthenticator>()._verificationId;
                                        await Navigation.PushAsync(new Views.Login.EnterOtpPage(mRegister));
                                    }
                                    else
                                    {
                                        //Common.DisplayWarningMessage(keyValue.Value);
                                        Settings.firebaseVerificationId = keyValue.Value;
                                        Settings.PhoneAuthToken = keyValue.Value;

                                        mRegister.FirebaseVerificationId = Settings.PhoneAuthToken;
                                        mResponse = await registerAPI.Register(mRegister);
                                        if (mResponse != null && mResponse.Succeeded)
                                        {
                                            App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
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
                                else if (!string.IsNullOrEmpty(keyValue.Value))
                                {
                                    Common.DisplayErrorMessage(keyValue.Value);
                                }
                                else
                                {
                                    Common.DisplayErrorMessage(Constraints.CouldNotSentOTP);
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
                            if (mResponse != null)
                                Common.DisplayErrorMessage(mResponse.Message);
                            else
                                Common.DisplayErrorMessage(Constraints.Something_Wrong);
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
                Common.DisplayErrorMessage("SignupPage/RegisterUser: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }



        async Task<Dictionary<bool, string>> SendOTP(string phoneNumber)
        {
            Dictionary<bool, string> keyValuePairs = new Dictionary<bool, string>();
            try
            {
                keyValuePairs = await Xamarin.Forms.DependencyService.Get<IFirebaseAuthenticator>().SendOtpCodeAsync(phoneNumber);
                var keyValue = keyValuePairs.FirstOrDefault();

                if (!keyValue.Key)
                {
                    var isTryAgain = await UserDialogs.Instance.ConfirmAsync(Constraints.CouldNotSentOTP, "Verification", "Try Again", "Goto Login");
                    if (isTryAgain)
                        return await SendOTP(phoneNumber);
                    else
                        return keyValuePairs;
                }
                else
                {
                    //await DisplayAlert("OTP", "OTP send to your Phone number " + phoneNumber, "Ok");
                    return keyValuePairs;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SignupPage/SendOTP: " + ex.Message);
                return keyValuePairs;
            }
        }

        void NavigateToDashboard(Response mResponse)
        {
            try
            {
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        var mBuyer = jObject.ToObject<Model.Request.Buyer>();
                        if (mBuyer != null)
                        {
                            Settings.UserId = mBuyer.Id;
                            Settings.RefreshToken = mBuyer.RefreshToken;
                            Settings.LoginTrackingKey = mBuyer.LoginTrackingKey == "00000000-0000-0000-0000-000000000000" ? Settings.LoginTrackingKey : mBuyer.LoginTrackingKey;
                            Common.Token = mBuyer.JwToken;

                            App.Current.MainPage = new MasterData.MasterDataPage();
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
                Common.DisplayErrorMessage("EnterOtpPage/NavigateToDashboard: " + ex.Message);
            }
        }

        Model.Request.Login FillLogin()
        {
            Model.Request.Login mLogin = new Model.Request.Login();
            try
            {
                mLogin.PhoneNumber = txtPhoneNumber.Text;
                if (!Common.EmptyFiels(Settings.fcm_token))
                {
                    mLogin.FcmToken = Settings.fcm_token;
                }
                if (!Common.EmptyFiels(Settings.firebaseVerificationId))
                {
                    mLogin.FirebaseVerificationId = Settings.PhoneAuthToken;
                }
                else
                {
                    Common.DisplayErrorMessage(Constraints.Something_Wrong);
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
            return mLogin;
        }
        #endregion

        #region Events
        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void StkLogin_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void StkAgreeTC_Tapped(object sender, EventArgs e)
        {
            if (imgCheck.Source.ToString().Replace("File: ", "") == Constraints.CheckBox_Checked)
            {
                isChecked = false;
                imgCheck.Source = Constraints.CheckBox_UnChecked;
            }
            else
            {
                isChecked = true;
                imgCheck.Source = Constraints.CheckBox_Checked;
            }
        }

        private void BtnGetOtp_Clicked(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(button: BtnGetOtp);
                RegisterUser();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SignupPage/GetOtp_Tapped: " + ex.Message);
            }
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            try
            {
                var entry = (Extention.ExtEntry)sender;
                if (!Common.EmptyFiels(entry.Text))
                {
                    if (entry.ClassId == "FullName")
                    {
                        BoxFullName.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "Email")
                    {
                        BoxEmailAddress.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "PhoneNumber")
                    {
                        BoxPhoneNumber.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SignupPage/Entry_Unfocused: " + ex.Message);
            }
        }
        #endregion
    }
}