using Microsoft.VisualBasic;
using Coop.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Coop.Infrastructure.Helpers;

namespace Coop.Library
{
    public class Deposit
    {
        // CalcIntType คือ ตัวแปรของวิธีการคำนวณดอกเบี้ย 6 วิธี 
        //   1. "M" - คำนวณดอกเบี้ยเป็นเดือน เศษของเดือนตัดทิ้ง, อัตราดอกเบี้ยเดียว ณ. วันฝาก
        //   2. "m" - คำนวณดอกเบี้ยเป็นเดือน เศษของเดือนตัดทิ้ง, อัตราดอกเบี้ยเดียว ณ. วันถอน
        //   3. "X" - คำนวณดอกเบี้ยเป็นเดือน เศษของเดือนสุดท้ายตัดทิ้ง, หลายอัตราดอกเบี้ย
        //   4. "x" - คำนวณดอกเบี้ยเป็นเดือน เศษของเดือนนับวัน, หลายอัตราดอกเบี้ย
        //   5. "D" - คำนวณดอกเบี้ยเป็นรายวัน หลายอัตราดอกเบี้ย
        //   6. "d" - คำนวณดอกเบี้ยเป็นรายวัน อัตราดอกเบี้ยเดียว, อัตราดอกเบี้ย ณ. วันฝาก
        //   7. "w" - คำนวณดอกเบี้ยเป็นรายวัน อัตราดอกเบี้ยเดียว, อัตราดอกเบี้ย ณ. วันถอน
        // <parameter>
        /// <param name="InterestTable"></param>
        /// <param name="lastCalcDate"></param>
        /// <param name="balance"></param>
        /// <param name="depositType"></param>
        /// <param name="intType"></param>
        /// <param name="calcDate"></param>
        /// <param name="CalcIntType"></param>
        /// <param name="intBalance"></param>
        /// <param name="daysInYear"></param>
        /// <returns></returns>
        public static double DepositIntAmt(List<InterestModel> InterestTable, DateTime lastCalcDate, double balance,
        //public static double DepositIntAmt(DateTime lastCalcDate, double balance,
            string depositType, string intType, DateTime calcDate, string CalcIntType, double intBalance, int daysInYear)
        {
            double returnValue = 0;
            if (InterestTable == null || InterestTable.Count == 0)
            {
                return 0;
            }
            if (lastCalcDate >= calcDate || lastCalcDate.Equals(default(DateTime)) ||
                string.IsNullOrWhiteSpace(depositType) ||
                string.IsNullOrWhiteSpace(intType))
            {
                return 0;
            }

            double calcIntAmt = 0;
            double intRate = 0;
            long intDays = 0;
            int intMonths = 0;

            DateTime firstDate = lastCalcDate;
            DateTime endDate = calcDate;

            //List<InterestModel> lstInterest = _unitOfWork.Interest.ReadDetail().Where(p => p.Type == depositData.DepositTypeID &&
            //                                                                                p.TInt == depositData.IntType &&
            //                                                                                p.Filestatus == "A")
            //                                                                    .OrderBy(p => p.FirstEffectDate)
            //                                                                    .ToList();

            //List<InterestModel> InterestRec = _unitOfWork.Interest.ReadDetail().Where(p => p.Type == depositType &&
            //                                                          p.TInt == intType &&
            //                                                          p.Filestatus == "A")
            //                                               .OrderBy(p => p.FirstEffectDate).ToList();

            InterestModel interestData = InterestTable[0];
            int runningRecord = 0;
            if (InterestTable.Count > 0)
            {
                foreach (InterestModel interest in InterestTable)
                {
                    if (interest.LastEffectDate.HasValue == false)
                    {
                        interestData = interest;
                        break;
                    }
                    else
                    {
                        if (CalcIntType == "w" || CalcIntType == "m")
                        {
                            if (interest.LastEffectDate >= endDate)
                            {
                                interestData = interest;
                                break;
                            }
                        }
                        else
                        {
                            if (interest.LastEffectDate >= firstDate)
                            {
                                interestData = interest;
                                break;
                            }
                        }
                    }
                    runningRecord++;
                }
            }
            else
            {
                //Interaction.MsgBox("ข้อมูลดอกเบี้ย หรือ รหัสดอกเบี้ย หรือ วันที่คำนวณดอกเบี้ยไม่ถูกต้อง โปรดตรวจสอบข้อมูล");
                //string script = "alert(\"ข้อมูลดอกเบี้ย หรือ รหัสดอกเบี้ย หรือ วันที่คำนวณดอกเบี้ยไม่ถูกต้อง โปรดตรวจสอบข้อมูล\");";
                //ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
                //var Messg = "ข้อมูลดอกเบี้ย หรือ รหัสดอกเบี้ย หรือ วันที่คำนวณดอกเบี้ยไม่ถูกต้อง โปรดตรวจสอบข้อมูล";
                returnValue = 0;
                return returnValue;
            }

            switch (CalcIntType)
            {
                ///"M" - คำนวณดอกเบี้ยเป็นเดือน เศษของเดือนตัดทิ้ง, อัตราดอกเบี้ยเดียว  ณ. วันฝาก
                case "M":
                    ///1. "M" - คำนวณดอกเบี้ยเป็นเดือน เศษของเดือนตัดทิ้ง, อัตราดอกเบี้ยเดียว  ณ. วันฝาก
                    intRate = Deposit.DepositIntRate(interestData, intBalance);
                    endDate = calcDate;
                    intMonths = NoMonths(firstDate, endDate);
                    calcIntAmt = calcIntAmt + (balance * intRate * intMonths / 1200);
                    //goto Final_Loop;
                    break;
                case "m":
                    ///2. "m" - คำนวณดอกเบี้ยเป็นเดือน เศษของเดือนตัดทิ้ง, อัตราดอกเบี้ยเดียว  ณ. วันถอน
                    intRate = Deposit.DepositIntRate(interestData, intBalance);
                    endDate = calcDate;
                    intMonths = NoMonths(firstDate, endDate);
                    calcIntAmt = calcIntAmt + (balance * intRate * intMonths / 1200);
                    //goto Final_Loop;
                    break;
                case "X":
                    //'        '   3. "X" - คำนวณดอกเบี้ยเป็นเดือน หลายอัตราดอกเบี้ย
                    break;
                case "x":
                    //'        '   4. "x" - คำนวณดอกเบี้ยเป็นเดือน เศษของเดือนนับวัน, หลายอัตราดอกเบี้ย
                    break;
                case "D":
                    ///5. "D" - คำนวณดอกเบี้ยเป็นรายวัน หลายอัตราดอกเบี้ย
                    ///' ************ single rate ************
                    if (interestData.LastEffectDate.HasValue == false)
                    {
                        intRate = Deposit.DepositIntRate(interestData, intBalance);
                        endDate = calcDate;
                        intDays = NoDays(firstDate, endDate);
                        calcIntAmt = calcIntAmt + (balance * intRate * intDays / (daysInYear * 100));
                    }
                    else
                    {
                        ///' ************ multiple rate ************
                        ///' ************ first rate ************
                        intRate = DepositIntRate(interestData, intBalance); //first rate
                        if (interestData.LastEffectDate >= calcDate)
                        {
                            endDate = calcDate;
                            intDays = NoDays(firstDate, endDate);
                        }
                        else
                        {
                            endDate = interestData.LastEffectDate ?? default(DateTime);
                            intDays = NoDays(firstDate, endDate) + 1;
                        }
                        calcIntAmt = calcIntAmt + (balance * intRate * intDays / (daysInYear * 100));

                        runningRecord++; /// ' second or more rate --> rsTInterest.MoveNext() 
                        if (runningRecord == InterestTable.Count) //If rsTInterest.EOF Then
                        {
                            goto Final_Loop; //GoTo Final_Loop
                        }
                        else
                        {
                            interestData = InterestTable[runningRecord];
                            if (interestData.TInt != intType || interestData.Type != depositType || calcDate < interestData.FirstEffectDate)
                            {
                                goto Final_Loop; //GoTo Final_Loop
                            }
                            else
                            {
                                firstDate = interestData.FirstEffectDate;
                            }
                        }
                        ///************ second or more rate ************
                        do
                        {
                            intRate = Deposit.DepositIntRate(interestData, intBalance);
                            if (interestData.LastEffectDate.HasValue == false) /// only second rate
                            {
                                endDate = calcDate;
                                intDays = NoDays(firstDate, endDate);
                                calcIntAmt = calcIntAmt + (balance * intRate * intDays / (daysInYear * 100));
                            }
                            else ///************ third rate ************
                            {
                                if (interestData.LastEffectDate >= calcDate)
                                {
                                    endDate = calcDate;
                                    intDays = NoDays(firstDate, endDate);
                                }
                                else
                                {
                                    endDate = interestData.LastEffectDate ?? default(DateTime);
                                    intDays = NoDays(firstDate, endDate) + 1;
                                }
                                calcIntAmt = calcIntAmt + (balance * intRate * intDays / (daysInYear * 100));
                            }
                            runningRecord++;
                            if (runningRecord != InterestTable.Count)
                            {
                                interestData = InterestTable[runningRecord];
                                if (interestData.TInt != intType || interestData.Type != depositType || calcDate < interestData.FirstEffectDate)
                                {
                                    break;
                                }
                                else
                                {
                                    firstDate = interestData.FirstEffectDate;
                                }
                            }

                        } while (runningRecord != InterestTable.Count);
                    }
                    break;
                case "d":
                    ///6. "d" - คำนวณดอกเบี้ยเป็นรายวัน อัตราดอกเบี้ยเดียว, อัตราดอกเบี้ย ณ. วันฝาก                    
                    intRate = Deposit.DepositIntRate(interestData, intBalance);
                    endDate = calcDate;
                    intDays = NoDays(firstDate, endDate);
                    calcIntAmt = calcIntAmt + (balance * intRate * intDays / (daysInYear * 100));

                    break;
                case "w":
                    ///7. "w" - คำนวณดอกเบี้ยเป็นรายวัน อัตราดอกเบี้ยเดียว, อัตราดอกเบี้ย ณ. วันถอน
                    intRate = Deposit.DepositIntRate(interestData, intBalance);
                    endDate = calcDate;
                    intDays = NoDays(firstDate, endDate);
                    calcIntAmt = calcIntAmt + (balance * intRate * intDays / (daysInYear * 100));
                    break;
                default:
                    break;
            }

        Final_Loop:
            if (calcIntAmt > 0)
            {
                returnValue = (int)(calcIntAmt * 100) / 100.00;
            }
            else
            {
                returnValue = 0;
            }
            return returnValue;
        }

        private static long NoDays(DateTime firstDate, DateTime endDate)
        {
            //int x = DateAndTime.DateDiff(DateInterval.Day, 10 / 1 / 2003, 10 / 31 / 2003);

            //int Days = 0;
            //var timeDiff = endDate.Subtract(firstDate);
            //int hour = timeDiff.Hours;
            //Days = hour / 24;
            long Days = DateAndTime.DateDiff(DateInterval.Day, firstDate, endDate);
            return Days;
        }

        private static int NoMonths(DateTime firstDate, DateTime endDate)
        {
            int Months = 0;
            DateTime CurrentDate = firstDate;
            do
            {
                Months = Months + 1;
                CurrentDate.AddMonths(1);
            } while (CurrentDate < endDate);

            if (CurrentDate.AddDays(-1) >= endDate)
            {
                Months = Months - 1;
            }
            return Months;
        }

        private static double DepositIntRate(InterestModel interestData, double intBalance)
        {
            double returnValue = 0;
            if (intBalance <= (double)interestData.Balance1)
            {
                returnValue = (double)interestData.Rate1;
            }
            else if (intBalance <= (double)interestData.Balance2)
            {
                returnValue = (double)interestData.Rate2;
            }
            else if (intBalance <= (double)interestData.Balance3)
            {
                returnValue = (double)interestData.Rate3;
            }
            else if (intBalance <= (double)interestData.Balance4)
            {
                returnValue = (double)interestData.Rate4;
            }
            else
            {
                returnValue = (double)interestData.Rate5;
            }
            return returnValue;
        }

        public static string IssueAccountNo(DepositTypeModel rsDepositType, int intLen)
        {
            string IssueAccountNo = null;

            if (rsDepositType == null || string.IsNullOrEmpty(rsDepositType.DepositTypeID) || intLen == 0)
            {
                return IssueAccountNo;
            }
            //string dataIn = "";
            int intMaxDigit = 0;
            int intCtr;
            int intSummation = 0;
            int intRemainder = 0;
            var LastAccNo = rsDepositType.LastAccountNo + 1;

            string dataIn = Utility.FillZero((int)(rsDepositType.LastAccountNo + 1), intLen);
            intMaxDigit = Microsoft.VisualBasic.Strings.Len(dataIn);

            var intDigit = new int[intMaxDigit];

            for (intCtr = 0; intCtr <= (intMaxDigit - 1); intCtr++)
            {
                if ((intCtr % 2) == 0)
                {
                    intDigit[intCtr] = Convert.ToInt32(Microsoft.VisualBasic.Strings.Mid(dataIn, intCtr + 1, 1)) * 1;
                }
                else
                {
                    intDigit[intCtr] = Convert.ToInt32(Microsoft.VisualBasic.Strings.Mid(dataIn, intCtr + 1, 1)) * 2;
                }
            }
            for (intCtr = 0; intCtr <= (intMaxDigit - 1); intCtr++)
            {
                intSummation = intSummation + intDigit[intCtr];
            }

            intRemainder = intSummation % 10;
            IssueAccountNo = dataIn + intRemainder.ToString();

            return IssueAccountNo;
        }

        public static string IssueBookNo(DepositTypeModel rsDepositType, int intLen)
        {
            string IssueBookNo = null;

            if (rsDepositType == null
                || intLen == 0)
            {
                return IssueBookNo;
            }

            IssueBookNo = Utility.FillZero((int)(rsDepositType.LastBookNo + 1), intLen);

            return IssueBookNo;
        }

        //public static string DateMDY(DateTime date)
        //{
        //    var DateMDY = date.ToString("MM/dd/yyyy");
        //    return DateMDY;
        //}
        //public static string DateDMY(DateTime date)
        //{
        //    var DateDMY = date.ToString("dd/MM/yyyy");
        //    return DateDMY;
        //}
    }
}