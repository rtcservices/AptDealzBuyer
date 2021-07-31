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
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(5 * 1000);
            BindNavigation();
        }

        private void BindNavigation()
        {
            try
            {
                if (Common.EmptyFiels(Settings.UserToken))
                {
                    if (Settings.IsViewWelcomeScreen)
                    {
                        App.Current.MainPage = new NavigationPage(new Views.SplashScreen.WelcomePage());
                    }
                    else
                    {
                        App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
                    }
                }
                else
                {
                    Common.Token = Settings.UserToken;
                    App.Current.MainPage = new MasterDataPage();
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