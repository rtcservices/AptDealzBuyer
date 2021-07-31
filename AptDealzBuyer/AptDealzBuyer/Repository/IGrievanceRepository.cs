using AptDealzBuyer.Model.Request;
using System.Threading.Tasks;

namespace AptDealzBuyer.Repository
{
    interface IGrievanceRepository
    {
        Task<Grievance> GetGrievancesDetails(string GrievanceId);
    }

}
