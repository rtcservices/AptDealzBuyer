using Acr.UserDialogs;
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
            await Task.Delay(5 * 1000);
            App.Current.MainPage = new NavigationPage(new Views.SplashScreen.WelcomePage());
        }
        #endregion
    }
}