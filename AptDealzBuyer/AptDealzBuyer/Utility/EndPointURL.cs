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
        public const string RefreshToken = "api/Account/refresh-token";
        public const string Logout = "api/Account/logout";
        #endregion

        #region [ Profile API ]
        public const string GetMyProfileData = "api/v{0}/BuyerManagement/GetMyProfileData";
        public const string SaveProfile = "api/v{0}/BuyerManagement/Update";
        public const string DeactivateUser = "api/Account/DeactivateUser";
        public const string Country = "api/v{0}/Country/Get";
        public const string FileUpload = "api/FileUpload";
        public const string GetUserProfileByEmail = "api/Account/GetUserProfile/Email";
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
        public const string GetMyPreviousRequirements = "api/v{0}/Requirement/GetMyPreviousRequirements?PageNumber={1}&PageSize={2}";
        public const string DeleteRequirement = "api/v{0}/Requirement/Delete/{1}";
        public const string CancelRequirement = "api/v{0}/Requirement/CancelRequirement";
        public const string UpdateStatusRequirement = "api/v{0}/Requirement/UpdateStatus";
        #endregion
    }
}
