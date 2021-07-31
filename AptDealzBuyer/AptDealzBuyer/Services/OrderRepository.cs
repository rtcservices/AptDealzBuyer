using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;

namespace AptDealzBuyer.Services
{
    public class OrderRepository : IOrderRepository
    {
        OrderAPI orderAPI = new OrderAPI();
        public async Task<Order> GetOrderDetails(string orderId)
        {
            Order mOrder = new Order();
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await orderAPI.GetOrderDetailsForBuyer(orderId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        mOrder = jObject.ToObject<Order>();
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
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderRepository/GetOrderDetails: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return mOrder;
        }

        public async Task CancelOrder(string orderId)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await orderAPI.CancelOrder(orderId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    SuccessfullSavedQuote(mResponse.Message);
                }
                else
                {
                    if (mResponse != null)
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderRepository/GetOrderDetails: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        public async Task<string> GenerateQRCodeImage(string orderId)
        {
            string imageBase64 = "";
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await orderAPI.GenerateQRCodeImageForBuyerApp(orderId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    imageBase64 = (string)mResponse.Data;
                }
                else
                {
                    if (mResponse != null)
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderRepository/GetOrderDetails: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return imageBase64;
        }

        void SuccessfullSavedQuote(string MessageString)
        {
            try
            {
                var successPopup = new Views.PopupPages.SuccessPopup(MessageString);
                PopupNavigation.Instance.PushAsync(successPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderRepository/SuccessSavedQuote: " + ex.Message);
            }
        }
    }
}
