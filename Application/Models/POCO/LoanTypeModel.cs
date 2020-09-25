using System;
using Coop.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coop.Models.POCO
{
    public class LoanTypeModel
    {
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Filestatus { get; set; }
        public int CoopID { get; set; }
        public string LoanTypeID { get; set; }
        public string LoanTypeName { get; set; }
        public string IntType { get; set; }
        public Nullable<int> NoMemberMonths { get; set; }
        public Nullable<decimal> MinLoanAmt { get; set; }
        public Nullable<decimal> MaxLoanAmt { get; set; }
        public Nullable<bool> SecurityFlag { get; set; }
        public Nullable<int> CalcIntFlag { get; set; }
        public Nullable<int> NoOfLoanYears { get; set; }
        public Nullable<int> LastLoanID { get; set; }
        public string PrefixLoanID { get; set; }
        public Nullable<int> LastRequestNo { get; set; }
        public string PrefixRequestNo { get; set; }
        public Nullable<decimal> ChargeRate { get; set; }
        public Nullable<decimal> DiscIntRate { get; set; }
        public Nullable<int> DiscIntFlag { get; set; }
    }
}
