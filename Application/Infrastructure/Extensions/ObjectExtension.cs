using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Coop.Infrastructure.Extensions
{
    public static class ObjectExtension
    {
        /// <summary>
        ///   Creates a comma delimeted string of all the objects property values names.
        /// </summary>
        /// <param name="expression"> object. </param>
        /// <returns> string. </returns>
        public static string ToCSV(this object expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("obj", "Value can not be null or Nothing!");
            }

            var sb = new StringBuilder();
            var t = expression.GetType();
            var pi = t.GetProperties();

            for (var index = 0; index < pi.Length; index++)
            {
                sb.Append(pi[index].GetValue(expression, null));

                if (index < pi.Length - 1)
                {
                    sb.Append(",");
                }
            }

            return sb.ToString();
        }

        public static bool IsNumeric(this object expresstion)
        {
            if (expresstion == null || expresstion is DateTime)
                return false;

            if (expresstion is Int16 || expresstion is Int32 || expresstion is Int64 || expresstion is Decimal ||
                expresstion is Single || expresstion is Double || expresstion is Boolean)
                return true;

            try
            {
                if (expresstion is string)
                    Double.Parse(expresstion as string);
                else
                    Double.Parse(expresstion.ToString());
                return true;
            }
            catch (Exception)
            {
            } // just dismiss errors but return false
            return false;
        }

        public static object TryConvertOADateTime(this object value)
        {
            object r;

            try
            {
                var dValue = double.Parse(value.ToString());

                r = DateTime.FromOADate(dValue).ToString(CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
                r = value;
            }

            return r;
        }

        public static object TryConvertDMYEngDateTime(this object value)
        {
            object r;
            try
            {
                string sValue = value.ToString();
                //รูปแบบ 31212
                if (sValue.Length == 5)
                {
                    sValue = "20" + sValue.Substring(3, 2) + "-" + sValue.Substring(1, 2) + "-0" + sValue.Substring(0, 1);
                }
                //รูปแบบ 301212
                else if (sValue.Length == 6)
                {
                    sValue = "20" + sValue.Substring(4, 2) + "-" + sValue.Substring(2, 2) + "-" + sValue.Substring(0, 2);
                }
                else if (sValue.Length == 7)
                {
                    sValue = sValue.Substring(3, 4) + "-" + sValue.Substring(1, 2) + "-0" + sValue.Substring(0, 1);
                }
                //รูปแบบ 30122012
                else if (sValue.Length == 8)
                {
                    sValue = sValue.Substring(4, 4) + "-" + sValue.Substring(2, 2) + "-" + sValue.Substring(0, 2);
                }
                r = (sValue == "0" || string.IsNullOrEmpty(sValue)) ? null : DateTime.Parse(sValue).ToString("yyyy-MM-dd");
            }
            catch (FormatException)
            {
                r = null;
            }
            return r;
        }

        public static object TryConvertYMDThaiDateTime(this object value)
        {
            object r;
            try
            {
                string sValue = value.ToString();
                //รูปแบบ 54011
                if (sValue.Length == 5)
                {
                    sValue = (Convert.ToInt32("25" + sValue.Substring(0, 2)) - 543) + "-" + sValue.Substring(2, 2) + "-0" + sValue.Substring(4, 1);
                }
                //รูปแบบ 540101
                else if (sValue.Length == 6)
                {
                    sValue = (Convert.ToInt32("25" + sValue.Substring(0, 2)) - 543) + "-" + sValue.Substring(2, 2) + "-" + sValue.Substring(4, 2);
                }
                //รูปแบบ 2554011
                else if (sValue.Length == 7)
                {
                    sValue = (Convert.ToInt32(sValue.Substring(0, 4)) - 543) + "-" + sValue.Substring(4, 2) + "-0" + sValue.Substring(6, 1);
                }
                //รูปแบบ 25540101
                else if (sValue.Length == 8)
                {
                    sValue = (Convert.ToInt32(sValue.Substring(0, 4)) - 543) + "-" + sValue.Substring(4, 2) + "-" + sValue.Substring(6, 2);
                }
                r = (sValue == "0" || string.IsNullOrEmpty(sValue)) ? null : DateTime.Parse(sValue).ToString("yyyy-MM-dd");
            }
            catch (FormatException)
            {
                r = null;
            }
            return r;
        }

        public static object TryConvertDMYThaiDateTime(this object value)
        {
            object r;
            try
            {   // 06/12/2554
                string sValue = value.ToString();
                if (sValue.IndexOf('/') != -1) { sValue = sValue.Split('/')[0] + (sValue.Split('/')[1].Length == 1 ? "0" + sValue.Split('/')[1] : sValue.Split('/')[1]) + sValue.Split('/')[2]; }

                //รูปแบบ 61254
                if (sValue.Length == 5)
                {
                    sValue = (Convert.ToInt32("25" + sValue.Substring(3, 2)) - 543) + "-" + sValue.Substring(1, 2) + "-0" + sValue.Substring(0, 1);
                }
                //รูปแบบ 061254
                else if (sValue.Length == 6)
                {
                    sValue = (Convert.ToInt32("25" + sValue.Substring(4, 2)) - 543) + "-" + sValue.Substring(2, 2) + "-" + sValue.Substring(0, 2);
                }
                //รูปแบบ 60122554
                else if (sValue.Length == 7)
                {
                    sValue = (Convert.ToInt32(sValue.Substring(3, 4)) - 543) + "-" + sValue.Substring(1, 2) + "-0" + sValue.Substring(0, 1);
                }
                //รูปแบบ 06122554
                else if (sValue.Length == 8)
                {
                    sValue = (Convert.ToInt32(sValue.Substring(4, 4)) - 543) + "-" + sValue.Substring(2, 2) + "-" + sValue.Substring(0, 2);
                }
                r = (sValue == "0" || string.IsNullOrEmpty(sValue)) ? null : DateTime.Parse(sValue).ToString("yyyy-MM-dd");
            }
            catch (FormatException)
            {
                r = null;
            }
            return r;
        }
    }
}