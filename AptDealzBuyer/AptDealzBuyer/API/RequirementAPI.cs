using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.SplashScreen;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Polly;
using System;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AptDealzBuyer.API
{
    public class RequirementAPI
    {
        #region [ GET ]
        public async Task<Response> GetRequirement(int PageNumber, int PageSize)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.GetRequirement, (int)App.Current.Resources["Version"], PageNumber, PageSize);
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
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            if (responseJson.Contains("TokenExpired"))
                            {
                                Common.DisplayErrorMessage(Constraints.Session_Expired);
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
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
                        await GetRequirement(PageNumber, PageSize);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementAPI/GetRequirement: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> GetRequirementById(string RequirmentId)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.GetRequirementById, (int)App.Current.Resources["Version"], RequirmentId);
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
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            if (responseJson.Contains("TokenExpired"))
                            {
                                Common.DisplayErrorMessage(Constraints.Session_Expired);
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
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
                        await GetRequirementById(RequirmentId);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementAPI/GetRequirementById: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> GetAllMyActiveRequirements(string SortBy = "", bool? IsAscending = null, int PageNumber = 1, int PageSize = 10)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = "";
                        if (!Common.EmptyFiels(SortBy) && IsAscending != null)
                            url = string.Format(EndPointURL.GetAllMyActiveRequirements + "?SortBy={1}&IsAscending={2}&PageNumber={3}&PageSize={4}", (int)App.Current.Resources["Version"], SortBy, IsAscending, PageNumber, PageSize);
                        else if (!Common.EmptyFiels(SortBy) && IsAscending == null)
                            url = string.Format(EndPointURL.GetAllMyActiveRequirements + "?SortBy={1}&PageNumber={2}&PageSize={3}", (int)App.Current.Resources["Version"], SortBy, PageNumber, PageSize);
                        else if (Common.EmptyFiels(SortBy) && IsAscending != null)
                            url = string.Format(EndPointURL.GetAllMyActiveRequirements + "?IsAscending={1}&PageNumber={2}&PageSize={3}", (int)App.Current.Resources["Version"], IsAscending, PageNumber, PageSize);
                        else
                            url = string.Format(EndPointURL.GetAllMyActiveRequirements + "?PageNumber={1}&PageSize={2}", (int)App.Current.Resources["Version"], PageNumber, PageSize);

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
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            if (responseJson.Contains("TokenExpired"))
                            {
                                Common.DisplayErrorMessage(Constraints.Session_Expired);
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
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
                        await GetAllMyActiveRequirements(RequirementSortBy.ID.ToString(), null, PageNumber, PageSize);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementAPI/GetActiveRequirements: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> GetMyPreviousRequirements(string SortBy = "", bool? IsAscending = null, int PageNumber = 1, int PageSize = 10)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = "";
                        if (!Common.EmptyFiels(SortBy) && IsAscending != null)
                            url = string.Format(EndPointURL.GetMyPreviousRequirements + "?SortBy={1}&IsAscending={2}&PageNumber={3}&PageSize={4}", (int)App.Current.Resources["Version"], SortBy, IsAscending, PageNumber, PageSize);
                        else if (!Common.EmptyFiels(SortBy) && IsAscending == null)
                            url = string.Format(EndPointURL.GetMyPreviousRequirements + "?SortBy={1}&PageNumber={2}&PageSize={3}", (int)App.Current.Resources["Version"], SortBy, PageNumber, PageSize);
                        else if (Common.EmptyFiels(SortBy) && IsAscending != null)
                            url = string.Format(EndPointURL.GetMyPreviousRequirements + "?IsAscending={1}&PageNumber={2}&PageSize={3}", (int)App.Current.Resources["Version"], IsAscending, PageNumber, PageSize);
                        else
                            url = string.Format(EndPointURL.GetMyPreviousRequirements + "?PageNumber={1}&PageSize={2}", (int)App.Current.Resources["Version"], PageNumber, PageSize);

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
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            if (responseJson.Contains("TokenExpired"))
                            {
                                Common.DisplayErrorMessage(Constraints.Session_Expired);
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
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
                        await GetMyPreviousRequirements("", null, PageNumber, PageSize);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementAPI/GetPreviousRequirements: " + ex.Message);
            }
            return mResponse;
        }
        #endregion

        #region [ POST ]
        public async Task<Response> CreateRequirement(Model.Request.Requirement mRequirement)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var requestJson = JsonConvert.SerializeObject(mRequirement);
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.CreateRequirement, (int)App.Current.Resources["Version"]);
                        var response = await hcf.PostAsync(url, requestJson);
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
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            if (responseJson.Contains("TokenExpired"))
                            {
                                Common.DisplayErrorMessage(Constraints.Session_Expired);
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
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
                        await CreateRequirement(mRequirement);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Errors = ex.Message;
                Common.DisplayErrorMessage("RequirementAPI/CreateRequirement: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> CancelRequirement(string requirementId)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = "{\"requirementId\":\"" + requirementId + "\"}";
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.CancelRequirement, (int)App.Current.Resources["Version"]);
                        var response = await hcf.PostAsync(url, requestJson);
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
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            if (responseJson.Contains("TokenExpired"))
                            {
                                Common.DisplayErrorMessage(Constraints.Session_Expired);
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
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
                        await CancelRequirement(requirementId);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Errors = ex.Message;
                Common.DisplayErrorMessage("RequirementAPI/CancelRequirement: " + ex.Message);
            }
            return mResponse;
        }
        #endregion

        #region [ DELETE ]
        public async Task<Response> DeleteRequirement(string id)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.DeleteRequirement, (int)App.Current.Resources["Version"], id);
                        var response = await hcf.DeleteAsync(url);
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
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            if (responseJson.Contains("TokenExpired"))
                            {
                                Common.DisplayErrorMessage(Constraints.Session_Expired);
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
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
                        await DeleteRequirement(id);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Errors = ex.Message;
                Common.DisplayErrorMessage("RequirementAPI/DeleteRequirement: " + ex.Message);
            }
            return mResponse;
        }
        #endregion

        #region [ PUT ]
        public async Task<Response> UpdateStatusRequirement(string requirementId, int status)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    string requestJson = "{\"requirementId\":\"" + requirementId + "\",\"status\":\"" + status + "\"}";
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.UpdateStatusRequirement, (int)App.Current.Resources["Version"]);
                        var response = await hcf.PutAsync(url, requestJson);
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
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
                            }
                        }
                        else
                        {
                            if (responseJson.Contains("TokenExpired"))
                            {
                                Common.DisplayErrorMessage(Constraints.Session_Expired);
                                App.Current.MainPage = new NavigationPage(new WelcomePage(true));
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
                        await UpdateStatusRequirement(requirementId, status);
                    }
                }
            }
            catch (Exception ex)
            {
                mResponse.Succeeded = false;
                mResponse.Errors = ex.Message;
                Common.DisplayErrorMessage("RequirementAPI/UpdateStatusRequirement: " + ex.Message);
            }
            return mResponse;
        }
        #endregion
    }
}
