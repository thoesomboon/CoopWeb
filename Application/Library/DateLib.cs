using Coop.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Library
{
    public class DateLib
    {
        public static DateTime DateInCE(string DateIN)
        {
            if (string.IsNullOrEmpty(DateIN) || DateIN.Length < 8)
            {
                return AuthorizeHelper.Current.CoopControls().SystemDate ?? DateTime.Now;
            }
            var year = Convert.ToInt32((DateIN.Split('/')[2])) - 543;
            var month = Convert.ToInt32(DateIN.Split('/')[1]);
            var day = Convert.ToInt32(DateIN.Split('/')[0]);
            var date = new DateTime(year, month, day);
            return date;
        }
        public static DateTime FirstDateOfMonth(string DateIN)
        {
            if (string.IsNullOrEmpty(DateIN) || DateIN.Length < 8)
            {
                return AuthorizeHelper.Current.CoopControls().SystemDate ?? DateTime.Now;
            }
            var year = Convert.ToInt32((DateIN.Split('/')[2])) - 543;
            var month = Convert.ToInt32(DateIN.Split('/')[1]);
            var day = Convert.ToInt32(DateIN.Split('/')[0]);
            var date = new DateTime(year, month, day);
            return date;
        }
        public static DateTime EndDateOfMonth(string DateIN)
        {
            if (string.IsNullOrEmpty(DateIN) || DateIN.Length < 8)
            {
                return AuthorizeHelper.Current.CoopControls().SystemDate ?? DateTime.Now;
            }
            var year = Convert.ToInt32((DateIN.Split('/')[2])) - 543;
            var month = Convert.ToInt32(DateIN.Split('/')[1]);
            int day = 0;

            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
            {
                day = 31;
            };
            if (month == 4 || month == 6 || month == 9 || month == 11)
            {
                day = 30;
            };
            if (month == 2)
            {
                day = 28;
                if (year % 4 == 0) { day = 29; };
            };
            var date = new DateTime(year, month, day);
            return date;
        }
        public static DateTime WorkingDay(string DateIN)
        {
            //if (DateIN != Null)
            //{
            //    return AuthorizeHelper.Current.CoopControls().SystemDate ?? DateTime.Now;
            //}
            DateTime dateINDate = DateInCE(DateIN);

            DateTime WorkingDate = dateINDate.AddDays(1);
            {
                DayOfWeek day = dateINDate.DayOfWeek;
                //DayOfWeek dayToday = " " + day.ToString();
                if (day == DayOfWeek.Friday)
                {
                    WorkingDate = dateINDate.AddDays(3);
                }
                if (day == DayOfWeek.Saturday)
                {
                    WorkingDate = dateINDate.AddDays(2);
                }
                if (day == DayOfWeek.Sunday)
                {
                    WorkingDate = dateINDate.AddDays(1);
                }
            }
            return WorkingDate;
        }
    }
}