using AptDealzBuyer.Interfaces;
using AptDealzBuyer.Model.Reponse;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Java.Util.Concurrent;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AptDealzBuyer.Droid.DependencService
{
    public class FirebaseAuthenticator : PhoneAuthProvider.OnVerificationStateChangedCallbacks, IFirebaseAuthenticator
    {
        const int OTP_TIMEOUT = 30; // seconds
        private TaskCompletionSource<bool> _phoneAuthTcs;
        private string _verificationId;

        public Task<string> LoginAsync(string username, string password)
        {
            var tcs = new TaskCompletionSource<string>();
            FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(username, password)
                .ContinueWith((task) => OnAuthCompleted(task, tcs));
            return tcs.Task;
        }

        public override void OnVerificationCompleted(PhoneAuthCredential credential)
        {
            // This callback will be invoked in two situations:
            // 1 - Instant verification. In some cases the phone number can be instantly
            //     verified without needing to send or enter a verification code.
            // 2 - Auto-retrieval. On some devices Google Play services can automatically
            //     detect the incoming verification SMS and perform verification without
            //     user action.
            if (credential.SmsCode == null && credential.SignInMethod?.ToLower() == "phone")
            {
                // 1 - Instant verification.  -> In case of instant verification and to ensure sms is always send, try to singout the instance and try again with another request
                FirebaseAuth.Instance.SignInWithCredential(credential);
                FirebaseAuth.Instance.SignOut();
                _phoneAuthTcs?.TrySetResult(false);
            }
            System.Diagnostics.Debug.WriteLine("PhoneAuthCredential created Automatically");
        }

        public override void OnVerificationFailed(FirebaseException exception)
        {
            System.Diagnostics.Debug.WriteLine("Verification Failed: " + exception.Message);
            _phoneAuthTcs?.TrySetResult(false);
        }

        public override void OnCodeSent(string verificationId, PhoneAuthProvider.ForceResendingToken forceResendingToken)
        {
            base.OnCodeSent(verificationId, forceResendingToken);
            _verificationId = verificationId;
            _phoneAuthTcs?.TrySetResult(true);
        }

        public Task<bool> SendOtpCodeAsync(string phoneNumber)
        {
            _phoneAuthTcs = new TaskCompletionSource<bool>();
            PhoneAuthProvider.Instance.VerifyPhoneNumber(
                phoneNumber,
                OTP_TIMEOUT,
                TimeUnit.Seconds,
                Platform.CurrentActivity,
                this);

            var user = FirebaseAuth.Instance.CurrentUser;
            return _phoneAuthTcs.Task;
        }

        private async void OnAuthCompleted(Task<Firebase.Auth.IAuthResult> task, TaskCompletionSource<string> tcs)
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                // something went wrong
                tcs.SetResult(string.Empty);
                return;
            }
            _verificationId = null;
            var result = task.Result;
            var token = await result.User.GetIdTokenAsync(false);
            tcs.SetResult(token.Token);
        }

        public Task<string> VerifyOtpCodeAsync(string code)
        {
            if (!string.IsNullOrWhiteSpace(_verificationId))
            {
                var credential = PhoneAuthProvider.GetCredential(_verificationId, code);
                var tcs = new TaskCompletionSource<string>();
                FirebaseAuth.Instance.SignInWithCredentialAsync(credential)
                    .ContinueWith((task) => OnAuthCompleted(task, tcs));
                return tcs.Task;
            }
            return System.Threading.Tasks.Task.FromResult(string.Empty);
        }

        public Task<AuthenticatedUser> GetUserAsync()
        {
            var tcs = new TaskCompletionSource<AuthenticatedUser>();

            FirebaseFirestore.Instance
                .Collection("users")
                .Document(FirebaseAuth.Instance.CurrentUser.Uid)
                .Get()
                .AddOnCompleteListener(new OnCompleteListener(tcs));

            return tcs.Task;
        }

        public Task<bool> Signout()
        {
            var tcs = new TaskCompletionSource<bool>();
            try
            {
                FirebaseAuth.Instance.SignOut();
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


    internal class OnCompleteListener : Java.Lang.Object, Android.Gms.Tasks.IOnCompleteListener
    {
        private TaskCompletionSource<AuthenticatedUser> _tcs;

        public OnCompleteListener(TaskCompletionSource<AuthenticatedUser> tcs)
        {
            _tcs = tcs;
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                // process document
                var result = task.Result;
                if (result is DocumentSnapshot doc)
                {
                    var user = new AuthenticatedUser();
                    user.Id = doc.Id;
                    user.FirstName = doc.GetString("FirstName");
                    user.LastName = doc.GetString("LastName");
                    _tcs.TrySetResult(user);
                    return;
                }
            }
            // something went wrong
            _tcs.TrySetResult(default(AuthenticatedUser));
        }

    }

}