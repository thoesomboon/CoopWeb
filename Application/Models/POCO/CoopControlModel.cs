using System;
using Coop.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coop.Models.POCO
{
    public class CoopControlModel
    {
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Filestatus { get; set; }
        public int CoopID { get; set; }
        public string CoopName { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string PrevBudgetYear { get; set; }
        public string BudgetYear { get; set; }
        public Nullable<int> AccountPeriod { get; set; }
        public Nullable<int> SystemLogin { get; set; }
        public Nullable<System.DateTime> PrevSystemDate { get; set; }
        public string PrevSystemDateTH { get; set; }
        public Nullable<System.DateTime> SystemDate { get; set; }
        public string SystemDateTH { get; set; }
        public Nullable<System.DateTime> NextSystemDate { get; set; }
        public string NextSystemDateTH { get; set; }
        public Nullable<System.DateTime> StartBudgetDate { get; set; }
        public string StartBudgetDateTH { get; set; }
        public Nullable<System.DateTime> EndBudgetDate { get; set; }
        public string EndBudgetDateTH { get; set; }
        public Nullable<System.DateTime> PrevStartBudgetDate { get; set; }
        public string PrevStartBudgetDateTH { get; set; }
        public Nullable<System.DateTime> PrevEndBudgetDate { get; set; }
        public string PrevEndBudgetDateTH { get; set; }
        public Nullable<System.DateTime> PrevMthProcDate { get; set; }
        public string PrevMthProcDateTH { get; set; }
        public Nullable<System.DateTime> ThisMthProcDate { get; set; }
        public string ThisMthProcDateTH { get; set; }
        public Nullable<System.DateTime> NextMthProcDate { get; set; }
        public string NextMthProcDateTH { get; set; }
        public string MaskMemberId { get; set; }
        public Nullable<decimal> ShareBookValue { get; set; }
        public Nullable<int> DaysINYear { get; set; }
        public Nullable<int> RoundIntMethod { get; set; }
        public string ManagerName { get; set; }
        public Nullable<int> LastReceiptBookNo { get; set; }
        public Nullable<int> LastReceiptRunNo { get; set; }
    }
    [Serializable]
    public class CoopMachineModel
    {
        public string WorkStationID { get; set; }
        public string BranchID { get; set; }
        public string Server { get; set; }
    }
}
