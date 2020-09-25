using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coop.Entities;
using Coop.Models.POCO;
using Coop.Infrastructure.Helpers;

namespace Coop.Models.Repository
{
    public interface IModuleRepository : IRepository<Modules>
    {
        IQueryable<ModuleModel> GetMenu(int userTypeID);
        IQueryable<ModuleModel> GetMenuCategory(int userTypeID);
    }
    public class ModuleRepository : Repository<Modules>, IModuleRepository
    {
        public ModuleRepository(CoopWebEntities context) : base(context) { }

        public IQueryable<ModuleModel> GetMenu(int UserTypeID)
        {
            var modules = (from m in Read()
                          join a in _context.AccessPermissions on m.ModuleID equals a.ModuleID
                          join mc in _context.ModuleCategories on m.ModuleCategoryID equals mc.ModuleCategoryID
                          where m.IsActive && a.IsAccess && a.UserTypeID == UserTypeID
                          orderby mc.SortOrder, m.SortOrder
                          select new ModuleModel
                          {
                              ModuleID = m.ModuleID,
                              ModuleName = m.ModuleName,
                              ModuleDescription = m.ModuleDescription,
                              ModuleURL = m.ModuleURL,
                              ModuleCategoryID = mc.ModuleCategoryID,
                              ModuleCategoryName = mc.ModuleCategoryName,
                              SortOrder = mc.SortOrder,
                              IconUrl = m.IconUrl
                          });
            return modules;
        }

        public IQueryable<ModuleModel> GetMenuCategory(int userTypeID)
        {
            var moduleCategory = (from m in Read()
                                  join a in _context.AccessPermissions on m.ModuleID equals a.ModuleID
                                  join mc in _context.ModuleCategories on m.ModuleCategoryID equals mc.ModuleCategoryID
                                  orderby mc.SortOrder, m.SortOrder
                                  where m.IsActive && a.IsAccess && a.UserTypeID == userTypeID
                                  group mc by new { mc.ModuleCategoryID, mc.ModuleCategoryName, mc.IconUrl, mc.SortOrder }
                                  into g
                                  select new ModuleModel
                                  {
                                      ModuleCategoryID = g.Key.ModuleCategoryID,
                                      ModuleCategoryName = g.Key.ModuleCategoryName,
                                      IconUrl = g.Key.IconUrl,
                                      CountSubMenu = g.Count(),
                                      SortOrder = g.Key.SortOrder
                                  });
            return moduleCategory.OrderBy(o => o.SortOrder);
        }
    }
}