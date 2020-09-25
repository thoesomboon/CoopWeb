using System;
using Coop.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coop.Models.POCO
{
    public partial class LoanDueModel
    {
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Filestatus { get; set; }
        public int CoopID { get; set; }
        public string LoanID { get; set; }
        public string LoanTypeID { get; set; }
        public string LoanTypeName { get; set; }
        public string MemberID { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> LoanAmt { get; set; }
        public Nullable<decimal> LoanBal { get; set; }
        public Nullable<System.DateTime> LoanDate { get; set; }
        public int Seq { get; set; }
        public System.DateTime DueDate { get; set; }
        public Nullable<decimal> LoanDueAmt { get; set; }
        public Nullable<decimal> BFLoanDueAmt { get; set; }
        public Nullable<decimal> BFLoanDueAmtBeforeUPD { get; set; }
        public Nullable<decimal> BFLoanDueAmtAfterUPD { get; set; }
    }
    public partial class LoanDueListModel
    {
        public string LoanID { get; set; }
        public int Seq { get; set; }
        public System.DateTime DueDate { get; set; }
        public Nullable<decimal> LoanDueAmt { get; set; }
    }
}
