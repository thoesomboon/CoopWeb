using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.Repository
{
    public interface IModuleCategoriesRepository : IRepository<ModuleCategories>
    {
        IQueryable<ModuleCategoriesModel> ReadDetail();
    }
    public class ModuleCategoriesRepository : Repository<ModuleCategories>, IModuleCategoriesRepository
    {
        public ModuleCategoriesRepository(CoopWebEntities context) : base(context) { }
        public IQueryable<ModuleCategoriesModel> ReadDetail()
        {
            var modulecategories = from m in Read()
                select new ModuleCategoriesModel
                {
                    ModuleCategoryID = m.ModuleCategoryID,
                    ModuleCategoryName = m.ModuleCategoryName
                    //ModuleCategoryDescription = m.ModuleCategoryDescription,
                    //IconUrl = m.IconUrl,
                    //IsActive = m.IsActive,
                    //SortOrder = m.SortOrder,
                    //CreatedBy = m.CreatedBy,
                    //CreatedDate = m.CreatedDate,
                    //ModifiedBy = m.ModifiedBy,
                    //ModifiedDate = m.ModifiedDate
                };
            return modulecategories;
        }
    }
}
