using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.SplashScreen;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AptDealzBuyer.API
{
    public class AuthenticationAPI
    {
        #region [POST]
        public async Task<Response> BuyerAuthPhone(Model.Request.Login mRequestLogin)
        {
            Response mResponseLogin = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var requestJson = JsonConvert.SerializeObject(mRequestLogin);
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.BuyerAuthenticatePhone, (int)App.Current.Resources["Version"]);
                        var response = await hcf.PostAsync(url, requestJson);
                        var responseJson = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            mResponseLogin = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            var errorString = JsonConvert.DeserializeObject<string>(responseJson);
                            if (errorString == Constraints.Session_Expired)
                            {
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            mResponseLogin = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await BuyerAuthPhone(mRequestLogin);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponseLogin.Succeeded = false;
                mResponseLogin.Errors = ex.Message;
                Common.DisplayErrorMessage("AuthenticationAPI/BuyerAuthPhone: " + ex.Message);
            }
            return mResponseLogin;
        }

        public async Task<Response> BuyerAuthEmail(AuthenticateEmail mAuthenticateEmail)
        {
            Response mResponseLogin = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var requestJson = JsonConvert.SerializeObject(mAuthenticateEmail);
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.BuyerAuthenticateEmail, (int)App.Current.Resources["Version"]);
                        var response = await hcf.PostAsync(url, requestJson);
                        var responseJson = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            mResponseLogin = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            var errorString = JsonConvert.DeserializeObject<string>(responseJson);
                            if (errorString == Constraints.Session_Expired)
                            {
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            mResponseLogin = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await BuyerAuthEmail(mAuthenticateEmail);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponseLogin.Succeeded = false;
                mResponseLogin.Errors = ex.Message;
                Common.DisplayErrorMessage("AuthenticationAPI/BuyerAuthEmail: " + ex.Message);
            }
            return mResponseLogin;
        }

        public async Task<Response> SendOtpByEmail(string email)
        {
            Response mResponseLogin = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = "{\"email\":\"" + email + "\"}";
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.SendOtpByEmail);
                        var response = await hcf.PostAsync(url, requestJson);
                        var responseJson = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            mResponseLogin = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            var errorString = JsonConvert.DeserializeObject<string>(responseJson);
                            if (errorString == Constraints.Session_Expired)
                            {
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            mResponseLogin = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await SendOtpByEmail(email);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponseLogin.Succeeded = false;
                mResponseLogin.Errors = ex.Message;
                Common.DisplayErrorMessage("AuthenticationAPI/SendOtpByEmail: " + ex.Message);
            }
            return mResponseLogin;
        }
        #endregion
    }
}
