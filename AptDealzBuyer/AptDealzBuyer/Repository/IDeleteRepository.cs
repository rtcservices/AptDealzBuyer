using System.Threading.Tasks;

namespace AptDealzBuyer.Repository
{
    public interface IDeleteRepository
    {
        Task<bool> DeleteRequirement(string RequirmentID);
    }
}
