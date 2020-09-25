using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.POCO
{
    public class OpenRequestLoanModel
    {
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Filestatus { get; set; }
        public int CoopID { get; set; }
        public string RequestNo { get; set; }
        public string LoanTypeID { get; set; }
        public string MemberID { get; set; }
        public string Address { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public Nullable<decimal> RequestAmt { get; set; }
        public string ReasonTypeID { get; set; }
        public Nullable<bool> AuthorizeFlag { get; set; }
        public string LoanUsageDescript1 { get; set; }
        public Nullable<decimal> LoanUsage1 { get; set; }
        public string LoanUsageDescript2 { get; set; }
        public Nullable<decimal> LoanUsage2 { get; set; }
        public string LoanUsageDescript3 { get; set; }
        public Nullable<decimal> LoanUsage3 { get; set; }
        public string IncomeDescript1 { get; set; }
        public string IncomeArea1 { get; set; }
        public Nullable<decimal> IncomeAmt1 { get; set; }
        public string IncomeDescript2 { get; set; }
        public string IncomeArea2 { get; set; }
        public Nullable<decimal> IncomeAmt2 { get; set; }
        public string IncomeDescript3 { get; set; }
        public string IncomeArea3 { get; set; }
        public Nullable<decimal> IncomeAmt3 { get; set; }
        public string SecurityLicenceNo1 { get; set; }
        public string SecurityOwnerName1 { get; set; }
        public string SecurityArea1 { get; set; }
        public Nullable<decimal> SecurityValue1 { get; set; }
        public string SecurityLicenceNo2 { get; set; }
        public string SecurityOwnerName2 { get; set; }
        public string SecurityArea2 { get; set; }
        public Nullable<decimal> SecurityValue2 { get; set; }
        public string SecurityLicenceNo3 { get; set; }
        public string SecurityOwnerName3 { get; set; }
        public string SecurityArea3 { get; set; }
        public Nullable<decimal> SecurityValue3 { get; set; }
        public string LoanID { get; set; }
    }
}