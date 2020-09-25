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
    public interface IRequestLoanDueRepository : IRepository<RequestLoanDue>
    {
        IQueryable<RequestLoanDueModel> ReadDetail();
        IQueryable<RequestLoanDueModel> ReadDetail(string LonID, System.DateTime DDate);
        bool Update(RequestLoanDueModel model);
        bool NotActive(String LonID, System.DateTime DDate);

    }
    public class RequestLoanDueRepository : Repository<RequestLoanDue>, IRequestLoanDueRepository
    {
        public RequestLoanDueRepository(CoopWebEntities context) : base(context)
        {
        }
        public IQueryable<RequestLoanDueModel> ReadDetail()
        {
            var ReqDue = from l in Read()
                select new RequestLoanDueModel
                {
                    CreatedBy = l.CreatedBy,
                    CreatedDate = l.CreatedDate,
                    ModifiedBy = l.ModifiedBy,
                    ModifiedDate = l.ModifiedDate,
                    Filestatus = l.Filestatus,
                    CoopID = l.CoopID,
                    RequestNo = l.RequestNo,
                    DueDate = l.DueDate,
                    LoanDueAmt = l.LoanDueAmt
                };
            return ReqDue;
        }
        public IQueryable<RequestLoanDueModel> ReadDetail(string ReqNo, System.DateTime DDate)
        {
            var ReqDue = ReadDetail().Where(l => l.RequestNo == ReqNo && l.DueDate == DDate);
            return ReqDue;
        }
        public bool Update(RequestLoanDueModel model)
        {
            var data = (from rDue in Read()
                        where rDue.RequestNo == model.RequestNo && rDue.DueDate == model.DueDate
                        select rDue).FirstOrDefault();

            if (data == null) { return false; }

            data.Filestatus = model.Filestatus;
            data.CoopID = model.CoopID;
            data.RequestNo = model.RequestNo;
            data.DueDate = model.DueDate;
            data.LoanDueAmt = model.LoanDueAmt;

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
        public bool NotActive(String LonID, System.DateTime DDate)
        {
            var data = (from rDue in Read()
                        where rDue.RequestNo == LonID && rDue.DueDate == DDate
                        select rDue).FirstOrDefault();

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
        //public List<RequestLoanDue> RequestLoanDueList(String LonID) 
        //{
        //    CoopWebEntities db = new CoopWebEntities();
        //    List<RequestLoanDue> ReqDueList = db.RequestLoanDue.Where(l => l.ReqNo == LonID).ToList();
        //    return ReqDueList;
        //}
    }
}
