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