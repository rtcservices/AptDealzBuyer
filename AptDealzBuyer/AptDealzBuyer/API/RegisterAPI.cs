using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.SplashScreen;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AptDealzBuyer.API
{
    public class RegisterAPI
    {
        #region [ POST ]
        public async Task<Response> IsUniquePhoneNumber(Model.Request.UniquePhoneNumber mUniquePhoneNumber)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var requestJson = JsonConvert.SerializeObject(mUniquePhoneNumber);
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = EndPointURL.IsUniquePhoneNumber;
                        var mResponseMessage = await hcf.PostAsync(url, requestJson);
                        var responseJson = await mResponseMessage.Content.ReadAsStringAsync();
                        if (mResponseMessage.IsSuccessStatusCode)
                        {
                            mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                        else if (mResponseMessage.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            var errorString = JsonConvert.DeserializeObject<string>(responseJson);
                            if (errorString == Constraints.Session_Expired)
                            {
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await IsUniquePhoneNumber(mUniquePhoneNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Errors = ex.Message;
                Common.DisplayErrorMessage("RegisterAPI/IsUniquePhoneNumber: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> IsUniqueEmail(Model.Request.UniqueEmail mUniqueEmail)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var requestJson = JsonConvert.SerializeObject(mUniqueEmail);
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = EndPointURL.IsUniqueEmail;
                        var mResponseMessage = await hcf.PostAsync(url, requestJson);
                        var responseJson = await mResponseMessage.Content.ReadAsStringAsync();
                        if (mResponseMessage.IsSuccessStatusCode)
                        {
                            mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                        else if (mResponseMessage.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            var errorString = JsonConvert.DeserializeObject<string>(responseJson);
                            if (errorString == Constraints.Session_Expired)
                            {
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await IsUniqueEmail(mUniqueEmail);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Errors = ex.Message;
                Common.DisplayErrorMessage("RegisterAPI/IsUniqueEmail: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> Register(Model.Request.Register mRequestRegister)
        {
            Response mResponseRegister = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var requestJson = JsonConvert.SerializeObject(mRequestRegister);
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.Register, (int)App.Current.Resources["Version"]);
                        var response = await hcf.PostAsync(url, requestJson);
                        var responseJson = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            mResponseRegister = JsonConvert.DeserializeObject<Response>(responseJson);
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
                            mResponseRegister = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await Register(mRequestRegister);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponseRegister.Succeeded = false;
                mResponseRegister.Errors = ex.Message;
                Common.DisplayErrorMessage("RegisterAPI/Register: " + ex.Message);
            }
            return mResponseRegister;
        }

        public async Task<Response> SendOtp(string userId)
        {
            Response mResponseLogin = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var requestJson = JsonConvert.SerializeObject(userId);
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.SendOtp, userId);
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
                        await SendOtp(userId);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponseLogin.Succeeded = false;
                mResponseLogin.Errors = ex.Message;
                Common.DisplayErrorMessage("RegisterAPI/SendOtp: " + ex.Message);
            }
            return mResponseLogin;
        }
        #endregion
    }
}
