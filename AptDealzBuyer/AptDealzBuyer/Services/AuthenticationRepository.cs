using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
using System.Threading.Tasks;

namespace AptDealzBuyer.Services
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        public async Task<bool> RefreshToken()
        {
            bool result = false;

            AuthenticationAPI authenticationAPI = new AuthenticationAPI();
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await authenticationAPI.RefreshToken(Settings.RefreshToken);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        var mBuyer = jObject.ToObject<Model.Request.Buyer>();
                        if (mBuyer != null)
                        {
                            Settings.UserId = mBuyer.Id;
                            Settings.UserToken = mBuyer.JwToken;
                            Common.Token = mBuyer.JwToken;
                            Settings.RefreshToken = mBuyer.RefreshToken;
                            Settings.LoginTrackingKey = mBuyer.LoginTrackingKey == "00000000-0000-0000-0000-000000000000" ? Settings.LoginTrackingKey : mBuyer.LoginTrackingKey;

                            result = true;
                        }
                    }
                }
                else
                {
                    if (mResponse != null)
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);

                    //App.Current.MainPage = new NavigationPage(new Views.SplashScreen.WelcomePage());
                }
            }
            catch (System.Exception ex)
            {
                Common.DisplayErrorMessage("AuthenticationRepository/RefreshToken: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return result;
        }
    }
}
