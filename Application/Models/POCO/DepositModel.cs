using System;
using Coop.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Coop.Models.POCO
{
    public class DepositModel
    {
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Filestatus { get; set; }
        public int CoopID { get; set; }
        public string AccountNo { get; set; }
        public string DepositTypeID { get; set; }
        public string DepositTypeName { get; set; }
        public string MemberID { get; set; }
        public string Name { get; set; }
        public string AccountName { get; set; }
        public string BookNo { get; set; }
        public Nullable<System.DateTime> OpenDate { get; set; }
        public string OpenDateTH { get; set; }
        public Nullable<System.DateTime> LastContact { get; set; }
        public string LastContactTH { get; set; }
        public Nullable<System.DateTime> LastCalcInt { get; set; }
        public string LastCalcIntTH { get; set; }
        public string IntType { get; set; }
        public Nullable<decimal> BFLedgerBal { get; set; }
        public Nullable<decimal> LedgerBal { get; set; }
        public Nullable<decimal> AvailBal { get; set; }
        public Nullable<decimal> BookBal { get; set; }
        public Nullable<decimal> AccInt { get; set; }
        public Nullable<int> LastLedgerLine { get; set; }
        public Nullable<int> LastBookLine { get; set; }
        public Nullable<int> BookPage { get; set; }
        public Nullable<int> HoldTypeID { get; set; }
        public Nullable<decimal> HoldAmt { get; set; }
        public Nullable<decimal> IntDueAmt { get; set; }
        public string BudgetYear { get; set; }
        public Nullable<decimal> UnpayInt { get; set; }
        public Nullable<int> BookSeq { get; set; }
        public Nullable<decimal> MonthDepAmt { get; set; }
        public Nullable<System.DateTime> MonthDepositDate { get; set; }
        public String MonthDepositDateTH { get; set; }
        public Nullable<decimal> MonthWithdrawAmt { get; set; }
        public Nullable<int> MonthWithdrawTimes { get; set; }
        public Nullable<decimal> Amt1 { get; set; }
        public Nullable<decimal> Amt2 { get; set; }
        public Nullable<decimal> Amt3 { get; set; }
        ////ส่วนนี้เป็นของ DepositType
        //public string CalcIntType { get; set; }
        //public string CalcIntRate { get; set; }
        //public Nullable<double> MonthMaxWithdrawAmt { get; set; }
        //public Nullable<short> MonthMaxWithdrawTimes { get; set; }
        //public Nullable<decimal> MaxChargeAmt { get; set; }
        //public Nullable<decimal> MinChargeAmt { get; set; }
        //public Nullable<int> WithdrawChargePercent { get; set; }
        //public Nullable<decimal> MinDepAmt { get; set; }
        //public Nullable<decimal> MaxDepAmt { get; set; }
        //public Nullable<decimal> MinWithdrawAmt { get; set; }
        //public Nullable<decimal> MaxWithdrawAmt { get; set; }
        //public Nullable<decimal> MinLedgerBal { get; set; }
        ////ส่วนนี้เป็นของ ดอกเบี้ยคำนวณ
        //public Nullable<decimal> IntAmt { get; set; }
        //public Nullable<System.DateTime> TxnDate { get; set; }
        //public string TxnDateTH { get; set; }
    }
}
