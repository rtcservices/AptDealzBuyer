using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System.ComponentModel;

namespace AptDealzBuyer.Utility
{
    public class UserInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string emailAddress;
        public string EmailAddress
        {
            get { return EmailAddress; }
            set
            {
                emailAddress = value;
                PropertyChanged(this, new PropertyChangedEventArgs("EmailAddress"));
                Settings.EmailAddress = value;
            }
        }

        public string _fcm_token;
        public string fcm_token
        {
            get { return fcm_token; }
            set
            {
                _fcm_token = value;
                PropertyChanged(this, new PropertyChangedEventArgs("fcm_token"));
                Settings.fcm_token = value;
            }
        }

        public string _firebaseVerificationId;
        public string firebaseVerificationId
        {
            get { return firebaseVerificationId; }
            set
            {
                _firebaseVerificationId = value;
                PropertyChanged(this, new PropertyChangedEventArgs("firebaseVerificationId"));
                Settings.firebaseVerificationId = value;
            }
        }

        public string _userToken;
        public string UserToken
        {
            get { return UserToken; }
            set
            {
                _userToken = value;
                PropertyChanged(this, new PropertyChangedEventArgs("UserToken"));
                Settings.UserToken = value;
            }
        }

        public string userId;
        public string UserId
        {
            get { return UserId; }
            set
            {
                userId = value;
                PropertyChanged(this, new PropertyChangedEventArgs("UserId"));
                Settings.UserId = value;
            }
        }
    }

    public class Settings
    {
        private static ISettings AppSettings
        {
            get { return CrossSettings.Current; }
        }

        private const string EmailAddressKey = "email_address_key";
        private const string fcm_tokenKey = "fcm_token_key";
        private const string firebaseVerificationIdKey = "firebaseVerificationId_key";
        private const string UserTokenKey = "userToken_key";
        private const string UserIdKey = "userId_key";
        private static readonly string SettingsDefault = string.Empty;

        public static string EmailAddress
        {
            get { return AppSettings.GetValueOrDefault(EmailAddressKey, SettingsDefault); }
            set { AppSettings.AddOrUpdateValue(EmailAddressKey, value); }
        }

        public static string fcm_token
        {
            get { return AppSettings.GetValueOrDefault(fcm_tokenKey, SettingsDefault); }
            set { AppSettings.AddOrUpdateValue(fcm_tokenKey, value); }
        }

        public static string firebaseVerificationId
        {
            get { return AppSettings.GetValueOrDefault(firebaseVerificationIdKey, SettingsDefault); }
            set { AppSettings.AddOrUpdateValue(firebaseVerificationIdKey, value); }
        }

        public static string UserToken
        {
            get { return AppSettings.GetValueOrDefault(UserTokenKey, SettingsDefault); }
            set { AppSettings.AddOrUpdateValue(UserTokenKey, value); }
        }

        public static string UserId
        {
            get { return AppSettings.GetValueOrDefault(UserIdKey, SettingsDefault); }
            set { AppSettings.AddOrUpdateValue(UserIdKey, value); }
        }
    }
}
