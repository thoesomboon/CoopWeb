using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.POCO
{
    public class MonthBalanceLoanModel
    {
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Filestatus { get; set; }
        public int CoopID { get; set; }
        public string LoanID { get; set; }
        public string BudgetYear { get; set; }
        public int Period { get; set; }
        public string LoanTypeID { get; set; }
        public string MemberID { get; set; }
        public Nullable<decimal> BFbalance { get; set; }
        public Nullable<decimal> BalanceD { get; set; }
        public Nullable<decimal> BalanceC { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<decimal> BalancecDue { get; set; }
        public Nullable<decimal> BalanceCBDue { get; set; }
        public Nullable<decimal> BFint { get; set; }
        public Nullable<decimal> BFintC { get; set; }
        public Nullable<decimal> IntCalC { get; set; }
        public Nullable<decimal> IntCalcC { get; set; }
        public Nullable<decimal> UnpayIntCalc { get; set; }
        public Nullable<decimal> BFcharge { get; set; }
        public Nullable<decimal> BFChargeC { get; set; }
        public Nullable<decimal> IntCharge { get; set; }
        public Nullable<decimal> IntChargeC { get; set; }
        public Nullable<decimal> UnpayChargeCalc { get; set; }
        public Nullable<decimal> BalanceA { get; set; }
        public Nullable<decimal> BalanceCB { get; set; }
        public Nullable<decimal> BalanceCM { get; set; }
        public Nullable<decimal> BalanceCN { get; set; }
        public Nullable<decimal> BalanceCR { get; set; }
        public Nullable<decimal> BalanceCt { get; set; }
        public Nullable<decimal> BFDiscInt { get; set; }
        public Nullable<decimal> DiscIntCalc { get; set; }
        public Nullable<decimal> DiscIntCalcC { get; set; }
        public Nullable<decimal> UnpayDiscIntCalc { get; set; }
    }
}