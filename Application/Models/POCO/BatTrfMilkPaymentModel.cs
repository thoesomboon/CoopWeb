using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.POCO
{
    public class BatTrfMilkPaymentModel
    {
        public int CoopID { get; set; }
        public Nullable<System.DateTime> CalcDate { get; set; }
        //DataType(DataType.Upload)
        //Display(Name = "Upload File")
        public string FileName { get; set; }
    }
}