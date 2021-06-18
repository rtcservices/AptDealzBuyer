using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Interfaces;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.MasterData;
using AptDealzBuyer.Views.SplashScreen;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterOtpPage : ContentPage
    {
        #region Objects
        private string UserAuth;
        private bool isEmail;
        private bool IsKeepLogin;
        private bool IsRegister;
        private string OTPString;
        private Register mRegister;
        #endregion

        #region Constructor
        public EnterOtpPage(string UserAuth, bool isEmail = true, bool IsKeepLogin = false)
        {
            InitializeComponent();
            this.UserAuth = UserAuth;
            this.isEmail = isEmail;
            this.IsKeepLogin = IsKeepLogin;
            ResendButtonEnable();
        }

        void ResendButtonEnable()
        {
            BtnResentOtp.IsEnabled = false;
            int i = 120;

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                BtnResentOtp.Text = i + " sec";
                if (i == 0)
                {
                    BtnResentOtp.IsEnabled = true;
                    BtnResentOtp.Text = "Resend OTP";
                    return false;
                }
                i--;
                return true;
            });
        }

        public EnterOtpPage(Register register)
        {
            InitializeComponent();
            mRegister = register;
            this.IsRegister = true;
        }
        #endregion

        #region Methods        
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        bool Validations()
        {
            bool isValid = false;
            try
            {
                if (Common.EmptyFiels(TxtOtpOne.Text)
                  || Common.EmptyFiels(TxtOtpTwo.Text)
                  || Common.EmptyFiels(TxtOtpThree.Text)
                  || Common.EmptyFiels(TxtOtpFour.Text)
                  || Common.EmptyFiels(TxtOtpFive.Text)
                  || Common.EmptyFiels(TxtOtpSix.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_VerificationCode);
                }
                else
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("EnterOtpPage/Validations: " + ex.Message);
            }
            return isValid;
        }

        async void RegisterBuyer()
        {
            try
            {
                mRegister.FirebaseVerificationId = Settings.PhoneAuthToken;
                RegisterAPI registerAPI = new RegisterAPI();
                var mResponse = await registerAPI.Register(mRegister);
                if (mResponse != null && mResponse.Succeeded)
                {
                    App.Current.MainPage = new NavigationPage(new WelcomePage(true));
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
                Common.DisplayErrorMessage("EnterOtpPage/Validations: " + ex.Message);
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

                            if (this.IsKeepLogin)
                            {
                                Settings.UserToken = mBuyer.JwToken;
                            }
                            else
                            {
                                Settings.UserToken = string.Empty;
                            }

                            App.Current.MainPage = new MasterDataPage();
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
                mLogin.PhoneNumber = UserAuth;
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

        async void SubmitOTP()
        {
            try
            {
                if (Validations())
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);

                    AuthenticationAPI authenticationAPI = new AuthenticationAPI();
                    AuthenticateEmail mAuthenticateEmail = new AuthenticateEmail();
                    mAuthenticateEmail.Email = UserAuth;

                    OTPString = TxtOtpOne.Text + TxtOtpTwo.Text + TxtOtpThree.Text + TxtOtpFour.Text + TxtOtpFive.Text + TxtOtpSix.Text;
                    mAuthenticateEmail.Otp = OTPString;

                    Response mResponse = new Response();
                    if (!this.isEmail)
                    {
                        var token = await Xamarin.Forms.DependencyService.Get<IFirebaseAuthenticator>().VerifyOtpCodeAsync(OTPString);
                        if (!Common.EmptyFiels(token))
                        {
                            Settings.PhoneAuthToken = token;
                            if (IsRegister)
                            {
                                RegisterBuyer();
                            }
                            else
                            {
                                var mLogin = FillLogin();
                                mResponse = await authenticationAPI.BuyerAuthPhone(mLogin);
                                NavigateToDashboard(mResponse);
                            }
                        }
                    }
                    else
                    {
                        if (IsRegister)
                        {
                            RegisterBuyer();
                        }
                        else
                        {
                            mResponse = await authenticationAPI.BuyerAuthEmail(mAuthenticateEmail);
                            NavigateToDashboard(mResponse);
                        }
                    }
                }
                else
                {
                    Common.DisplayErrorMessage(Constraints.Required_VerificationCode);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("EnterOtpPage/FrmSubmit_Tapped: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        async void ResentOTP()
        {
            try
            {
                AuthenticationAPI authenticationAPI = new AuthenticationAPI();
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                if (this.isEmail)
                {
                    var mResponse = await authenticationAPI.SendOtpByEmail(UserAuth);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        Common.DisplaySuccessMessage(mResponse.Message);
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
                    var result = await Xamarin.Forms.DependencyService.Get<IFirebaseAuthenticator>().SendOtpCodeAsync(UserAuth);
                    if (!result)
                    {
                        Common.DisplayErrorMessage("Could not send Verification Code to the given number!");
                    }
                }

                ResendButtonEnable();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("EnterOtpPage/BtnResentOtp_Tapped: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion

        #region Events
        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void FrmSubmit_Tapped(object sender, EventArgs e)
        {
            SubmitOTP();
        }

        private void BtnResentOtp_Tapped(object sender, EventArgs e)
        {
            ResentOTP();
        }

        private void TxtOtpOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(TxtOtpOne.Text))
                TxtOtpTwo.Focus();
        }

        private void TxtOtpTwo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(TxtOtpTwo.Text))
                TxtOtpThree.Focus();
            else
                TxtOtpOne.Focus();
        }

        private void TxtOtpThree_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(TxtOtpThree.Text))
                TxtOtpFour.Focus();
            else
                TxtOtpTwo.Focus();
        }

        private void TxtOtpFour_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(TxtOtpFour.Text))
                TxtOtpFive.Focus();
            else
                TxtOtpThree.Focus();
        }

        private void TxtOtpFive_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(TxtOtpFive.Text))
                TxtOtpSix.Focus();
            else
                TxtOtpFour.Focus();
        }

        private void TxtOtpSix_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(TxtOtpSix.Text))
            {
                TxtOtpSix.Unfocus();
                frmSubmit.BackgroundColor = (Color)App.Current.Resources["Green"];
            }
            else
                TxtOtpFive.Focus();
        }
        #endregion
    }
}