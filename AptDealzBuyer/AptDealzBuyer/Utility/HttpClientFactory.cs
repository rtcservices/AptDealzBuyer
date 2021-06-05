using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AptDealzBuyer.Utility
{
    public class HttpClientFactory : IDisposable
    {
        public HttpClient client;

        public HttpClientFactory(string baseUrl = null, string mediaType = null, string token = null)
        {
            try
            {
                client = new HttpClient(new System.Net.Http.HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                    {
                        var auth = sender?.Headers?.Authorization;
                        return true;
                    },

                    MaxRequestContentBufferSize = 2097151,  // 2097151Kb = 2Gb.
                }, false);

                client.Timeout = new TimeSpan(0, 15, 0); // hr, min, ss

                if (baseUrl == null)
                {
                    baseUrl = App.Current.Resources["BaseURL"].ToString();
                    client.BaseAddress = new Uri(baseUrl);
                    //client.BaseAddress = new Uri(EndPointURL.ProductionBaseURL);
                }
                else
                {
                    client.BaseAddress = new Uri(baseUrl);
                }

                if (mediaType == null)
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                else
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
                }

                if (token != null)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("HttpClientFactory: " + ex.Message);
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string controllerName, string srlzRequest)
        {
            return await client.PostAsync(controllerName, new StringContent(srlzRequest, Encoding.UTF8, "application/json"));
        }

        public async Task<HttpResponseMessage> PutAsync(string controllerName, string srlzRequest)
        {
            return await client.PutAsync(controllerName, new StringContent(srlzRequest, Encoding.UTF8, "application/json"));
        }

        public async Task<HttpResponseMessage> DeleteAsync(string controllerName)
        {
            return await client.DeleteAsync(controllerName);
        }

        public async Task<HttpResponseMessage> GetAsync(string controllerName)
        {
            return await client.GetAsync(controllerName);
        }

        public void Dispose()
        {
            client = null;
            GC.Collect();
        }
    }
}
