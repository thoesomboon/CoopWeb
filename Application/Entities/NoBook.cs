//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Coop.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class NoBook
    {
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Filestatus { get; set; }
        public int CoopID { get; set; }
        public string AccountNo { get; set; }
        public int Seq { get; set; }
        public Nullable<System.DateTime> TxnDate { get; set; }
        public Nullable<System.DateTime> BackDate { get; set; }
        public string TTxnCode { get; set; }
        public string AbbCode { get; set; }
        public Nullable<int> ItemNo { get; set; }
        public string CDCode { get; set; }
        public Nullable<decimal> TxnAmt { get; set; }
        public Nullable<decimal> CfLedgerBal { get; set; }
        public Nullable<decimal> ChequeAmt { get; set; }
        public Nullable<decimal> Tax { get; set; }
    
        public virtual Deposit Deposit { get; set; }
    }
}
