using System;
using Coop.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coop.Models.POCO
{
    public partial class TtlfLoanModel
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
        public string LoanID { get; set; }
        public Nullable<System.DateTime> BackDate { get; set; }
        public Nullable<decimal> BFBal { get; set; }
        public Nullable<decimal> Amt1 { get; set; }
        public Nullable<decimal> Amt2 { get; set; }
        public Nullable<decimal> CFBal { get; set; }
        public Nullable<decimal> PrincipleAmt { get; set; }
        public Nullable<decimal> CFUnpayPrinciple { get; set; }
        public Nullable<decimal> IntCalc { get; set; }
        public Nullable<decimal> IntAmt { get; set; }
        public Nullable<decimal> BFInt { get; set; }
        public Nullable<decimal> UnpayInt { get; set; }
        public Nullable<decimal> ChargeCalc { get; set; }
        public Nullable<decimal> ChargeAmt { get; set; }
        public Nullable<decimal> BFCharge { get; set; }
        public Nullable<decimal> UnpayCharge { get; set; }
        public Nullable<int> PayFlag { get; set; }
        public string OCFlag { get; set; }
        public string AbbCode { get; set; }
        public string TTxnCode { get; set; }
        public string CDCode { get; set; }
        public string ChequeNo { get; set; }
        public string RcptBookNo { get; set; }
        public string RcptRunNo { get; set; }
        public string ECFlag { get; set; }
        public Nullable<int> OverrideID { get; set; }
    }
}
