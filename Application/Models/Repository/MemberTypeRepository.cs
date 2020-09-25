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
    public interface IMemberTypeRepository : IRepository<MemberType>
    {
        IQueryable<MemberTypeModel> ReadDetail();
        IQueryable<MemberTypeModel> ReadDetail(int mTypeID);
    }

    public class MemberTypeRepository : Repository<MemberType>, IMemberTypeRepository
    {
        public MemberTypeRepository(CoopWebEntities context) : base(context)
        {
        }
        public IQueryable<MemberTypeModel> ReadDetail()
        {
            var MemberType = from l in Read()
                select new MemberTypeModel
                {
                    MemberTypeID = l.MemberTypeID,
                    MemberTypeName = l.MemberTypeName,
                    //IsActive = l.IsActive,
                    ModifiedBy = l.ModifiedBy,
                    ModifiedDate = l.ModifiedDate
                    };
            return MemberType;
        }
        public IQueryable<MemberTypeModel> ReadDetail(int mTypeID)
        {
            var MemberType = ReadDetail().Where(l => l.MemberTypeID == mTypeID || l.IsActive);
            return MemberType;
        }
        public bool NotActive(int mTypeID)
        {
            var data = (from mType in Read()
                        where mType.MemberTypeID == mTypeID
                        select mType).FirstOrDefault();

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
