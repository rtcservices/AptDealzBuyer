namespace AptDealzBuyer.Utility
{
    public static class Constraints
    {

        public const int LargeBufferSize = 4096;

        #region [ Images ]
        public const string Password_Hide = "iconHidePass.png";
        public const string Password_Visible = "iconVisiblepass.png";

        public const string Redio_UnSelected = "iconRadioUnselect.png";
        public const string Radio_Selected = "iconRadioSelect.png";

        public const string Switch_Off = "iconSwitchOff.png";
        public const string Switch_On = "iconSwitchOn.png";

        public const string Arrow_Right = "iconRightArrow.png";
        public const string Arrow_Left = "iconLeftArrow.png";
        public const string Arrow_Down = "iconDownArrow.png";
        public const string Arrow_Up = "iconUpArrow.png";

        public const string Sort_ASC = "iconSortASC.png";
        public const string Sort_DSC = "iconSortDSC.png";

        public const string CheckBox_UnChecked = "iconUncheck.png";
        public const string CheckBox_Checked = "iconCheck.png";

        public const string GreenArrow_Down = "iconGreenDownArrow.png";
        public const string GreenArrow_Up = "iconGreenUpArrow.png";
        #endregion

        #region [ Error Message ]
        public static string Session_Expired = "Session expired, Please login again!";
        public static string Something_Wrong = "Something went wrong!";
        public static string Something_Wrong_Server = "Something went wrong from server!";
        public static string ServiceUnavailable = "Service Unavailable, Try again later!";
        public static string Number_was_null = "Number was null or white space";
        public static string Phone_Dialer_Not_Support = "Phone Dialer is not supported on this device";
        #endregion

        #region [ Alert Title ]
        public const string Yes = "Yes";
        public const string No = "No";
        public const string Ok = "OK";
        public const string TryAgain = "Try Again";
        public const string Cancel = "Cancel";
        public const string UploadPicture = "Upload Picture";
        public const string TakePhoto = "Take Photo";
        public const string ChooseFromLibrary = "Choose From Library";
        public const string NoCamera = "No Camera";
        public const string Alert = "Alert";
        public const string Loading = "Loading...";
        public const string Logout = "Logout";
        #endregion

        #region [ Alert Message ]
        public const string CouldNotSentOTP = "Could not send Verification Code to the given number!";
        public const string DoYouWantToExit = "Do you really want to exit?";

        public const string AreYouSureWantLogout = "Are you sure want to logout?";
        public const string AreYouSureWantDelete = "Are you sure want to delete?";
        public const string AreYouSureWantCancel = "Are you sure want to cancel?";
        public const string AreYouSureWantDeleteNotification = "Are you sure you want to delete this notification?";
        public const string AreYouSureWantDeactivateAccount = "Are you sure want to deactivate account?";
        public const string AreYouSureWantCancelReq = "Are you sure you want to cancel this requirement?";
        public const string AreYouSureWantDeleteReq = "Are you sure you want to delete this requirement?";
        public const string AreYouSureWantCancelOrder = "Are you sure you want to cancel this order?";

        public const string NoInternetConnection = "No internet connection!";
        public const string NoCameraAwailable = "No camera awailable!";
        public const string UnableTakePhoto = "Unable to take photo!";
        public const string PermissionDenied = "Permission denied!";
        public const string PhotosNotSupported = "The Photo is not supported!";
        public const string PermissionNotGrantedPhotos = "Permission is not granted to photos!";
        public const string NeedStoragePermissionAccessYourPhotos = "Need storage permission to access your photos!";
        public const string PlsCheckInternetConncetion = "Please check internet connection to use App.";

        public const string Agree_T_C = "Please check box for Terms & Conditions.";
        public const string ContactRevealed = "Seller Contacts Revealed!";

        public const string InValid_Email = "Please enter valid email address!";
        public const string InValid_PhoneNumber = "Please enter valid phone number!";
        public const string InValid_OTP = "Verification code is invalid! Try again!";
        public const string InValid_Nationality = "Please enter valid nationality!";

        public const string InValid_Pincode = "Please enter valid pincode!";
        public const string InValid_DeliveryPinCode = "Please enter valid delivery location pin code!";
        public const string InValid_BillingPinCode = "Please enter valid billing address pin code!";
        public const string InValid_ShillingPinCode = "Please enter valid shipping address pin code!";

        public const string Required_All = "Selected fields are required!";
        public const string Required_Email_Phone = "Please enter email address or phone number!";
        public const string Required_Email = "Please enter email address!";
        public const string Required_VerificationCode = "Please enter verification code!";
        public const string Required_FullName = "Please enter full name!";
        public const string Required_PhoneNumber = "Please enter phone number!";
        public const string Required_BuildingNumber = "Please enter building number!";
        public const string Required_Street = "Please enter street!";
        public const string Required_City = "Please enter city!";
        public const string Required_Nationality = "Please enter nationality!";
        public const string Required_PinCode = "Please enter pincode!";
        public const string Required_Landmark = "Please enter landmark!";

        public const string Required_Title = "Please enter title!";
        public const string Required_Category = "Please select category!";
        public const string Required_SubCategory = "Please select sub category!";
        public const string Required_QuantityUnits = "Please select quantity unit!";
        public const string Required_Description = "Please enter description!";
        public const string Required_Quantity = "Please enter quantity!";
        public const string Required_PriceEstimation = "Please enter Total Price Estimation!";
        public const string Required_Delivery_PinCode = "Please enter delivery location pin code!";

        public const string Required_Billing_Name = "Please enter billing address name!";
        public const string Required_Billing_Building = "Please enter billing address building!";
        public const string Required_Billing_Street = "Please enter billing address street!";
        public const string Required_Billing_City = "Please enter billing address city!";
        public const string Required_Billing_PinCode = "Please enter billing address pin code!";

        public const string Required_Shipping_Name = "Please enter shipping address name!";
        public const string Required_Shipping_Building = "Please enter shipping address building!";
        public const string Required_Shipping_Street = "Please enter shipping address street!";
        public const string Required_Shipping_City = "Please enter shipping address city!";
        public const string Required_Shipping_PinCode = "Please enter shipping address pin code!";
        public const string Required_Shipping_Landmark = "Please enter shipping address landmark!";

        public const string Required_ComplainType = "Please select complaint type!";
        public const string Required_Response = "Please enter your message!";
        #endregion

        #region [ Success ]
        public const string OTPSent = "OTP Verification Code Sent Successfully";
        public const string InstantVerification = "you don't get any code, it is instant verification. Please try to login with email address";
        #endregion

        #region [ Copy Message ]
        public const string CopiedBuyerId = "Copied Buyer Id!";
        public const string CopiedSellerId = "Copied Seller Id!";
        public const string CopiedRequirementId = "Copied Requirement ID!";
        public const string CopiedOrderId = "Copied Order Id!";
        public const string CopiedQuoteRefNo = "Copied Quote Reference No!";
        public const string CopiedGrievanceId = "Copied Grievance Id!";
        #endregion
    }
}
