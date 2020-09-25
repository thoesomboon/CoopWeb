using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.POCO
{
    public class AccessTransactionModels
    {
        public int AccessTransactionID { get; set; }
        public int UserID { get; set; }
        public int? ReportTo { get; set; }
        public DateTime? LoginDate { get; set; }
        public DateTime? LogoutDate { get; set; }
        public string MachineName { get; set; }
        public string IPAddress { get; set; }
        public int LogStatusID { get; set; }
    }
}