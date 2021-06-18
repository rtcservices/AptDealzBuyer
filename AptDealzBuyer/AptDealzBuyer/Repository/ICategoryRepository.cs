using AptDealzBuyer.Model.Reponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AptDealzBuyer.Repository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategory();
        Task<List<SubCategory>> GetSubCategory(string CategortyId);
    }
}
