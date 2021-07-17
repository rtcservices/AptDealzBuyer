using AptDealzBuyer.API;
using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
using System;
using System.Threading.Tasks;

namespace AptDealzBuyer.Services
{
    public class ProfileRepository : IProfileRepository
    {
        public async Task<bool> ValidPincode(string pinCode)
        {
            bool isValid = false;
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
                        Common.DisplayErrorMessage(Constraints.InValid_Pincode);
                    }
                }
                else
                {
                    Common.DisplayErrorMessage(Constraints.InValid_Pincode);
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
