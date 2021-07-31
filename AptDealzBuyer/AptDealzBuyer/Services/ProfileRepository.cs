using AptDealzBuyer.API;
using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
using System;
using System.Threading.Tasks;

namespace AptDealzBuyer.Services
{
    public class ProfileRepository : IProfileRepository
    {
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
    }
}
