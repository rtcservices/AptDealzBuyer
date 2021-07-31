using AptDealzBuyer.Interfaces;
using AptDealzBuyer.Model.Reponse;
using Firebase.Auth;
using Foundation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AptDealzBuyer.iOS.Service.FirebaseAuthenticator))]

namespace AptDealzBuyer.iOS.Service
{
    public class FirebaseAuthenticator : IFirebaseAuthenticator
    {
        const int OTP_TIMEOUT = 30; // seconds
        private TaskCompletionSource<bool> _phoneAuthTcs;
        TaskCompletionSource<Dictionary<bool, string>> keyValuePairs;
        public string _verificationId { get; set; }

        public FirebaseAuthenticator()
        {
        }

        public Task<string> LoginAsync(string username, string password)
        {
            var tcs = new TaskCompletionSource<string>();
            Auth.DefaultInstance.SignInWithPasswordAsync(username, password)
                .ContinueWith((task) => OnAuthCompleted(task, tcs));
            return tcs.Task;
        }

        public Task<Dictionary<bool, string>> SendOtpCodeAsync(string phoneNumber)
        {
            phoneNumber = (string)App.Current.Resources["CountryCode"] + phoneNumber;
            //_phoneAuthTcs = new TaskCompletionSource<bool>();

            PhoneAuthProvider.DefaultInstance.VerifyPhoneNumber(
                phoneNumber,
                null,
                new VerificationResultHandler(OnVerificationResult));
            var user = Auth.DefaultInstance.CurrentUser;

            keyValuePairs = new TaskCompletionSource<Dictionary<bool, string>>();

            return keyValuePairs.Task;
            //return _phoneAuthTcs.Task;
        }

        private void OnVerificationResult(string verificationId, NSError error)
        {
            if (error != null)
            {
                // something went wrong
                _phoneAuthTcs?.TrySetResult(false);
                return;
            }
            _verificationId = verificationId;
            _phoneAuthTcs?.TrySetResult(true);
        }

        public Task<string> VerifyOtpCodeAsync(string code)
        {
            var tcs = new TaskCompletionSource<string>();

            var credential = PhoneAuthProvider.DefaultInstance.GetCredential(
                _verificationId, code);
            Auth.DefaultInstance.SignInWithCredentialAsync(credential)
                .ContinueWith((task) => OnAuthCompleted(task, tcs));

            return tcs.Task;
        }

        private async void OnAuthCompleted(Task<AuthDataResult> task, TaskCompletionSource<string> tcs)
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                // something went wrong
                tcs.SetResult(string.Empty);
                return;
            }
            // user is logged in
            var result = task.Result;
            var token = await result.User.GetIdTokenAsync(false);
            tcs.SetResult(token);
        }

        public Task<AuthenticatedUser> GetUserAsync()
        {
            var tcs = new TaskCompletionSource<AuthenticatedUser>();

            Firebase.CloudFirestore.Firestore.SharedInstance
                .GetCollection("users")
                .GetDocument(Auth.DefaultInstance.CurrentUser.Uid)
                .GetDocument((snapshot, error) =>
                {
                    if (error != null)
                    {
                        // something went wrong
                        tcs.TrySetResult(default(AuthenticatedUser));
                        return;
                    }
                    tcs.TrySetResult(new AuthenticatedUser
                    {
                        Id = snapshot.Id,
                        FirstName = snapshot.GetValue(new NSString("FirstName")).ToString(),
                        LastName = snapshot.GetValue(new NSString("LastName")).ToString()
                    });
                });

            return tcs.Task;
        }

        public Task<bool> Signout()
        {
            var tcs = new TaskCompletionSource<bool>();
            try
            {
                Auth.DefaultInstance.SignOut(out NSError error);
                tcs.TrySetResult(true);
                return tcs.Task;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.InnerException.InnerException);
            }
            tcs.TrySetResult(false);
            return tcs.Task;
        }

    }
}