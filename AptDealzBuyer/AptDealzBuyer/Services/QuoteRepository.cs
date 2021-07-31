using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model.Request;
using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AptDealzBuyer.Services
{
    public class QuoteRepository : IQuoteRepository
    {
        QuoteAPI quoteAPI = new QuoteAPI();

        public async Task<string> RevealContact(RevealSellerContact mRevealSellerContact)
        {
            string PhoneNumber = "Reveal Contact";
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await quoteAPI.RevealSellerContact(mRevealSellerContact);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        var mSellerContact = jObject.ToObject<RevealSellerContact>();
                        if (mSellerContact != null)
                        {
                            var successPopup = new Views.PopupPages.SuccessPopup(Constraints.ContactRevealed);
                            await PopupNavigation.Instance.PushAsync(successPopup);

                            PhoneNumber = mSellerContact.PhoneNumber;
                        }
                    }
                }
                else
                {
                    if (mResponse != null)
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }

            }
            catch (ArgumentNullException)
            {
                Common.DisplayErrorMessage(Constraints.Number_was_null);
            }
            catch (FeatureNotSupportedException)
            {
                Common.DisplayErrorMessage(Constraints.Phone_Dialer_Not_Support);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteRepository/RevealContact: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return PhoneNumber;
        }
    }
}
