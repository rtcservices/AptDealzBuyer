using AptDealzBuyer.API;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.MasterData;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Views.SplashScreen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Spalshscreen : ContentPage
    {
        #region Constructor
        public Spalshscreen()
        {
            InitializeComponent();
        }
        #endregion

        #region Method
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindNavigation();
        }

        async void BindNavigation()
        {
            try
            {
                await Task.Delay(5 * 1000);

                if (Common.EmptyFiels(Settings.UserToken))
                {
                    App.Current.MainPage = new NavigationPage(new WelcomePage());
                }
                else
                {
                    Common.Token = Settings.UserToken;

                    AuthenticationAPI authenticationAPI = new AuthenticationAPI();
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

                        App.Current.MainPage = new NavigationPage(new WelcomePage());
                    }
                }
            }
            catch (System.Exception ex)
            {
                Common.DisplayErrorMessage("Spalshscreen/BindNavigation: " + ex.Message);
            }
        }
        #endregion
    }
}