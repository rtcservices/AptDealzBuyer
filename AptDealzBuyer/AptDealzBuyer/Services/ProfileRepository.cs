using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
using AptDealzBuyer.Views.Login;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AptDealzBuyer.Services
{
    public class ProfileRepository : IProfileRepository
    {
        CategoryAPI categoryAPI = new CategoryAPI();
        public async Task<bool> ValidPincode(string pinCode, string pinCodeName = null)
        {
            bool isValid = false;
            string ValidationMsg = string.Empty;

            if (pinCodeName == "Billing")
            {
                ValidationMsg = Constraints.InValid_BillingPinCode;
            }
            else if (pinCodeName == "Shipping")
            {
                ValidationMsg = Constraints.InValid_ShillingPinCode;
            }
            else if (pinCodeName == "Delivery")
            {
                ValidationMsg = Constraints.InValid_DeliveryPinCode;
            }
            else
            {
                ValidationMsg = Constraints.InValid_Pincode;
            }

            try
            {
                ProfileAPI profileAPI = new ProfileAPI();
                if (Common.IsValidPincode(pinCode))
                {
                    var response = await profileAPI.GetPincodeInfo(pinCode);
                    if (response != null && response.Succeeded)
                    {
                        isValid = true;
                    }
                    else
                    {
                        Common.DisplayErrorMessage(ValidationMsg);
                    }
                }
                else
                {
                    Common.DisplayErrorMessage(ValidationMsg);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProfileRepository/ValidPincode: " + ex.Message);
            }
            return isValid;
        }

        public async Task<List<Category>> GetCategory()
        {
            List<Category> categories = new List<Category>();
            try
            {
                //UserDialogs.Instance.ShowLoading(Constraints.Loading);
                categories = await categoryAPI.GetCategory();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProfileRepository/GetCategory: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return categories;
        }

        public async Task<List<SubCategory>> GetSubCategory(string CategortyId)
        {
            List<SubCategory> subCategories = new List<SubCategory>();
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                subCategories = await categoryAPI.GetSubCategory(CategortyId);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProfileRepository/GetSubCategory: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return subCategories;
        }

        public async Task DeactivateAccount()
        {
            try
            {
                var result = await App.Current.MainPage.DisplayAlert(Constraints.Alert, Constraints.AreYouSureWantDeactivateAccount, Constraints.Yes, Constraints.No);
                if (result)
                {
                    ProfileAPI profileAPI = new ProfileAPI();
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    var mResponse = await profileAPI.DeactiviateUser(Settings.UserId);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        Settings.EmailAddress = string.Empty;
                        Settings.UserToken = string.Empty;
                        Settings.RefreshToken = string.Empty;
                        Settings.UserId = string.Empty;
                        Settings.LoginTrackingKey = string.Empty;
                        Settings.fcm_token = string.Empty;
                        MessagingCenter.Unsubscribe<string>(this, "NotificationCount");
                        App.stoppableTimer.Stop();

                        App.Current.MainPage = new NavigationPage(new LoginPage());
                    }
                    else
                    {
                        if (mResponse != null)
                            Common.DisplayErrorMessage(mResponse.Message);
                        else
                            Common.DisplayErrorMessage(Constraints.Something_Wrong);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProfileRepository/DeactivateAccount: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}
