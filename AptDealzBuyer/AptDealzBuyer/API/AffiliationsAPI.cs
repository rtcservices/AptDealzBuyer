using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace AptDealzBuyer.API
{
    public class AffiliationsAPI
    {
        #region [ GET ]
        public async Task<Response> GetAllAffiliations()
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.GetAllAffiliations, (int)App.Current.Resources["Version"]);
                        var response = await hcf.GetAsync(url);
                        var responseJson = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                        {
                            var errorString = JsonConvert.DeserializeObject<string>(responseJson);
                            if (errorString == Constraints.Session_Expired)
                            {
                                Common.DisplayErrorMessage(Constraints.Session_Expired);
                                App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
                            }
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                        {
                            Common.DisplayErrorMessage(Constraints.ServiceUnavailable);
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        {
                            Common.DisplayErrorMessage(Constraints.Something_Wrong_Server);
                        }
                        else
                        {
                            if (responseJson.Contains(Constraints.Str_TokenExpired) || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                            {
                                var isRefresh = await DependencyService.Get<IAuthenticationRepository>().RefreshToken();
                                if (!isRefresh)
                                {
                                    Common.DisplayErrorMessage(Constraints.Session_Expired);
                                    App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
                                }
                            }
                            else
                            {
                                mResponse = JsonConvert.DeserializeObject<Response>(responseJson);
                            }
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await GetAllAffiliations();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AffiliationsAPI/GetAllAffiliations: " + ex.Message);
            }
            return mResponse;
        }
        #endregion
    }
}
