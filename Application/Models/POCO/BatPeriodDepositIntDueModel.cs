using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.POCO
{
    public class BatPeriodDepositIntDueModel
    {
        //public DepositTypeModel DepositType { get; set; }
        public int CoopID { get; set; }
        public string DepositTypeID { get; set; }
        public string DepositTypeName { get; set; }
        public string BudgetYear { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public int UserId { get; set; }
    }
}
