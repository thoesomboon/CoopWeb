using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.POCO
{
    public class BatMthLoanModel
    {
        //public LoanTypeModel LoanType { get; set; }
        public int CoopID { get; set; }
        public string LoanTypeID { get; set; }
        public string LoanTypeName { get; set; }
        public string BudgetYear { get; set; }
        public int Period { get; set; }
        public Nullable<System.DateTime> CalcDate { get; set; }
        public string CalcDateTH { get; set; }
        public int UserId { get; set; }
    }
}
