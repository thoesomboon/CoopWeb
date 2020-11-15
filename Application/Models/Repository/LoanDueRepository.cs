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
    public interface ILoanDueRepository : IRepository<LoanDue>
    {
        IQueryable<LoanDueModel> ReadDetail();
        IQueryable<LoanDueModel> ReadDetail(string LonID);
        IQueryable<LoanDueModel> ReadDetail(string LonID, int Sq);
        IQueryable<LoanDueModel> ReadDetail(string LonID, System.DateTime DDate);
        bool Update(LoanDueModel model);
        bool NotActive(String LonID, System.DateTime DDate);
        LoanDueModel ReadLoanDueInfo(string LonId);
    }
    public class LoanDueRepository : Repository<LoanDue>, ILoanDueRepository
    {
        public LoanDueRepository(CoopWebEntities context) : base(context)
        {
        }
        public IQueryable<LoanDueModel> ReadDetail()
        {
            var loanDue = from l in Read()
                select new LoanDueModel
                {
                    CreatedBy = l.CreatedBy,
                    CreatedDate = l.CreatedDate,
                    ModifiedBy = l.ModifiedBy,
                    ModifiedDate = l.ModifiedDate,
                    Filestatus = l.Filestatus,
                    CoopID = l.CoopID,
                    LoanID = l.LoanID,
                    DueDate = l.DueDate,
                    LoanDueAmt = l.LoanDueAmt,
                    BFLoanDueAmt = l.BFLoanDueAmt
                    //BFLoanDueAmtBeforeUPD = l.BFLoanDueAmtBeforeUPD,
                    //BFLoanDueAmtAfterUPD = l.BFLoanDueAmtAfterUPD
                };
            return loanDue;
        }
        public IQueryable<LoanDueModel> ReadDetail(string LonID)
        {
            var loanDue = ReadDetail().Where(l => l.LoanID == LonID);
            return loanDue;
        }
        public IQueryable<LoanDueModel> ReadDetail(string LonID, int Sq)
        {
            var loanDue = ReadDetail().Where(l => l.LoanID == LonID && l.Seq == Sq);
            return loanDue;
        }
        public IQueryable<LoanDueModel> ReadDetail(string LonID, System.DateTime DDate)
        {
            var loanDue = ReadDetail().Where(l => l.LoanID == LonID && l.DueDate == DDate);
            return loanDue;
        }              
        public bool Update(LoanDueModel model)
        {
            var data = (from lDue in Read()
                        where lDue.LoanID == model.LoanID && lDue.DueDate == model.DueDate
                        select lDue).FirstOrDefault();

            if (data == null) { return false; }

            data.Filestatus = model.Filestatus;
            data.CoopID = model.CoopID;
            data.LoanID = model.LoanID;
            data.DueDate = model.DueDate;
            data.LoanDueAmt = model.LoanDueAmt;
            data.BFLoanDueAmt = model.BFLoanDueAmt;
            //data.BFLoanDueAmtBeforeUPD = model.BFLoanDueAmtBeforeUPD;
            //data.BFLoanDueAmtAfterUPD = model.BFLoanDueAmtAfterUPD;

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
            var data = (from lDue in Read()
                        where lDue.LoanID == LonID && lDue.DueDate == DDate
                        select lDue).FirstOrDefault();

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
        public LoanDueModel ReadLoanDueInfo(string LonId)
        {
            var d = from ld in Read()
                    join l in _context.Loan on ld.LoanID equals l.LoanID
                    join m in _context.Member on l.MemberID equals m.MemberID
                    join lt in _context.LoanType on l.LoanTypeID equals lt.LoanTypeID
                    where ld.LoanID == LonId && ld.Filestatus == "A"

            //var q = from l in Read()
            //        join m in _context.Member on l.MemberID equals m.MemberID
            //        join lt in _context.LoanType on l.LoanTypeID equals lt.LoanTypeID
            //        join i in _context.InstallMethod on l.InstallMethodID equals i.InstallMethodID
            //        join r in _context.Reason on l.ReasonID equals r.ReasonID
            //        where l.LoanID == LonId //&& m.CoopID == coopId
            //        select new LoanModel

                    select new LoanDueModel
                    {
                        Filestatus = l.Filestatus,
                        CoopID = ld.CoopID,
                        LoanID = ld.LoanID,
                        LoanTypeID = l.LoanTypeID,
                        LoanTypeName = lt.LoanTypeName,
                        MemberID = m.MemberID,
                        Name = m.Name,
                        Seq = ld.Seq,
                        DueDate = ld.DueDate,
                        LoanDueAmt = ld.LoanDueAmt,
                        BFLoanDueAmt = ld.BFLoanDueAmt,
                        LoanDate = l.LoanDate,
                        LoanAmt = l.LoanAmt,
                        LoanBal = l.LoanBal
                    };
            return d.FirstOrDefault();
        }
    }
}
