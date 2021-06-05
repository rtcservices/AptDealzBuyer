using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Interfaces;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.MasterData;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterOtpPage : ContentPage
    {
        #region Objects
        private string emailAddress;
        private bool isEmail;
        private bool IsKeepLogin;
        #endregion

        #region Constructor
        public EnterOtpPage(string EmailAddress, bool isEmail = true, bool IsKeepLogin = false)
        {
            InitializeComponent();
            emailAddress = EmailAddress;
            this.isEmail = isEmail;
            this.IsKeepLogin = IsKeepLogin;
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

        void NavigateToDashboard(Response mResponse)
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
                        Common.Token = mBuyer.JwToken;
                        Common.RefreshToken = mBuyer.RefreshToken;

                        if (this.IsKeepLogin)
                            Settings.UserToken = mBuyer.JwToken;
                        App.Current.MainPage = new MasterDataPage();
                    }
                }
            }
            else
            {
                Common.DisplayErrorMessage(mResponse.Message);
            }
        }

        Model.Request.Login FillLogin()
        {
            Model.Request.Login mLogin = new Model.Request.Login();
            try
            {
                mLogin.PhoneNumber = emailAddress;
                if (Common.EmptyFiels(Settings.fcm_token) && Common.EmptyFiels(Settings.firebaseVerificationId))
                {
                    mLogin.FcmToken = Settings.fcm_token;
                    mLogin.FirebaseVerificationId = Settings.firebaseVerificationId;
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

        private async void FrmSubmit_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (Validations())
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);

                    AuthenticationAPI authenticationAPI = new AuthenticationAPI();
                    AuthenticateEmail mAuthenticateEmail = new AuthenticateEmail();
                    mAuthenticateEmail.Email = emailAddress;
                    var OTPString = TxtOtpOne.Text + TxtOtpTwo.Text + TxtOtpThree.Text + TxtOtpFour.Text + TxtOtpFive.Text + TxtOtpSix.Text;
                    mAuthenticateEmail.Otp = OTPString;
                    if (!this.isEmail)
                    {
                        var token = await Xamarin.Forms.DependencyService.Get<IFirebaseAuthenticator>().VerifyOtpCodeAsync(OTPString);
                        if (!Common.EmptyFiels(token))
                        {
                            var mLogin = FillLogin();
                            var mResponse = await authenticationAPI.BuyerAuthPhone(mLogin);
                            NavigateToDashboard(mResponse);
                        }
                    }
                    else
                    {
                        var mResponse = await authenticationAPI.BuyerAuthEmail(mAuthenticateEmail);
                        NavigateToDashboard(mResponse);
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

        private async void BtnResentOtp_Tapped(object sender, EventArgs e)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);

                AuthenticationAPI authenticationAPI = new AuthenticationAPI();
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await authenticationAPI.SendOtpByEmail(emailAddress);
                if (mResponse != null && mResponse.Succeeded)
                {
                    Common.DisplaySuccessMessage(mResponse.Message);
                }
                else
                {
                    Common.DisplayErrorMessage(mResponse.Message);
                }

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