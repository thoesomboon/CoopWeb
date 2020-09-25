using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coop.Models.Repository;

namespace Coop.Models.Repository
{
    public interface IMemberGroupRepository : IRepository<MemberGroup>
    {
        IQueryable<MemberGroupModel> ReadDetail();
        IQueryable<MemberGroupModel> ReadDetail(int mGroupID);
    }

    public class MemberGroupRepository : Repository<MemberGroup>, IMemberGroupRepository
    {
        public MemberGroupRepository(CoopWebEntities context) : base(context)
        {
        }
        public IQueryable<MemberGroupModel> ReadDetail()
        {
            var MemberGroup = from l in Read()
                select new MemberGroupModel
                {
                    MemberGroupID = l.MemberGroupID,
                    MemberGroupName = l.MemberGroupName,
                    //IsActive = l.IsActive,
                    ModifiedBy = l.ModifiedBy,
                    ModifiedDate = l.ModifiedDate
                    };
            return MemberGroup;
        }
        public IQueryable<MemberGroupModel> ReadDetail(int mGroupID)
        {
            var MemberGroup = ReadDetail().Where(l => l.MemberGroupID == mGroupID || l.IsActive);
            return MemberGroup;
        }
        public bool NotActive(int mGroupID)
        {
            var data = (from mGroup in Read()
                        where mGroup.MemberGroupID == mGroupID
                        select mGroup).FirstOrDefault();

            if (data == null) { return false; }

            data.IsActive = false;
            data.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
            data.ModifiedDate = System.DateTime.Now;

            int returnVal = 0;
            bool result = false;

            try
            {
                returnVal = _context.SaveChanges();
                result = returnVal > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
