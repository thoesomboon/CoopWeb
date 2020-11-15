using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.POCO
{
    public class BatYrLoanModel
    {
        //public DepositTypeModel DepositType { get; set; }
        public int CoopID { get; set; }
        public string LoanTypeID { get; set; }
        public string LoanTypeName { get; set; }
        public string BudgetYear { get; set; }
        public int Period1 { get; set; }
        public int Period2 { get; set; }
        public int UserId { get; set; }
    }
}
