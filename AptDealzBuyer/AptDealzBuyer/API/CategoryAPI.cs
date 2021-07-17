using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Utility;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AptDealzBuyer.API
{
    public class CategoryAPI
    {
        #region [ GET ]
        public async Task<List<Category>> GetCategory()
        {
            List<Category> mCategory = new List<Category>();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.Category, (int)App.Current.Resources["Version"]);
                        var response = await hcf.GetAsync(url);
                        var responseJson = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            mCategory = JsonConvert.DeserializeObject<List<Category>>(responseJson);
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
                            mCategory = null;
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await GetCategory();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CategoryAPI/GetCategory: " + ex.Message);
            }
            return mCategory;
        }

        public async Task<List<SubCategory>> GetSubCategory(string categoryId)
        {
            List<SubCategory> mSubCategory = new List<SubCategory>();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var hcf = new HttpClientFactory())
                    {
                        string url = string.Format(EndPointURL.SubCategory, (int)App.Current.Resources["Version"], categoryId);
                        var response = await hcf.GetAsync(url);
                        var responseJson = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            mSubCategory = JsonConvert.DeserializeObject<List<SubCategory>>(responseJson);
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
                            mSubCategory = null;
                        }
                    }
                }
                else
                {
                    if (await Common.InternetConnection())
                    {
                        await GetSubCategory(categoryId);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CategoryAPI/GetSubCategory: " + ex.Message);
            }
            return mSubCategory;
        }
        #endregion
    }
}
