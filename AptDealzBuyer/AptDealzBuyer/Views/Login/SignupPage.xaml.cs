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
        private bool isEmail = false;
        #endregion

        #region Constructor
        public SignupPage()
        {
            InitializeComponent();
            txtFullName.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
            txtUserName.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeNone);
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
                else if (Common.EmptyFiels(txtUserName.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Email_Phone);
                }
                else if (txtUserName.Text.Contains("@") || txtUserName.Text.Contains("."))
                {
                    if (!txtUserName.Text.IsValidEmail())
                    {
                        Common.DisplayErrorMessage(Constraints.InValid_Email);
                    }
                    else
                    {
                        if (!isChecked)
                        {
                            Common.DisplayErrorMessage(Constraints.Agree_T_C);
                        }
                        else
                        {
                            isEmail = true;
                            result = true;
                        }
                    }
                }
                else if (!txtUserName.Text.IsValidPhone())
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
                mRegister.UserName = txtUserName.Text;
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
                    mUniquePhoneNumber.PhoneNumber = txtUserName.Text;

                    UniqueEmail mUniqueEmail = new UniqueEmail();
                    mUniqueEmail.Email = txtUserName.Text;

                    var mRegister = FillRegister();
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    Response mResponse = new Response();
                    if (isEmail)
                    {
                        mResponse = await registerAPI.IsUniqueEmail(mUniqueEmail);
                    }
                    else
                    {
                        mResponse = await registerAPI.IsUniquePhoneNumber(mUniquePhoneNumber);
                    }

                    if (mResponse != null && mResponse.Succeeded)
                    {
                        var result = (bool)mResponse.Data;
                        if (result)
                        {
                            mResponse = await registerAPI.Register(mRegister);
                            if (mResponse != null && mResponse.Succeeded)
                            {
                                var registerId = mResponse.Data;
                                Settings.UserId = registerId.ToString();
                                if (!isEmail)
                                {
                                    var isSent = await SendOTP(mRegister.UserName);
                                    if (isSent)
                                    {
                                        await Navigation.PushAsync(new Views.Login.EnterOtpPage(txtUserName.Text, false));
                                    }
                                }
                                else
                                {
                                    mResponse = await registerAPI.SendOtp(registerId.ToString());
                                    if (mResponse != null && mResponse.Succeeded)
                                    {
                                        Common.DisplaySuccessMessage(mResponse.Message);
                                        await Navigation.PushAsync(new Views.Login.EnterOtpPage(txtUserName.Text));
                                    }
                                    else
                                    {
                                        Common.DisplayErrorMessage(mResponse.Message);
                                        Navigation.PopAsync();
                                    }
                                }
                                txtFullName.Text = string.Empty;
                                txtUserName.Text = string.Empty;
                                isChecked = false;
                                imgCheck.Source = Constraints.CheckBox_UnChecked;
                            }
                            else
                            {
                                Common.DisplayErrorMessage(mResponse.Message);
                            }
                        }
                        else
                        {
                            Common.DisplayErrorMessage(mResponse.Message);
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
                Common.DisplayErrorMessage("SignupPage/RegisterUser: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        async Task<bool> SendOTP(string phoneNumber)
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