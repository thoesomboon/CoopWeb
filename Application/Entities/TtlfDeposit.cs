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
    
    public partial class TtlfDeposit
    {
        public int CoopID { get; set; }
        public System.DateTime TxnDate { get; set; }
        public int TxnSeq { get; set; }
        public Nullable<System.DateTime> TxnTime { get; set; }
        public Nullable<int> UserID { get; set; }
        public string WorkstationID { get; set; }
        public string OriginalProcess { get; set; }
        public string Filestatus { get; set; }
        public string MemberID { get; set; }
        public string DepositTypeID { get; set; }
        public string AccountNo { get; set; }
        public Nullable<System.DateTime> BackDate { get; set; }
        public Nullable<decimal> BFLedgerBal { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<decimal> ChequeAmt { get; set; }
        public Nullable<decimal> CFLedgerBal { get; set; }
        public Nullable<decimal> Fee { get; set; }
        public Nullable<decimal> AccInt { get; set; }
        public Nullable<decimal> ChargeAmt { get; set; }
        public Nullable<decimal> IntDueAmt { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<int> ItemNo { get; set; }
        public string BookFlag { get; set; }
        public string ReferenceNo { get; set; }
        public string BudgetYear { get; set; }
        public string Type { get; set; }
        public string TTxnCode { get; set; }
        public string CDCode { get; set; }
        public string InstrumentType { get; set; }
        public string ECFlag { get; set; }
        public string BranchId { get; set; }
        public Nullable<int> OverrideID { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        public string BankID { get; set; }
        public string ClearingFlag { get; set; }
        public Nullable<System.DateTime> ClearingDate { get; set; }
        public string OCFlag { get; set; }
    
        public virtual DepositType DepositType { get; set; }
        public virtual Deposit Deposit { get; set; }
    }
}