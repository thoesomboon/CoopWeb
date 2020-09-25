using System;
using Coop.Entities;
using Coop.Infrastructure.ActionFilters;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
//using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace Coop.Controllers
{
    public class DateController : Controller
    {
        // GET: Date
        public ActionResult Index()
        {
            return View();
        }
        public static string DateInBE(DateTime DateIN)
        {
            var DateInBE = DateIN.ToString("dd/MM/") + (DateIN.Year + 543).ToString();
            return DateInBE;
        }
        //public static object DateInCE(string strDate)
        //{
        //    object functionReturnValue = null;
        //    //dynamic DateConvert = null;
        //    //please check again ??
        //    //Remark DateIn must format "dd/mm/yyyy Or dd/mm/yy ( Year Thai ) only ( Bordin )
        //    //Remark DateOutput ==> dd/mm/yyyy

        //    string DateConvert = strDate;

        //    if (Microsoft.VisualBasic.Strings.Len(DateConvert) == gsDateLen) //If Len(DateConvert) = gsDateLen Then
        //    {
        //        DateConvert = Microsoft.VisualBasic.Strings.Left(DateConvert, 6) + Microsoft.VisualBasic.Strings.Format(Microsoft.VisualBasic.Conversion.Val(Microsoft.VisualBasic.Strings.Right(DateConvert, 4)) - 543, "0000"); //DateConvert = Left(DateConvert, 6) & Format(Val(Right(DateConvert, 4)) - 543, "0000")
        //        functionReturnValue = DateConvert; //DateInCE = DateConvert
        //    }
        //    else if (Microsoft.VisualBasic.Strings.Len(DateConvert) == 8)
        //    {
        //        DateConvert = Microsoft.VisualBasic.Strings.Left(DateConvert, 6) + Microsoft.VisualBasic.Strings.Format(2500 + Microsoft.VisualBasic.Conversion.Val(Microsoft.VisualBasic.Strings.Right(DateConvert, 2)) - 543, "0000");
        //        functionReturnValue = DateConvert;
        //    }
        //    else
        //    {
        //        functionReturnValue = DateConvert;
        //    }
        //    return functionReturnValue;
        //}
        public DateTime DateInCE(string strDate)
        {
            if (string.IsNullOrEmpty(strDate) || strDate.Length != 10)
            {
                return AuthorizeHelper.Current.CoopControls().SystemDate ?? DateTime.Now;
            }
            var intDT = strDate.ToString().Replace("12:00:00 AM", "");
            var year = Convert.ToInt32((intDT.Split('/')[2])) - 543;
            //var year = Convert.ToInt32((intDT.Split('/')[2])) - 543;
            var month = Convert.ToInt32(intDT.Split('/')[1]);
            var day = Convert.ToInt32(intDT.Split('/')[0]);
            var date = new DateTime(year, month, day);
            return date;
        }
    }
}