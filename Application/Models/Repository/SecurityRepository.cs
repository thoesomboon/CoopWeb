using System;
using System.Linq;
using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
//using Coop.Library;
using System.Web;
using System.Collections.Generic;
using Coop.Models.Repository;

namespace Coop.Models.Repository
{
    public interface ISecurityRepository : IRepository<Security>
    {
        IQueryable<SecurityModel> ReadDetail();
        IQueryable<SecurityModel> ReadDetail(string LonID, int Seq);
        bool Update(SecurityModel model);
        bool NotActive(String LonID, int Seq);
    }
    public class SecurityRepository : Repository<Security>, ISecurityRepository
    {
        public SecurityRepository(CoopWebEntities context) : base(context)
        {
        }
        public IQueryable<SecurityModel> ReadDetail()
        {
            var sec = from s in Read()
                          select new SecurityModel
                          {
                              CreatedBy = s.CreatedBy,
                              CreatedDate = s.CreatedDate,
                              ModifiedBy = s.ModifiedBy,
                              ModifiedDate = s.ModifiedDate,
                              Filestatus = s.Filestatus,
                              CoopID = s.CoopID,
                              LoanID = s.LoanID,
                              Seq = s.Seq,
                              SecurityTypeID = s.SecurityTypeID,
                              SecurityTypeName = s.SecurityTypeName,
                              LicenceNo = s.LicenceNo,
                              OwnerName = s.OwnerName,
                              Area = s.Area,
                              Value = s.Value
                          };
            return sec;
        }
        public IQueryable<SecurityModel> ReadDetail(string LonID, int Seq)
        {
            var sec = ReadDetail().Where(s => s.LoanID == LonID && s.Seq == Seq);
            return sec;
        }
        public bool Update(SecurityModel model)
        {
            var data = (from sec in Read()
                        where sec.LoanID == model.LoanID && sec.Seq == model.Seq
                        select sec).FirstOrDefault();

            if (data == null) { return false; }

            data.Filestatus = model.Filestatus;
            data.CoopID = model.CoopID;
            data.LoanID = model.LoanID;
            data.Seq = model.Seq;
            data.SecurityTypeID = model.SecurityTypeID;
            data.SecurityTypeName = model.SecurityTypeName;
            data.LicenceNo = model.LicenceNo;
            data.OwnerName = model.OwnerName;
            data.Area = model.Area;
            data.Value = model.Value;

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
        public bool NotActive(String LonID, int Seq)
        {
            var data = (from sec in Read()
                        where sec.LoanID == LonID && sec.Seq == Seq
                        select sec).FirstOrDefault();

            if (data == null) { return false; }

            if (data == null) { return false; }

            data.Filestatus = "D";
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
