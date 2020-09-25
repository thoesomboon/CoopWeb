using System;
using System.Globalization;

namespace Coop.Library
{
    public static class Utility
    {
        public static string FillZero(int data, int lenght)
        {
            var returnValue = data.ToString().Trim();
            string format = "{0:D" + lenght + "}";

            returnValue = string.Format(format, data);            
            
            return returnValue;
        }

        public static string FmtText(string unFmtTextIn, string masks)
        {
            int intCnt = 1;
            string rText = "";
            for (int j = 1; j <= masks.Length; j++)
            {
                string strChr = Microsoft.VisualBasic.Strings.Mid(masks, j, 1);
                switch (strChr)
                {
                    case "#":
                    case "&":
                    case "?":
                    case "A":
                    case "H":
                    case "0":
                        rText = rText + Microsoft.VisualBasic.Strings.Mid(unFmtTextIn, intCnt, 1);
                        intCnt = intCnt + 1;
                        break;
                    case "U":
                        rText = rText + Microsoft.VisualBasic.Strings.Mid(unFmtTextIn, intCnt, 1).ToUpper();
                        intCnt = intCnt + 1;
                        break;
                    case "L":
                        rText = rText + Microsoft.VisualBasic.Strings.Mid(unFmtTextIn, intCnt, 1).ToLower();
                        intCnt = intCnt + 1;
                        break;
                    default:
                        rText = rText + strChr;
                        break;
                }
            }
            return rText;
        }
        public static string UnFmtText(string FmtTextIn, string Masks)
        {
            int i;
            int intCnt = 1;
            string UnFmtText = "";
            for (i = 1; i <= Masks.Length; i++)
            {
                string Cha_Renamed = Microsoft.VisualBasic.Strings.Mid(Masks, i, 1);
                switch (Cha_Renamed)
                {
                    case "#":
                    case "&":
                    case "?":
                    case "A":
                    case "U":
                    case "L":
                    case "H":
                        UnFmtText = UnFmtText + Microsoft.VisualBasic.Strings.Mid(FmtTextIn, i, 1);
                        intCnt = intCnt + 1;
                        break;
                }
            }
            return UnFmtText;
        }

        public static string StrFmtTextRunning(string strMasks, int intRunNo,DateTime systemDate)
        {
            string strTextRunning = "";
            DateTime gsSystemDate = systemDate;
            for (int intCnt = 1; intCnt < strMasks.Length; intCnt++)
            {
                string strChr = Microsoft.VisualBasic.Strings.Mid(strMasks, intCnt, 1);
                string strYY;
                switch (strChr)
                {
                    case "#":
                    case "&":
                    case "?":
                    case "A":
                    case "H":
                        strTextRunning = strTextRunning + Microsoft.VisualBasic.Strings.Mid(strMasks, intCnt, 1);
                        break;
                    case "U":
                        strTextRunning = strTextRunning + Microsoft.VisualBasic.Strings.Mid(strMasks, intCnt, 1).ToUpper();
                        break;
                    case "L":
                        strTextRunning = strTextRunning + Microsoft.VisualBasic.Strings.Mid(strMasks, intCnt, 1).ToLower();
                        break;
                    case "Y":
                        strYY = "";
                        for (int i = 1; i < strMasks.Length; i++)
                        {
                            if (strChr == "Y")
                            {
                                strYY = strYY + strChr;
                                strChr = Microsoft.VisualBasic.Strings.Mid(strMasks, intCnt + i, 1);
                            }
                        }

                        int year = gsSystemDate.Year;
                        strTextRunning = strTextRunning + Microsoft.VisualBasic.Strings.Right(Microsoft.VisualBasic.Conversion.Str(year), strYY.Replace("พ", "Y").Length);
                        intCnt = intCnt + (strYY.Length - 1);
                        break;
                    case "y":
                        strYY = "";
                        for (int i = 1; i < strMasks.Length; i++)
                        {
                            if (strChr == "y")
                            {
                                strYY = strYY + strChr;
                                strChr = Microsoft.VisualBasic.Strings.Mid(strMasks, intCnt + i, 1);
                            }
                        }
                        int yearyy = gsSystemDate.Year;
                        strTextRunning = strTextRunning +
                                         Microsoft.VisualBasic.Strings.Right(Microsoft.VisualBasic.Conversion.Str((yearyy + 543)), strYY.Replace("พ", "Y").Length);
                        intCnt = intCnt + (strYY.Length - 1);
                        break;
                    case "M":
                        strYY = "";
                        for (int i = 1; i < strMasks.Length; i++)
                        {
                            if (strChr == "M")
                            {
                                strYY = strYY + strChr;
                                strChr = Microsoft.VisualBasic.Strings.Mid(strMasks, intCnt + i, 1);
                            }
                        }
                        strTextRunning = strTextRunning + gsSystemDate.ToString(strYY);
                        intCnt = intCnt + (strYY.Length - 1);
                        break;
                    case "D":
                        strYY = "";
                        for (int i = 1; i < strMasks.Length; i++)
                        {
                            if (strChr == "D")
                            {
                                strYY = strYY + strChr;
                                strChr = Microsoft.VisualBasic.Strings.Mid(strMasks, intCnt + i, 1);
                            }
                        }
                        strTextRunning = strTextRunning + gsSystemDate.ToString(strYY);
                        intCnt = intCnt + (strYY.Length - 1);
                        break;
                    case "R":
                        strYY = "";
                        for (int i = 1; i < strMasks.Length; i++)
                        {
                            if (strChr == "R")
                            {
                                strYY = strYY + strChr;
                                strChr = Microsoft.VisualBasic.Strings.Mid(strMasks, intCnt + i, 1);
                            }
                        }
                        strTextRunning = strTextRunning + intRunNo.ToString(strYY.Replace("R", "0"));
                        intCnt = intCnt + (strYY.Length - 1);
                        break;
                    default:
                        strTextRunning = strTextRunning + strChr;
                        break;
                }
            }
            return strTextRunning;
        }

        public static DateTime ConvertDateTHtoEn(this string strDate)
        {
            DateTime functionReturnValue;
            //dynamic DateConvert = null;
            //please check again ??
            //Remark DateIn must format "dd/mm/yyyy Or dd/mm/yy ( Year Thai ) only ( Bordin )
            //Remark DateOutput ==> dd/mm/yyyy

            string dateConvert = strDate;
            if (Microsoft.VisualBasic.Strings.Len(dateConvert) == 10) //If Len(DateConvert) = gsDateLen Then
            {
                //dateConvert = Microsoft.VisualBasic.Strings.Left(dateConvert, 6) + Microsoft.VisualBasic.Strings.Format(Microsoft.VisualBasic.Conversion.Val(Microsoft.VisualBasic.Strings.Right(dateConvert, 4)) - 543, "0000"); //DateConvert = Left(DateConvert, 6) & Format(Val(Right(DateConvert, 4)) - 543, "0000")
                dateConvert = Microsoft.VisualBasic.Strings.Right(Microsoft.VisualBasic.Strings.Left(dateConvert, 6), 3) + Microsoft.VisualBasic.Strings.Left(dateConvert, 3) + Microsoft.VisualBasic.Strings.Format(Microsoft.VisualBasic.Conversion.Val(Microsoft.VisualBasic.Strings.Right(dateConvert, 4)) - 543, "0000");
                functionReturnValue = DateTime.Parse(dateConvert); //DateInCE = DateConvert
            }
            else if (Microsoft.VisualBasic.Strings.Len(dateConvert) == 8)
            {
                dateConvert = Microsoft.VisualBasic.Strings.Right(Microsoft.VisualBasic.Strings.Left(dateConvert, 6), 3) + Microsoft.VisualBasic.Strings.Left(dateConvert, 3) + Microsoft.VisualBasic.Strings.Format(2500 + Microsoft.VisualBasic.Conversion.Val(Microsoft.VisualBasic.Strings.Right(dateConvert, 2)) - 543, "0000");
                functionReturnValue = DateTime.Parse(dateConvert);
            }
            else
            {
                functionReturnValue = DateTime.Parse(dateConvert);
            }
            return functionReturnValue;
        }

        public static string ChkIdCardFormat(string id_card = "")
        {
            string strIdCardReplace = id_card.Replace("-", "");
            strIdCardReplace = strIdCardReplace.Replace("_", "");
            var strIdCardLength = strIdCardReplace.Length;
            if (strIdCardLength != 13)
            {
                return "หมายเลขบัตรประชาชนต้องกรอก 13 ตัว";
            }
            if (strIdCardLength == 13)
            {
                int idCardValue = 0;
                idCardValue += Convert.ToInt32(strIdCardReplace.Substring(0, 1)) * 13;
                idCardValue += Convert.ToInt32(strIdCardReplace.Substring(1, 1)) * 12;
                idCardValue += Convert.ToInt32(strIdCardReplace.Substring(2, 1)) * 11;
                idCardValue += Convert.ToInt32(strIdCardReplace.Substring(3, 1)) * 10;
                idCardValue += Convert.ToInt32(strIdCardReplace.Substring(4, 1)) * 9;
                idCardValue += Convert.ToInt32(strIdCardReplace.Substring(5, 1)) * 8;
                idCardValue += Convert.ToInt32(strIdCardReplace.Substring(6, 1)) * 7;
                idCardValue += Convert.ToInt32(strIdCardReplace.Substring(7, 1)) * 6;
                idCardValue += Convert.ToInt32(strIdCardReplace.Substring(8, 1)) * 5;
                idCardValue += Convert.ToInt32(strIdCardReplace.Substring(9, 1)) * 4;
                idCardValue += Convert.ToInt32(strIdCardReplace.Substring(10, 1)) * 3;
                idCardValue += Convert.ToInt32(strIdCardReplace.Substring(11, 1)) * 2;
                double temp = Convert.ToDouble(idCardValue / 11);
                idCardValue = 11 - (idCardValue - (int)(Math.Floor(temp)) * 11);
                // ถ้าไม่เท่ากัน Return false ถ้าเท่ากัน Return true
                if (idCardValue.ToString(CultureInfo.InvariantCulture).Substring(idCardValue.ToString(CultureInfo.InvariantCulture).Length - 1, 1)
                        != strIdCardReplace.Substring(strIdCardReplace.Length - 1, 1))
                {
                    return "หมายเลขบัตรประชาชนไม่ถูกต้อง";
                }
            }
            return "";
        }

        public static double Rounded(double value)
        {
            double returnValue = 0;
            double Amt = 0, AmtReal = 0;//Dim Amt As Double, AmtReal As Double

            Amt = (value * 1000 + 5); //Amt = Int(Amount * 1000 + 5)
            AmtReal = (int)(Amt / 10); //AmtReal = Int(Amt / 10)
            AmtReal = AmtReal / 100; //AmtReal = AmtReal / 100
            returnValue = AmtReal; //Rounded = AmtReal

            return returnValue;
        }
    }
}
