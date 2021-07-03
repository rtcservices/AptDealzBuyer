using Acr.UserDialogs;
using AptDealzBuyer.API;
using AptDealzBuyer.Views.MasterData;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace AptDealzBuyer.Utility
{
    public static class Common
    {
        #region Properties
        public static MasterDataPage MasterData { get; set; }
        public static string Token { get; set; }
        private static Regex PhoneNumber { get; set; } = new Regex(@"^[0-9]{10}$");
        private static Regex RegexPassword { get; set; } = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$");
        private static Regex RegexPincode { get; set; } = new Regex(@"^[0-9]{6}$");
        #endregion

        #region DisplayMessages
        public static void DisplayErrorMessage(string errormessage)
        {
            UserDialogs.Instance.Toast(new ToastConfig(errormessage)
            {
                Position = ToastPosition.Top,
                BackgroundColor = (Color)App.Current.Resources["ErrorBackground"],
                MessageTextColor = (Color)App.Current.Resources["White"],
                Duration = new TimeSpan(0, 0, 5),
            });
        }

        public static void DisplaySuccessMessage(string successmessage)
        {
            UserDialogs.Instance.Toast(new ToastConfig(successmessage)
            {
                Position = ToastPosition.Top,
                BackgroundColor = (Color)App.Current.Resources["SuccessBackground"],
                MessageTextColor = (Color)App.Current.Resources["White"],
                Duration = new TimeSpan(0, 0, 5)
            });
        }

        public static void DisplayWarningMessage(string warningmessage)
        {
            UserDialogs.Instance.Toast(new ToastConfig(warningmessage)
            {
                Position = ToastPosition.Top,
                BackgroundColor = (Color)App.Current.Resources["WarningBackground"],
                MessageTextColor = (Color)App.Current.Resources["White"],
                Duration = new TimeSpan(0, 0, 5),
            });
        }
        #endregion

        #region Methods
        public static async void BindAnimation(ImageButton imageButton = null, Button button = null, Grid grid = null, StackLayout stackLayout = null, Label label = null, Image image = null, Frame frame = null)
        {
            try
            {
                if (imageButton != null)
                {
                    await imageButton.ScaleTo(0.9, 100, Easing.Linear);
                    await imageButton.ScaleTo(1, 100, Easing.Linear);
                }

                if (button != null)
                {
                    await button.ScaleTo(0.9, 100, Easing.Linear);
                    await button.ScaleTo(1, 100, Easing.Linear);
                }

                if (grid != null)
                {
                    await grid.ScaleTo(0.9, 100, Easing.Linear);
                    await grid.ScaleTo(1, 100, Easing.Linear);
                }

                if (stackLayout != null)
                {
                    await stackLayout.ScaleTo(0.9, 100, Easing.Linear);
                    await stackLayout.ScaleTo(1, 100, Easing.Linear);
                }

                if (label != null)
                {
                    await label.ScaleTo(0.9, 100, Easing.Linear);
                    await label.ScaleTo(1, 100, Easing.Linear);
                }

                if (image != null)
                {
                    await image.ScaleTo(0.9, 100, Easing.Linear);
                    await image.ScaleTo(1, 100, Easing.Linear);
                }

                if (frame != null)
                {
                    await frame.ScaleTo(0.9, 100, Easing.Linear);
                    await frame.ScaleTo(1, 100, Easing.Linear);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Common/BindAnimation: " + ex.Message);
            }
        }

        public static bool IsValidPhone(this string value)
        {
            value = value.Trim();
            return PhoneNumber.IsMatch(value);
        }

        public static bool IsValidEmail(this string value)
        {
            try
            {
                value = value.Trim();
                var addr = new System.Net.Mail.MailAddress($"{value}");
                return addr.Address == $"{value}";
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPassword(this string value)
        {
            value = value.Trim();
            return (RegexPassword.IsMatch($"{value}"));
        }

        public static bool IsValidPincode(this string value)
        {
            value = value.Trim();
            return (RegexPincode.IsMatch($"{value}"));
        }

        public static bool EmptyFiels(string extEntry)
        {
            if (string.IsNullOrEmpty(extEntry) || string.IsNullOrWhiteSpace(extEntry))
            {
                return true;
            }
            else
            {
                extEntry = extEntry.Trim();
                return false;
            }
        }

        public async static Task<bool> InternetConnection()
        {
            bool result = await App.Current.MainPage.DisplayAlert(Constraints.NoInternetConnection, Constraints.PlsCheckInternetConncetion, Constraints.TryAgain, Constraints.Cancel);
            return result;
        }

        public static void OpenMenu()
        {
            try
            {
                if (Common.MasterData != null)
                {
                    Common.MasterData.IsGestureEnabled = true;
                    Common.MasterData.IsPresented = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Common/OpenMenu: " + ex.Message);
            }
        }
        #endregion
    }

    #region Enum   
    public enum FileUploadCategory
    {
        ProfilePicture = 0,
        ProfileDocuments = 1,
        RequirementImages = 2
    }

    public enum RequirementStatus
    {
        Active = 1,
        Completed = 2,
        Rejected = 3,
        Inactive = 4,
        Cancelled = 5
    }

    public enum RequirementSortBy
    {
        ID = 1,
        Date = 2,
        Quotes = 3,
        Amount = 4,
        Validity = 5,
        ASC = 6,
        DSC = 7
    }

    public enum PaymentStatus
    {
        Success = 1,
        Failed = 2
    }
    #endregion
}
