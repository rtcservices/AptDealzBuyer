using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Interfaces;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Utility;
using System;
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
            //txtEmailAddress.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeNone);
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
            bool result = false;
            try
            {
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
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SignupPage/Validations: " + ex.Message);
            }
            return result;
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
                                var isSent = await SendOTP(mRegister.PhoneNumber);
                                if (isSent)
                                {
                                    var _verificationId = Xamarin.Forms.DependencyService.Get<IFirebaseAuthenticator>()._verificationId;
                                    await Navigation.PushAsync(new Views.Login.EnterOtpPage(mRegister));
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

        async Task<bool> SendOTP(string phoneNumber)
        {
            try
            {
                var result = await Xamarin.Forms.DependencyService.Get<IFirebaseAuthenticator>().SendOtpCodeAsync(phoneNumber);
                if (!result)
                {
                    var isTryAgain = await UserDialogs.Instance.ConfirmAsync("Could not send Verification Code to the given number", "Verification", "Try Again", "Goto Login");
                    if (isTryAgain)
                        return await SendOTP(phoneNumber);
                    else
                        return false;
                }
                else
                {
                    await DisplayAlert("OTP", "OTP send to your Phone number " + phoneNumber, "Ok");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SignupPage/SendOTP: " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Events
        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void FrmGetOtp_Tapped(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(frame: FrmGetOtp);
                RegisterUser();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SignupPage/GetOtp_Tapped: " + ex.Message);
            }
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
        #endregion      
    }
}