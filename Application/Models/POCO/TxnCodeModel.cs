using System;
using Coop.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coop.Models.POCO
{
    public class TxnCodeModel
    {
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Filestatus { get; set; }
        public int CoopID { get; set; }
        public string TxnType { get; set; }
        public string TTxnCode { get; set; }
        public string Descript { get; set; }
        public string AbbCode { get; set; }
        public string NBKAbbCode { get; set; }
        public string CDCode { get; set; }
        public string OCFlag { get; set; }
        public string ECFlag { get; set; }
        public string InstrumentType { get; set; }
        public Nullable<int> TellerLevel { get; set; }
        public Nullable<int> OverrideLevel { get; set; }
        public Nullable<int> ECOverrideLevel { get; set; }
        public string ProgramName { get; set; }
        public string AddDeleteFlag { get; set; }
        public Nullable<int> PrintSlip { get; set; }
        public string CreditPostGLLedgerNoDR { get; set; }
        public string CreditPostGLLedgerNoCR { get; set; }
        public string DebitPostGLLedgerNoDR { get; set; }
        public string DebitPostGLLedgerNoCR { get; set; }
        public string FeePostGLLedgerNoDR { get; set; }
        public string FeePostGLLedgerNoCR { get; set; }
        public string ChequeCreditPostGLLedgerNoDR { get; set; }
        public string ChequeCreditPostGLLedgerNoCR { get; set; }
        public string ChequeDebitPostGLLedgerNoDR { get; set; }
        public string ChequeDebitPostGLLedgerNoCR { get; set; }
        public string IntPostGLLedgerNoDR { get; set; }
        public string IntPostGLLedgerNoCR { get; set; }
        public Nullable<int> IntCalcFlag { get; set; }
        public string SlipID { get; set; }
        public Nullable<int> IsActive { get; set; }
    }
}
