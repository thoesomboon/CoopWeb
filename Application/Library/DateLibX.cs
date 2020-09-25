using System;
using System.Globalization;
using Coop.Infrastructure.Helpers;

namespace SaLPro.Library
{
    public class DateLib
    {
        public const int gsDateLen = 10; //Public Const gsDateLen = 10

        public enum DateInterval
        {
            Day,
            DayOfYear,
            Hour,
            Minute,
            Month,
            Quarter,
            Second,
            Weekday,
            WeekOfYear,
            Year
        }

        public static long DateDiff(DateInterval Interval, System.DateTime dateOne, System.DateTime dateTwo)
        {
            switch (Interval)
            {
                case DateInterval.Day:
                case DateInterval.DayOfYear:
                    System.TimeSpan spanForDays = dateTwo - dateOne;
                    return (long)spanForDays.TotalDays;
                case DateInterval.Hour:
                    System.TimeSpan spanForHours = dateTwo - dateOne;
                    return (long)spanForHours.TotalHours;
                case DateInterval.Minute:
                    System.TimeSpan spanForMinutes = dateTwo - dateOne;
                    return (long)spanForMinutes.TotalMinutes;
                case DateInterval.Month:
                    return ((dateTwo.Year - dateOne.Year) * 12) + (dateTwo.Month - dateOne.Month);
                case DateInterval.Quarter:
                    var dateOneQuarter = (long)System.Math.Ceiling(dateOne.Month / 3.0);
                    var dateTwoQuarter = (long)System.Math.Ceiling(dateTwo.Month / 3.0);
                    return (4 * (dateTwo.Year - dateOne.Year)) + dateTwoQuarter - dateOneQuarter;
                case DateInterval.Second:
                    System.TimeSpan spanForSeconds = dateTwo - dateOne;
                    return (long)spanForSeconds.TotalSeconds;
                case DateInterval.Weekday:
                    var spanForWeekdays = dateTwo - dateOne;
                    return (long)(spanForWeekdays.TotalDays / 7.0);
                case DateInterval.WeekOfYear:
                    var dateOneModified = dateOne;
                    var dateTwoModified = dateTwo;
                    while (System.Globalization.DateTimeFormatInfo.CurrentInfo != null && dateTwoModified.DayOfWeek != System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek)
                    {
                        dateTwoModified = dateTwoModified.AddDays(-1);
                    }
                    while (System.Globalization.DateTimeFormatInfo.CurrentInfo != null && dateOneModified.DayOfWeek != System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek)
                    {
                        dateOneModified = dateOneModified.AddDays(-1);
                    }
                    System.TimeSpan spanForWeekOfYear = dateTwoModified - dateOneModified;
                    return (long)(spanForWeekOfYear.TotalDays / 7.0);
                case DateInterval.Year:
                    return dateTwo.Year - dateOne.Year;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Returns a Date value containing a date and time value to which a specified time interval has been added.
        /// </summary>
        /// <param name="Interval">Required. DateInterval enumeration value or String expression representing the time interval you want to add.</param>
        /// <param name="Number">Required. Double. Floating-point expression representing the number of intervals you want to add. Number can be positive (to get date/time values in the future) or negative (to get date/time values in the past). It can contain a fractional part when Interval specifies hours, minutes, or seconds. For other values of Interval, any fractional part of Number is ignored.</param>
        /// <param name="DateValue">Required. Date. An expression representing the date and time to which the interval is to be added. DateValue itself is not changed in the calling program.</param>
        /// <returns>Returns a Date value containing a date and time value to which a specified time interval has been added.</returns>
        /// <remarks>Wrapper: You can use the DateAdd function to add or subtract a specified time interval from a date. For example, you can calculate a date 30 days from today or a time 45 minutes before now.
        /// <br/>To add days to DateValue, you can use DateInterval.Day, DateInterval.DayOfYear, or DateInterval.Weekday. These are treated as equivalent because DayOfYear and Weekday are not meaningful time intervals.
        /// <br/>The DateAdd function never returns an invalid date. If necessary, the day part of the resulting date is adjusted downward to the last day of the resulting month in the resulting year. The following example adds one month to January 31:</remarks>
        /// <example>DateTime Date = DateAdd(DateInterval.Month, 1, #1/31/1995#)</example>
        //public static DateTime DateAdd(DateInterval Interval, double Number, DateTime DateValue)
        //{
        //    /// not complete yet
        //    return DateValue.AddDays(Number);
        //}

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

        //public static string DateEntoTh(DateTime date)
        //{
        //    var dateTh = date.ToString("dd/MM/") + (date.Year + 543).ToString(CultureInfo.InvariantCulture);
        //    return dateTh;
        //}

        ////public static DateTime DateTHtoEn(this string strDate)
        ////{
        ////    DateTime functionReturnValue;
        ////    //dynamic DateConvert = null;
        ////    //please check again ??
        ////    //Remark DateIn must format "dd/mm/yyyy Or dd/mm/yy ( Year Thai ) only ( Bordin )
        ////    //Remark DateOutput ==> dd/mm/yyyy

        ////    string dateConvert = strDate;

        ////    if (Microsoft.VisualBasic.Strings.Len(dateConvert) == gsDateLen) //If Len(DateConvert) = gsDateLen Then
        ////    {
        ////        dateConvert = Microsoft.VisualBasic.Strings.Left(dateConvert, 6) + Microsoft.VisualBasic.Strings.Format(Microsoft.VisualBasic.Conversion.Val(Microsoft.VisualBasic.Strings.Right(dateConvert, 4)) - 543, "0000"); //DateConvert = Left(DateConvert, 6) & Format(Val(Right(DateConvert, 4)) - 543, "0000")
        ////        functionReturnValue = DateTime.Parse(dateConvert); //DateInCE = DateConvert
        ////    }
        ////    else if (Microsoft.VisualBasic.Strings.Len(dateConvert) == 8)
        ////    {
        ////        dateConvert = Microsoft.VisualBasic.Strings.Left(dateConvert, 6) + Microsoft.VisualBasic.Strings.Format(2500 + Microsoft.VisualBasic.Conversion.Val(Microsoft.VisualBasic.Strings.Right(dateConvert, 2)) - 543, "0000");
        ////        functionReturnValue = DateTime.Parse(dateConvert); ;
        ////    }
        ////    else
        ////    {
        ////        functionReturnValue = DateTime.Parse(dateConvert);
        ////    }
        ////    return functionReturnValue;

        ////}

        public DateTime DateTHDateTimeCE(string strDate)
        {
            if (string.IsNullOrEmpty(strDate) || strDate.Length != 10)
            {
                return AuthorizeHelper.Current.CoopControls().SystemDate ?? DateTime.Now;
            }
            var year = Convert.ToInt32((strDate.Split('/')[2])) - 543;
            var month = Convert.ToInt32(strDate.Split('/')[1]);
            var day = Convert.ToInt32(strDate.Split('/')[0]);
            var date = new DateTime(year, month, day);
            return date;
            //var date = (Convert.ToDateTime(strDate.Split('/')[1] + "/" + strDate.Split('/')[0] + "/" + (strDate.Split('/')[2]))).AddYears(-543);
        }

        public static void CalcAge(DateTime varDateBegin, DateTime varDateEnd, ref int intYear, ref int intMonth)
        {
            long diffYear = 0;
            if (varDateBegin != default(DateTime))
            {
                diffYear = Microsoft.VisualBasic.DateAndTime.DateDiff(Microsoft.VisualBasic.DateInterval.Year, varDateBegin, varDateEnd);
                DateTime yearAdd = Microsoft.VisualBasic.DateAndTime.DateAdd(Microsoft.VisualBasic.DateInterval.Year, diffYear, varDateBegin);
                if (yearAdd > varDateEnd)
                {
                    diffYear = diffYear - 1;
                }
                intYear = (int)diffYear;
                int diffMonth = (int)Microsoft.VisualBasic.DateAndTime.DateDiff(Microsoft.VisualBasic.DateInterval.Month, varDateBegin, varDateEnd);
                intMonth = diffMonth - (intYear * 12);

                if (intMonth == 12)
                {
                    intMonth = 0;
                    intYear += 1;
                }
            }
        }

        public static DateTime EndDateOfMonth(DateTime dateValue)
        {
            if (dateValue.Equals(default(DateTime)))
            {
                return default(DateTime);
            }

            DateTime newDateTime = new DateTime(dateValue.AddMonths(1).Year, dateValue.AddMonths(1).Month, 1).AddDays(-1);
            return newDateTime;
        }

        public static bool ValidateDateTH(string DateThai = "")
        {
            if (DateThai != null || DateThai != "" || DateThai.Length == 10)
            {
                var day = int.Parse(DateThai.Replace("/", "").Substring(0, 2)) * 1;
                var month = int.Parse(DateThai.Replace("/", "").Substring(2, 2)) * 1;
                var year = int.Parse(DateThai.Replace("/", "").Substring(4, 4)) * 1 - 543;
                var leap = 0;
                if (day * 0 == 0 && month * 0 == 0 && year * 0 == 0)
                {
                    if (year < 0)
                    {
                        return false;
                    }
                    if (month < 1 || month > 12)
                    {
                        return false;
                    }
                    if (day < 1 || day > 31)
                    {
                        return false;
                    }
                    if (year % 4 == 0 || year % 100 == 0 || year % 400 == 0)
                    {
                        leap = 1;
                    }
                    if ((month == 2) && (leap == 1) && (day > 29))
                    {
                        return false;
                    }
                    if ((month == 2) && (leap != 1) && (day > 28))
                    {
                        return false;
                    }
                    if ((day > 31) && ((month == 1) || (month == 3) || (month == 5) || (month == 7) || (month == 8) || (month == 10) || (month == 12)))
                    {
                        return false;
                    }
                    if ((day > 30) && ((month == 4) || (month == 6) || (month == 9) || (month == 11)))
                    {
                        return false;
                    }
                    return true;
                }
            }
            return false;

        }
    }
}