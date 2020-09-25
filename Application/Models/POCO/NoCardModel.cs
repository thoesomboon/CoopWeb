using System;
using Coop.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coop.Models.POCO
{
    public class NoCardModel
    {
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Filestatus { get; set; }
        public int CoopID { get; set; }
        public string AccountNo { get; set; }
        public int Seq { get; set; }
        public Nullable<System.DateTime> TxnDate { get; set; }
        public Nullable<System.DateTime> BackDate { get; set; }
        public string TxnCode { get; set; }
        public string AbbCode { get; set; }
        public Nullable<int> ItemNo { get; set; }
        public string CDCode { get; set; }
        public Nullable<decimal> TxnAmt { get; set; }
        public Nullable<decimal> CfLedgerBal { get; set; }
        public Nullable<decimal> ChequeAmt { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<decimal> AccInt { get; set; }
    }
}
