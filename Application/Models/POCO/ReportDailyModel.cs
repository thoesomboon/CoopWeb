using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Coop.Models.POCO
{
    public class ReportDailyModel
    {
        public int CoopID { get; set; }
        public string ReportFileName { get; set; }  // ขื่อ RDLC
        public string ReportTitle { get; set; }     // ขื่อรายงาน Heading
        public string ReportSubTitle { get; set; }  // ขื่อรายงาน Sub-Heading
        public string ReportSort { get; set; }
        public int ReportStartDate { get; set; }
        public int ReportEndDate { get; set; }
        public int UserId { get; set; }
        public DataTable DataSet { get; set; }
        public string DataSetName { get; set; }
        public bool IsHasParams { get; set; }
    }
}
