namespace AptDealzBuyer.Utility
{
    public class EndPointURL
    {
        #region [ Register API ]
        public const string Register = "api/v{0}/BuyerAuth/Register";
        public const string IsUniquePhoneNumber = "api/Account/IsUniquePhoneNumber";
        public const string IsUniqueEmail = "api/Account/IsUniqueEmail";
        public const string SendOtp = "api/Account/SendOtp/Email/{0}";
        #endregion

        #region [ Authentication API ]

        public const string BuyerAuthenticateEmail = "api/v{0}/BuyerAuth/authenticate/email";
        public const string BuyerAuthenticatePhone = "api/v{0}/BuyerAuth/authenticate/phone";
        public const string SendOtpByEmail = "api/Account/SendOtpByEmail";
        #endregion

        #region [ ProfileAPI ]
        public const string GetMyProfileData = "api/v{0}/BuyerManagement/GetMyProfileData";
        public const string SaveProfile = "api/v{0}/BuyerManagement/Update/{1}";
        public const string DeactivateUser = "api/Account/DeactivateUser";
        public const string Country = "api/v{0}/Country/Get?PageNumber={1}&PageSize={2}";
        public const string FileUpload = "api/FileUpload";
        public const string GetUserProfileByEmail = "api/Account/GetUserProfile/Email";
        #endregion
    }
}
