using System;
using Coop.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coop.Models.POCO
{
    public partial class TtlfDepositModel
    {
        public int CoopID { get; set; }
        public System.DateTime TxnDate { get; set; }
        public string TxnDateTH { get; set; }
        public int TxnSeq { get; set; }
        public Nullable<System.DateTime> TxnTime { get; set; }
        public Nullable<int> UserID { get; set; }
        public string WorkstationID { get; set; }
        public string OriginalProcess { get; set; }
        public string Filestatus { get; set; }
        public string MemberID { get; set; }
        public string Name { get; set; }
        public string DepositTypeID { get; set; }
        public string DepositTypeName { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public Nullable<System.DateTime> OpenDate { get; set; }
        public string OpenDateTH { get; set; }
        public Nullable<System.DateTime> LastContact { get; set; }
        public string LastContactTH { get; set; }
        public Nullable<System.DateTime> LastCalcInt { get; set; }
        public string LastCalcIntTH { get; set; }
        public string IntType { get; set; }
        public Nullable<decimal> LedgerBal { get; set; }
        public Nullable<decimal> AvailBal { get; set; }
        public Nullable<decimal> BookBal { get; set; }
        public string BookNo { get; set; }
        public int LastBookLine { get; set; }
        public Nullable<decimal> BFAccInt { get; set; }
        public Nullable<decimal> MonthWithdrawAmt { get; set; }
        public Nullable<int> MonthWithdrawTimes { get; set; }
        public Nullable<decimal> HoldAmt { get; set; }
        public Nullable<decimal> Amt3 { get; set; }
        public Nullable<System.DateTime> BackDate { get; set; }
        public string BackDateTH { get; set; }
        public Nullable<decimal> Amt { get; set; }
        public Nullable<decimal> BFLedgerBal { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<decimal> ChequeAmt { get; set; }
        public Nullable<decimal> CFLedgerBal { get; set; }
        public Nullable<decimal> Fee { get; set; }
        public Nullable<decimal> AccInt { get; set; }
        public Nullable<decimal> ChargeAmt { get; set; }
        public Nullable<decimal> NetAmt { get; set; }
        public Nullable<decimal> IntDueAmt { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<int> ItemNo { get; set; }
        public bool BookFlag { get; set; }
        public string ReferenceNo { get; set; }
        public string BudgetYear { get; set; }
        public string Type { get; set; }
        public string TTxnCode { get; set; }
        public string Descript { get; set; }
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
    }
}
