using System.Threading.Tasks;

namespace AptDealzBuyer.Repository
{
    public interface IProfileRepository
    {
        Task<bool> ValidPincode(string pinCode);
    }
}
