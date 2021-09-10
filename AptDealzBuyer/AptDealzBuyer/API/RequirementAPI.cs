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
    public class RequirementAPI
    {
        #region [ GET ]
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

        public async Task<Response> GetAllMyActiveRequirements(string SortBy = "", string Title = "", bool? IsAscending = null, int PageNumber = 1, int PageSize = 10)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.GetAllMyActiveRequirements + "?PageNumber={1}&PageSize={2}", (int)App.Current.Resources["Version"], PageNumber, PageSize);

                        if (!Common.EmptyFiels(Title))
                            url += "&Title=" + Title;
                        if (!Common.EmptyFiels(SortBy))
                            url += "&SortBy=" + SortBy;
                        if (IsAscending.HasValue)
                            url += "&IsAscending=" + IsAscending.Value;

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
                        await GetAllMyActiveRequirements(SortBy, Title, IsAscending, PageNumber, PageSize);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementAPI/GetActiveRequirements: " + ex.Message);
            }
            return mResponse;
        }

        public async Task<Response> GetMyPreviousRequirements(string SortBy = "", string Title = "", bool? IsAscending = null, int PageNumber = 1, int PageSize = 10)
        {
            Response mResponse = new Response();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory(token: Common.Token))
                    {
                        string url = string.Format(EndPointURL.GetMyPreviousRequirements + "?PageNumber={1}&PageSize={2}", (int)App.Current.Resources["Version"], PageNumber, PageSize);

                        if (!Common.EmptyFiels(Title))
                            url += "&Title=" + Title;
                        if (!Common.EmptyFiels(SortBy))
                            url += "&SortBy=" + SortBy;
                        if (IsAscending.HasValue)
                            url += "&IsAscending=" + IsAscending.Value;

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
                        await GetMyPreviousRequirements(SortBy, Title, IsAscending, PageNumber, PageSize);
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
                        string url = string.Format(EndPointURL.CancelRequirement, (int)App.Current.Resources["Version"], requirementId);
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
