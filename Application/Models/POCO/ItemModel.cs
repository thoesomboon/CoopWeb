using System;
using Coop.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coop.Models.POCO
{
    public class ItemModel
    {
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Filestatus { get; set; }
        public int CoopID { get; set; }
        public string AccountNo { get; set; }
        public int ItemNo { get; set; }
        public Nullable<System.DateTime> TxnDate { get; set; }
        public Nullable<System.DateTime> DepositDate { get; set; }
        public Nullable<decimal> DepositAmt { get; set; }
        public Nullable<decimal> DepositBal { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<decimal> IntDueAmt { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<int> ItemNoNew { get; set; }
        public Nullable<decimal> TempAccInt { get; set; }
    }
}
