using AptDealzBuyer.API;
using AptDealzBuyer.Model.Reponse;
using AptDealzBuyer.Repository;
using AptDealzBuyer.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AptDealzBuyer.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        CategoryAPI categoryAPI = new CategoryAPI();
        public async Task<List<Category>> GetCategory()
        {
            List<Category> categories = new List<Category>();
            try
            {
                categories = await categoryAPI.GetCategory();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CategoryRepository/GetCategory: " + ex.Message);
            }
            return categories;
        }

        public async Task<List<SubCategory>> GetSubCategory(string CategortyId)
        {
            List<SubCategory> subCategories = new List<SubCategory>();
            try
            {
                subCategories = await categoryAPI.GetSubCategory(CategortyId);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("CategoryRepository/GetSubCategory: " + ex.Message);
            }

            return subCategories;
        }
    }
}
