using System;
using Coop.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coop.Models.POCO
{
    public class OtxLoanModel
    {
        public OperationResult OperationResult { get; set; }
        //public LoanDueModel LoanDue { get; set; }
        //public TtlfLoanModel TtlfLoan { get; set; }
        public int CoopID { get; set; }
        public string Filestatus { get; set; }
        public string LoanID { get; set; }
        public string LoanTypeID { get; set; }
        public string LoanTypeName { get; set; }
        public string MemberID { get; set; }
        public string Name { get; set; }
        public string MemberTypeID { get; set; }
        public string MemberTypeName { get; set; }
        public string MemberGroupID { get; set; }
        public string MemberGroupName { get; set; }
        public Nullable<System.DateTime> LoanDate { get; set; }
        public string LoanDateTH { get; set; }
        public Nullable<System.DateTime> LastContact { get; set; }
        public string LastContactTH { get; set; }
        public Nullable<System.DateTime> StartCalcInt { get; set; }
        public string StartCalcIntTH { get; set; }
        public Nullable<System.DateTime> LastCalcInt { get; set; }
        public string LastCalcIntTH { get; set; }
        public Nullable<System.DateTime> LastCalcCharge { get; set; }
        public string LastCalcChargeTH { get; set; }
        public string IntType { get; set; }
        public Nullable<decimal> LoanAmt { get; set; }
        public Nullable<decimal> BFBal { get; set; }
        public Nullable<decimal> LoanBal { get; set; }
        public Nullable<decimal> BFInt { get; set; }
        public Nullable<decimal> BFCharge { get; set; }
        public Nullable<decimal> AccInt { get; set; }
        public Nullable<decimal> YTDAccInt { get; set; }
        public Nullable<decimal> UnpayInt { get; set; }
        public Nullable<decimal> UnpayPrinciple { get; set; }
        public Nullable<decimal> UnpayCharge { get; set; }
        public Nullable<decimal> IntRate { get; set; }
        public Nullable<int> PayFlag { get; set; }
        public Nullable<decimal> BFUnpayInt { get; set; }
        public Nullable<decimal> BFUnpayCharge { get; set; }
        public Nullable<int> DiscIntFlag { get; set; }
        public Nullable<decimal> BFDiscInt { get; set; }
        public Nullable<decimal> DiscInt { get; set; }
        public Nullable<decimal> BFUnpayDiscInt { get; set; }
        public Nullable<decimal> UnpayDiscInt { get; set; }
        //ส่วนนี้เป็น ส่วนของ Txn ใช้ในการคำนวณ
        public string TTxnCode { get; set; }
        public string Descript { get; set; }
        public Nullable<System.DateTime> TxnDate { get; set; }
        public String TxnDateTH { get; set; }
        public String BackDateTH { get; set; }
        public string CDCode { get; set; }
        public string InstrumentType { get; set; }
        public Nullable<decimal> IntCalc { get; set; }
        public Nullable<decimal> IntAmt { get; set; }
        public Nullable<decimal> DiscIntCalc { get; set; }
        public Nullable<decimal> DiscIntAmt { get; set; }
        public Nullable<decimal> ChargeCalc { get; set; }
        public Nullable<decimal> ChargeAmt { get; set; }
        public Nullable<decimal> Amt1 { get; set; }
        public Nullable<decimal> PrincipleAmt { get; set; }
        public Nullable<decimal> UnpayPrincipleAmt { get; set; }
        public Nullable<decimal> CFLoanBal { get; set; }
        public Nullable<decimal> CFUnpayPrinciple { get; set; }
        public Nullable<decimal> CFUnpayInt { get; set; }
        public Nullable<decimal> CFUnpayCharge { get; set; }
        public Nullable<decimal> CFUnpayDiscInt { get; set; }
        public Nullable<decimal> RcptBookNo { get; set; }
        public Nullable<decimal> RcptRunNo { get; set; }
    }
}
