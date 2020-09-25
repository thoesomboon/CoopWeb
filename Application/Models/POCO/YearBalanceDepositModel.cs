using System;
using Coop.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coop.Models.POCO
{
    public class YearBalanceDepositModel
    {
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Filestatus { get; set; }
        public int CoopID { get; set; }
        public string AccountNo { get; set; }
        public string BudgetYear { get; set; }
        public int Period1 { get; set; }
        public int Period2 { get; set; }
        public Nullable<decimal> BFLedgerBal { get; set; }
        public Nullable<decimal> Deposit { get; set; }
        public Nullable<decimal> Withdraw { get; set; }
        public Nullable<decimal> CFLedgerBal { get; set; }
        public Nullable<decimal> AccInt { get; set; }
    }
}
