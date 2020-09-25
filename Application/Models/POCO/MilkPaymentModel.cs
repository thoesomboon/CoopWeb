using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.POCO
{
    public class MilkPaymentModel
    {
        public string Filestatus { get; set; }
        public int CoopID { get; set; }
        public string MemberID { get; set; }
        public System.DateTime FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<decimal> Receive { get; set; }
        public Nullable<decimal> AccountNo { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public string LoanID { get; set; }
        public Nullable<System.DateTime> PayDate { get; set; }
        public Nullable<decimal> LoanAmt { get; set; }
        public Nullable<decimal> LoanInt { get; set; }
        public Nullable<decimal> LoanCharge { get; set; }
        public Nullable<decimal> DiscInt { get; set; }
        public Nullable<decimal> DiscInt2 { get; set; }
    }
}