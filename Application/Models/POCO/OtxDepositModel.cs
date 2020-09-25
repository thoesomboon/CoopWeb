using System;
using Coop.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coop.Models.POCO
{
    public class OtxDepositModel
    {
        //public MemberModel Member { get; set; }
        //public DepositTypeModel DepositType { get; set; }
        //public ItemModel Item { get; set; }
        //public TxnCodeModel TxnCode { get; set; }
        //public TtlfDepositModel TtlfDeposit { get; set; }
        public OperationResult OperationResult { get; set; }
        //public Nullable<int> CreatedBy { get; set; }
        //public Nullable<System.DateTime> CreatedDate { get; set; }
        //public Nullable<int> ModifiedBy { get; set; }
        //public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Filestatus { get; set; }
        public int CoopID { get; set; }
        public string AccountNo { get; set; }
        public string DepositTypeID { get; set; }
        public string DepositTypeName { get; set; }
        public string TypeOfDeposit { get; set; }
        public string MemberID { get; set; }
        public string Name { get; set; }
        public string AccountName { get; set; }
        public string BookNo { get; set; }
        public Nullable<System.DateTime> OpenDate { get; set; }
        public String OpenDateTH { get; set; }
        public Nullable<System.DateTime> LastContact { get; set; }
        public String LastContactTH { get; set; }
        public Nullable<System.DateTime> LastCalcInt { get; set; }
        public String LastCalcIntTH { get; set; }
        public string IntType { get; set; }
        //public Nullable<decimal> BFLedgerBal { get; set; }
        public Nullable<decimal> LedgerBal { get; set; }
        public Nullable<decimal> AvailBal { get; set; }
        public Nullable<decimal> BookBal { get; set; }
        public Nullable<decimal> AccInt { get; set; }
        //public Nullable<int> LastLedgerLine { get; set; }
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
        public Nullable<decimal> MonthMaxWithdrawAmt { get; set; }
        public Nullable<int> MonthMaxWithdrawTimes { get; set; }
        public Nullable<decimal> Amt1 { get; set; }
        public Nullable<decimal> Amt2 { get; set; }
        public Nullable<decimal> Amt3 { get; set; }
        //ส่วนนี้เป็น ของ DepositType
        public string CalcIntType { get; set; }
        public string CalcIntRate { get; set; }
        public Nullable<decimal> MaxChargeAmt { get; set; }
        public Nullable<decimal> MinChargeAmt { get; set; }
        public Nullable<int> WithdrawChargePercent { get; set; }
        public Nullable<decimal> MinDepAmt { get; set; }
        public Nullable<decimal> MaxDepAmt { get; set; }
        public Nullable<decimal> MinWithdrawAmt { get; set; }
        public Nullable<decimal> MaxWithdrawAmt { get; set; }
        public Nullable<decimal> MinLedgerBal { get; set; }
        //ส่วนนี้เป็น ส่วนของ Txn ใช้ในการคำนวณ
        public string TTxnCode { get; set; }
        public string Descript { get; set; }
        public string CDCode { get; set; }
        public string AbbCode { get; set; }
        public string OCFlag { get; set; }
        public Nullable<decimal> BFLedgerBal { get; set; }
        public Nullable<System.DateTime> TxnDate { get; set; }
        public String TxnDateTH { get; set; }        
        public Nullable<System.DateTime> BackDate { get; set; }
        public string BackDateTH { get; set; }        
        public Nullable<decimal> Amt { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> IntAmt { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<decimal> ChargeAmt { get; set; }
        public Nullable<decimal> NetAmt { get; set; }
        public Nullable<decimal> CFLedgerBal { get; set; }
        public Nullable<decimal> ChequeAmt { get; set; }
        public string BookFlag { get; set; }
        public string ECFlag { get; set; }        
        public string InstrumentType { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }        
        public String ChequeDateTH { get; set; }        
        public string BankID { get; set; }
        public Nullable<int> ItemNo { get; set; }
        public String ReferenceNo { get; set; }        
    }
}
