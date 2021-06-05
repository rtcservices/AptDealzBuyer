using AptDealzBuyer.API;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AptDealzBuyer.Extention
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchCountries : Grid
    {
        List<Country> countries;
        public SearchCountries()
        {
            InitializeComponent();
        }

        async void GetCountries()
        {
            try
            {
                ProfileAPI profileAPI = new ProfileAPI();
                var mResponse = await profileAPI.GetCountry((int)App.Current.Resources["PageNumber"], (int)App.Current.Resources["PageSize"]);
                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    countries = result.ToObject<List<Country>>();
                    pkNationality.ItemsSource = countries.Select(x => x.Name).ToList();
                }
                else
                {
                    Common.DisplayErrorMessage(mResponse.Message);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SearchCountries/GetCountries: " + ex.Message);
            }
        }

        private void SearchEntry_Unfocused(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(txtNationality.Text))
            {
                pkNationality.ItemsSource = countries.Where(x => x.Name.ToLower().Contains(txtNationality.Text.ToLower())).Select(x => x.Name).ToList();
            }
            else
            {
                pkNationality.ItemsSource = countries.Select(x => x.Name).ToList();
            }
        }
    }
}