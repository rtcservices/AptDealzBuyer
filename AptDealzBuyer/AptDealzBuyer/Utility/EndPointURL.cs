namespace AptDealzBuyer.Utility
{
    public class EndPointURL
    {
        #region [ Register API ]
        public const string IsUniquePhoneNumber = "api/v{0}/BuyerAuth/IsUniquePhoneNumber";
        public const string IsUniqueEmail = "api/v{0}/BuyerAuth/IsUniqueEmail";
        public const string Register = "api/v{0}/BuyerAuth/Register";
        #endregion

        #region [ Authentication API ]
        public const string CheckPhoneNumberExists = "api/v{0}/BuyerAuth/CheckPhoneNumberExists?phoneNumber={1}";
        public const string BuyerAuthenticateEmail = "api/v{0}/BuyerAuth/authenticate/email";
        public const string BuyerAuthenticatePhone = "api/v{0}/BuyerAuth/authenticate/phone";
        public const string SendOtpByEmail = "api/v{0}/BuyerAuth/SendOtpByEmail";
        public const string RefreshToken = "api/Account/refresh-token";
        public const string SendOtp = "api/Account/SendOtp/Email/{0}";
        public const string Logout = "api/Account/logout";
        #endregion

        #region [ Profile API ]
        public const string DeactivateUser = "api/Account/DeactivateUser";
        public const string GetUserProfileByEmail = "api/v{0}/BuyerAuth/GetUserProfile/Email";
        public const string GetMyProfileData = "api/v{0}/BuyerManagement/GetMyProfileData";
        public const string SaveProfile = "api/v{0}/BuyerManagement/Update";
        public const string Country = "api/v{0}/Country/Get";
        public const string FileUpload = "api/FileUpload";
        public const string GetPincodeInfo = "api/IndianPincode/GetPincodeInfo/{0}";
        #endregion

        #region [ Category API ]
        public const string Category = "api/v{0}/Category/Get";
        public const string SubCategory = "api/v{0}/SubCategory/Get?CategoryId={1}";
        #endregion

        #region [ Requirement API ]
        public const string CreateRequirement = "api/v{0}/Requirement/Create";
        public const string GetRequirement = "api/v{0}/Requirement/Get?PageNumber={1}&PageSize={2}";
        public const string GetRequirementById = "api/v{0}/Requirement/Get/{1}";
        public const string GetAllMyActiveRequirements = "api/v{0}/Requirement/GetAllMyActiveRequirements";
        public const string GetMyPreviousRequirements = "api/v{0}/Requirement/GetMyPreviousRequirements";
        public const string DeleteRequirement = "api/v{0}/Requirement/Delete/{1}";
        public const string CancelRequirement = "api/v{0}/Requirement/CancelRequirement?RequirementId={1}";
        public const string UpdateStatusRequirement = "api/v{0}/Requirement/UpdateStatus";
        #endregion

        #region [ Quote API ]
        public const string GetQuoteById = "api/v{0}/Quote/Get/{1}";
        public const string GetQuotes = "api/v{0}/Quote/Get?RequirementId={1}";
        public const string AcceptQuote = "api/v{0}/Quote/AcceptQuote?quoteId={1}";
        public const string RejectQuote = "api/v{0}/Quote/RejectQuote?quoteId={1}";
        public const string RevealSellerContact = "api/v{0}/Quote/RevealSellerContact";
        #endregion

        #region [ Order API ]
        public const string GetOrderDetailsForBuyer = "api/v{0}/Order/GetOrderDetailsForBuyer/{1}";
        public const string GetOrdersForBuyer = "api/v{0}/Order/GetOrdersForBuyer";
        public const string CancelOrder = "api/v{0}/Order/CancelOrder/{1}";
        public const string CreateOrder = "api/v{0}/Order/Create";
        #endregion
    }
}


