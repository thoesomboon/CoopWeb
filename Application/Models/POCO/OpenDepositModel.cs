using System;
using Coop.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coop.Models.POCO
{
    public class OpenDepositModel
    {
        public OperationResult OperationResult { get; set; }
        public int CoopID { get; set; }
        public string AccountNo { get; set; }
        public string DepositTypeID { get; set; }
        public string DepositTypeName { get; set; }
        public string TypeOfDeposit { get; set; }
        public string MemberID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string AccountName { get; set; }
        public string BookNo { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public String BirthDateTH { get; set; }
        public Nullable<System.DateTime> ApplyDate { get; set; }
        public String ApplyDateTH { get; set; }
        public string IdCard { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string IntType { get; set; }
        //ส่วนนี้เป็น ของ DepositType
        public Nullable<decimal> MinOpenAmt { get; set; }
        public Nullable<decimal> MaxOpenAmt { get; set; }
        //ส่วนนี้เป็น ส่วนของ Txn ใช้ในการคำนวณ
        public string TTxnCode { get; set; }
        public string Descript { get; set; }
        public string CDCode { get; set; }
        public string OCFlag { get; set; }
        public Nullable<System.DateTime> TxnDate { get; set; }
        public String TxnDateTH { get; set; }
        public Nullable<decimal> Amt { get; set; }
        public Nullable<decimal> Fee { get; set; }
        public string ReferenceNo { get; set; }
        public string AbbCode { get; set; }
        public Nullable<int> ItemNo { get; set; }
        public bool BookFlag { get; set; }
        public string InstrumentType { get; set; }
        //public Nullable<decimal> Credit { get; set; }
        //public string InstrumentType { get; set; }
        //public string Filestatus { get; set; }
    }
}
