using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Interfaces;
using AptDealzBuyer.Utility;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        #region Objects
        private bool isChecked = false;
        private bool isEmail = false;
        #endregion

        #region Constructor
        public LoginPage()
        {
            InitializeComponent();
            txtUserAuth.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeNone);
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindProperties();
        }

        void BindProperties()
        {
            try
            {
                if (isChecked)
                    imgCheck.Source = Constraints.CheckBox_Checked;
                else
                    imgCheck.Source = Constraints.CheckBox_UnChecked;

                if (!Common.EmptyFiels(Settings.EmailAddress))
                    txtUserAuth.Text = Settings.EmailAddress;
            }
            catch (Exception ex)
            {

            }
        }

        bool Validations()
        {
            bool result = false;
            try
            {
                if (Common.EmptyFiels(txtUserAuth.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Email_Phone);
                }
                else if (txtUserAuth.Text.Contains("@") || txtUserAuth.Text.Contains("."))
                {
                    if (!txtUserAuth.Text.IsValidEmail())
                    {
                        Common.DisplayErrorMessage(Constraints.InValid_Email);
                    }
                    else
                    {
                        isEmail = true;
                        result = true;
                    }
                }
                else if (!txtUserAuth.Text.IsValidPhone())
                {
                    Common.DisplayErrorMessage(Constraints.InValid_PhoneNumber);
                }
                else
                {
                    isEmail = false;
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/Validations: " + ex.Message);
            }
            return result;
        }

        async void LoginUserByEmail()
        {
            try
            {
                if (Validations())
                {
                    AuthenticationAPI authenticationAPI = new AuthenticationAPI();
                    if (isEmail)
                    {
                        UserDialogs.Instance.ShowLoading(Constraints.Loading);
                        var mResponse = await authenticationAPI.SendOtpByEmail(txtUserAuth.Text);
                        if (mResponse != null && mResponse.Succeeded)
                        {
                            if (isChecked)
                            {
                                Settings.EmailAddress = txtUserAuth.Text;
                            }

                            Common.DisplaySuccessMessage(mResponse.Message);

                            await Navigation.PushAsync(new Views.Login.EnterOtpPage(txtUserAuth.Text, IsKeepLogin: isChecked));

                            txtUserAuth.Text = string.Empty;
                            isChecked = false;
                            imgCheck.Source = Constraints.CheckBox_UnChecked;

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
                        var result = await Xamarin.Forms.DependencyService.Get<IFirebaseAuthenticator>().SendOtpCodeAsync(txtUserAuth.Text);

                        if (result)
                        {
                            await Navigation.PushAsync(new Views.Login.EnterOtpPage(txtUserAuth.Text, false, isChecked));
                            txtUserAuth.Text = string.Empty;
                            isChecked = false;
                            imgCheck.Source = Constraints.CheckBox_UnChecked;
                        }
                        else
                        {
                            Common.DisplayErrorMessage("Could not send Verification Code to the given number!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/FrmGetOtp_Tapped: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        async void SendOTP(string phoneNumber)
        {
            var result = await Xamarin.Forms.DependencyService.Get<IFirebaseAuthenticator>().SendOtpCodeAsync(phoneNumber);
            if (!result)
            {
                var isTryAgain = await UserDialogs.Instance.ConfirmAsync("Could not send Verification Code to the given number", "Verification", "Try Again", "Goto Login");
                if (isTryAgain)
                    SendOTP(phoneNumber);
            }
            else
            {
                await DisplayAlert("OTP", "OTP send to your Phone number " + phoneNumber, "Ok");
            }
        }
        #endregion

        #region Events
        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SplashScreen.WelcomePage());
        }

        private void StkRemember_Tapped(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/StkRemember_Tapped: " + ex.Message);
            }
        }

        private void StkSignup_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Login.SignupPage());
        }

        private void FrmGetOtp_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(frame: FrmGetOtp);
            //LoginUser();
            LoginUserByEmail();
        }

        #endregion
    }
}