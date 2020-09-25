using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using Coop.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.Repository
{
    public interface IAccessPermissionRepository : IRepository<AccessPermissions>
    {
        IQueryable<ViewAccessPermissionModels> ReadDetail(int userTypeId);
        IQueryable<GetUserType> GetUserType();
    }
    public class AccessPermissionRepository : Repository<AccessPermissions>, IAccessPermissionRepository
    {
        public AccessPermissionRepository(CoopWebEntities context) : base(context) { }

        public IQueryable<ViewAccessPermissionModels> ReadDetail(int userTypeId)
        {
            if (userTypeId > 0)
            {
                var list = (from m in _context.Modules
                            join ps in _context.AccessPermissions.Where(p => p.UserTypeID == userTypeId) on m.ModuleID equals ps.ModuleID into pss
                            from p in pss.DefaultIfEmpty()
                            where m.IsActive
                            select new ViewAccessPermissionModels
                            {
                                UserTypeID = userTypeId,
                                //IsAccess = p.IsAccess,
                                ModifiedBy = p.ModifiedBy,
                                ModuleID = m.ModuleID,
                                ModuleName = m.ModuleName,
                                ModifiedDate = p.ModifiedDate,
                            });
                return list.AsQueryable();
            }
            return new List<ViewAccessPermissionModels>().AsQueryable();
        }
        private AccessPermissions GetIsAccess(int userTypeID, int moduleID)
        {
            return
                _context.AccessPermissions.FirstOrDefault(
                    p => p.ModuleID == moduleID && p.UserTypeID == userTypeID) ?? new AccessPermissions
                    {
                        IsAccess = false,
                    };
        }
        public IQueryable<GetUserType> GetUserType()
        {
            var q = (from p in _context.UserTypes
                     select new GetUserType
                     {
                         UserTypeID = p.UserTypeID,
                         UserTypeName = p.UserTypeName,
                     });
            return q;
        }
    }
}