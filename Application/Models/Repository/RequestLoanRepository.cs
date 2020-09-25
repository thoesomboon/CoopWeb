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
    public interface IRequestLoanRepository : IRepository<RequestLoan>
    {
        IQueryable<RequestLoanModel> ReadDetail();
        IQueryable<RequestLoanModel> ReadDetail(string ReqNo);
    }

    public class RequestLoanRepository : Repository<RequestLoan>, IRequestLoanRepository
    {
        public RequestLoanRepository(CoopWebEntities context) : base(context)
        {
        }
        public IQueryable<RequestLoanModel> ReadDetail()
        {
            var reqLoan = from r in Read()
                select new RequestLoanModel
                {
                    CreatedBy = r.CreatedBy,
                    CreatedDate = r.CreatedDate,
                    ModifiedBy = r.ModifiedBy,
                    ModifiedDate = r.ModifiedDate,
                    Filestatus = r.Filestatus,
                    CoopID = r.CoopID,
                    RequestNo = r.RequestNo,
                    LoanTypeID = r.LoanTypeID,
                    MemberID = r.MemberID,
                    Address = r.Address,
                    RequestDate = r.RequestDate,
                    RequestAmt = r.RequestAmt,
                    ReasonTypeID = r.ReasonTypeID,
                    AuthorizeFlag = r.AuthorizeFlag,
                    LoanUsageDescript1 = r.LoanUsageDescript1,
                    LoanUsage1 = r.LoanUsage1,
                    LoanUsageDescript2 = r.LoanUsageDescript2,
                    LoanUsage2 = r.LoanUsage2,
                    LoanUsageDescript3 = r.LoanUsageDescript3,
                    LoanUsage3 = r.LoanUsage3,
                    IncomeDescript1 = r.IncomeDescript1,
                    IncomeArea1 = r.IncomeArea1,
                    IncomeAmt1 = r.IncomeAmt1,
                    IncomeDescript2 = r.IncomeDescript2,
                    IncomeArea2 = r.IncomeArea2,
                    IncomeAmt2 = r.IncomeAmt2,
                    IncomeDescript3 = r.IncomeDescript3,
                    IncomeArea3 = r.IncomeArea3,
                    IncomeAmt3 = r.IncomeAmt3,
                    SecurityLicenceNo1 = r.SecurityLicenceNo1,
                    SecurityOwnerName1 = r.SecurityOwnerName1,
                    SecurityArea1 = r.SecurityArea1,
                    SecurityValue1 = r.SecurityValue1,
                    SecurityLicenceNo2 = r.SecurityLicenceNo2,
                    SecurityOwnerName2 = r.SecurityOwnerName2,
                    SecurityArea2 = r.SecurityArea2,
                    SecurityValue2 = r.SecurityValue2,
                    SecurityLicenceNo3 = r.SecurityLicenceNo3,
                    SecurityOwnerName3 = r.SecurityOwnerName3,
                    SecurityArea3 = r.SecurityArea3,
                    SecurityValue3 = r.SecurityValue3,
                    LoanID = r.LoanID

                };
            return reqLoan;
        }
        public IQueryable<RequestLoanModel> ReadDetail(string ReqNo)
        {
            var loan = ReadDetail().Where(l => l.RequestNo == ReqNo);
            return loan;
        }
    }
}
