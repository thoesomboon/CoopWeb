using System;
using Coop.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coop.Models.POCO
{
    public class LoanModel
    {
        public string Filestatus { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int CoopID { get; set; }
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
        //public Nullable<System.DateTime> FirstInstallDate { get; set; }
        //public string FirstInstallDateTH { get; set; }
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
        public Nullable<int> ReasonID { get; set; }
        public string ReasonName { get; set; }
        public Nullable<short> PayFlag { get; set; }
        public Nullable<decimal> BFUnpayInt { get; set; }
        public Nullable<decimal> BFUnpayCharge { get; set; }
        public Nullable<decimal> InstallAmt { get; set; }
        public Nullable<int> InstallMethodID { get; set; }
        public string InstallMethodName { get; set; }
        public Nullable<int> DiscIntFlag { get; set; }
        public Nullable<decimal> BFDiscInt { get; set; }
        public Nullable<decimal> DiscInt { get; set; }
        public Nullable<decimal> BFUnpayDiscInt { get; set; }
        public Nullable<decimal> UnpayDiscInt { get; set; }
        public Nullable<decimal> TmpUnpayInt { get; set; }
        public Nullable<decimal> TmpUnpayPrinciple { get; set; }
        public Nullable<decimal> TmpUnpayCharge { get; set; }
        public Nullable<decimal> TmpDiscInt { get; set; }
        public Nullable<decimal> TmpMilkAmt { get; set; }
    }
}
