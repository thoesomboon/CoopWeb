using System;
using Coop.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coop.Models.POCO
{
    public class DepositTypeModel
    {
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Filestatus { get; set; }
        public int CoopID { get; set; }
        public string DepositTypeID { get; set; }
        public string DepositTypeName { get; set; }
        public string TypeOfDeposit { get; set; }
        public Nullable<decimal> MinOpenAmt { get; set; }
        public Nullable<decimal> MaxOpenAmt { get; set; }
        public Nullable<decimal> MinDepAmt { get; set; }
        public Nullable<decimal> MaxDepAmt { get; set; }
        public Nullable<decimal> MinWithdrawAmt { get; set; }
        public Nullable<decimal> MaxWithdrawAmt { get; set; }
        public Nullable<decimal> MinLedgerBal { get; set; }
        public Nullable<bool> ItemStatus { get; set; }
        public Nullable<bool> MdecimalonthDepAmtStatus { get; set; }
        public Nullable<bool> WithdrawApplyStatus { get; set; }
        public Nullable<decimal> MonthMaxWithdrawAmt { get; set; }
        public Nullable<int> MonthMaxWithdrawTimes { get; set; }
        public Nullable<decimal> MaxChargeAmt { get; set; }
        public Nullable<decimal> MinChargeAmt { get; set; }
        public Nullable<int> WithdrawChargePercent { get; set; }
        public Nullable<decimal> MinBalCalcInt { get; set; }
        public Nullable<bool> WithdrawStatus { get; set; }
        public string MaskOfAccountNo { get; set; }
        public Nullable<int> LastAccountNo { get; set; }
        public Nullable<int> LastBookNo { get; set; }
        public string PostIntTxnCode { get; set; }
        public Nullable<decimal> CloseAccountFee { get; set; }
        public Nullable<int> MonthIntDue { get; set; }
        public string CalcIntType { get; set; }
        public string CalcIntRate { get; set; }
        public Nullable<bool> StepCalcIntFlag { get; set; }
        public string StepCalcIntRate { get; set; }
        public string StepCalcIntType { get; set; }
        public string StepCalcIntRate3 { get; set; }
        public string StepCalcIntType3 { get; set; }
        public string StepCalcIntRate6 { get; set; }
        public string StepCalcIntType6 { get; set; }
        public string StepCalcIntRate9 { get; set; }
        public string StepCalcIntType9 { get; set; }
        public string StepCalcIntRate12 { get; set; }
        public string StepCalcIntType12 { get; set; }
        public string StepCalcIntRate15 { get; set; }
        public string StepCalcIntType15 { get; set; }
        public string StepCalcIntRate18 { get; set; }
        public string StepCalcIntType18 { get; set; }
        public string StepCalcIntRate21 { get; set; }
        public string StepCalcIntType21 { get; set; }
        public Nullable<System.DateTime> BatchIntDueDate1 { get; set; }
        public Nullable<System.DateTime> BatchIntDueDate2 { get; set; }
        public Nullable<System.DateTime> BatchIntDueDate3 { get; set; }
        public Nullable<System.DateTime> BatchIntDueDate4 { get; set; }
    }
}
