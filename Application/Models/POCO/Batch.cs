using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.POCO
{
    public class TransactionResultModel
    {
        public int RowCount { get; set; }
        public int TranCount { get; set; }
        public int ErrorNumber { get; set; }
        public int ErrorSeverity { get; set; }
        public int ErrorState { get; set; }
        public string ErrorProcedure { get; set; }
        public int ErrorLine { get; set; }
        public string Message { get; set; }
    }
}