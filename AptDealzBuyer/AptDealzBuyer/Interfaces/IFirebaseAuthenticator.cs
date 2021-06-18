using AptDealzBuyer.Model.Reponse;
using System.Threading.Tasks;


namespace AptDealzBuyer.Interfaces
{
    public interface IFirebaseAuthenticator
    {
        Task<string> LoginAsync(string username, string password);

        Task<bool> SendOtpCodeAsync(string phoneNumber);

        Task<string> VerifyOtpCodeAsync(string code);

        Task<AuthenticatedUser> GetUserAsync();

        Task<bool> Signout();

        string _verificationId { get; set; }
    }
}
