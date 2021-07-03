using System.Threading.Tasks;

namespace AptDealzBuyer.Repository
{
    public interface IAuthenticationRepository
    {
        Task<bool> RefreshToken();
    }
}
