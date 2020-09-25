using Microsoft.VisualBasic;
using Coop.Models.POCO;
using Coop.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaLPro.Library
{
    public class Core
    {
        /// <summary>
        /// คำนวณเงินที่ต้องจ่ายชำระ/(งวด|เดือน)
        /// </summary>
        /// <param name="strPrincipleMethod">รูปแบบการชำระ, PRNCIP.B: ชำระแบบธนาคาร(B), PRNCIP.C: ชำระแบบสหกรณ์(C)</param>
        /// <param name="dblInstallAmtChange"></param>
        /// <param name="dblLoanAmt">วงเงินกู้</param>
        /// <param name="dblIntRate">อัตราดอกเบี้ย</param>
        /// <param name="intInstallMonth">จำนวนงวด(เดือน)ที่คำนวณ</param>
        /// <returns>จำนวนเงินที่ต้องจ่ายชำระ/(งวด|เดือน)</returns>
        public static double dblInstallAmt(string strPrincipleMethod, double dblInstallAmtChange, double dblLoanAmt, double dblIntRate, int intInstallMonth)
        {
            double returnValue = 0;

            if (strPrincipleMethod == "B")
            {
                //dblInstallAmt = Rounded(System.Math.Abs(-Pmt((dblIntRate / 100) / 12, intInstallMonth, dblLoanAmt, 0, 1))) + 10
                //intInstallMonth = Microsoft.VisualBasic.Financial.NPer((dblIntRate / 100) / 12, -dblInstallAmt, dblLoanAmt, 0, Microsoft.VisualBasic.DueDate.EndOfPeriod) + 10
                var rate = dblIntRate / 100 / 12;
                var denominator = Math.Pow((1 + rate), intInstallMonth) - 1;
                return Rounded(rate + (rate / denominator) * dblLoanAmt);
            }
            else
            {
                returnValue = Rounded(dblLoanAmt / intInstallMonth);
            }
            if (dblInstallAmtChange > 0)
            {
                if ((int)(returnValue / dblInstallAmtChange) * dblInstallAmtChange != returnValue)
                {
                    returnValue = ((int)(returnValue / dblInstallAmtChange)) * dblInstallAmtChange + dblInstallAmtChange;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// จาก: Loan.vb :: dblLoanIntAmt
        /// '============================
        /// '   Input parameter
        /// ' *****************************************
        /// '   strTType              เพิ่มใหม่ 'LON' เพื่อนำมาอ่านตารางดอกเบี้ย
        /// '   strTInt               รหัสดอกเบี้ย เพื่อนำมาอ่านตารางดอกเบี้ย
        /// '   datStartDate          วันที่เริ่มคำนวณดอกเบี้ย
        /// '   datLastCalcDate       วันที่คำนวณดอกเบี้ยครั้งสุดท้าย
        /// '   dblBalance            ยอดเงินที่จะคำนวณดอกเบี้ย
        /// '   datCalcDate           วันที่จะคำนวณดอกเบี้ยถึง
        /// '   strIntCalcMethod      วิธีการคำนวณดอกเบี้ย
        /// '                           'D' single rate คำนวณดอกเบี้ยรายวัน และอัตราดอกเบี้ยจากสัญญาคือ dblInterestRate
        /// '                           'M' single rate คำนวณดอกเบี้ยรายเดือน และอัตราดอกเบี้ยจากสัญญาคือ dblInterestRate
        /// '                           'd' multiple rate คำนวณดอกเบี้ยรายวัน และอัตราดอกเบี้ยจากตารางดอกเบี้ย
        /// '   dblInterestRate       อัตราดอกเบี้ยของสัญญาเงินกู้
        /// '   intRoundIntMethod     การปัดเศษของดอกเบี้ย 0 - ไม่ป้ดเศษ, 25-ป้ดเศษ 25 สตางค์, 50-ป้ดเศษ 50 สตางค์, 100-ป้ดเศษ 100 สตางค์
        /// </summary>
        public static double DblLoanIntAmt(List<InterestModel> lstTInterest, string strTType, string strTInt, double dblBalance, DateTime datStartDate, DateTime datLastCalcDate, DateTime datCalcDate, string strIntCalcMethod, double dblInterestRate, int intRoundIntMethod, int daysInYear, bool bolRound, bool bolAdjust)
        {
            double returnValue = 0;

            /// Validate Parameters Section
            if (lstTInterest == null || lstTInterest.Count == 0)
            {
                return 0;
            }
            if (datStartDate > datCalcDate || datLastCalcDate >= datCalcDate)
            {
                return 0;
            }

            //Dim rsTInterest As New ADODB.Recordset

            //Dim SQLStmt As String
            System.DateTime datFirstDate = default(DateTime); //Dim datFirstDate As Date
            System.DateTime datEndDate = default(DateTime); //Dim datEndDate As Date
            double dblCalcIntAmt = 0; //Dim dblCalcIntAmt As Double = 0
            double dblRate = 0;  //Dim dblRate As Double = 0
            long Intdays = 0; //Dim Intdays As Long = 0
            double IntMonths = 0; //Dim IntMonths As Double = 0

            // ไม่ได้ใช้
            //long intRemainder = 0; //Dim intRemainder As Long

            // move to --> Validate Parameters Section
            //If datStartDate > datCalcDate Or datLastCalcDate >= datCalcDate Then
            //    Exit Function
            //End If          

            dblCalcIntAmt = 0; //dblCalcIntAmt = 0
            datFirstDate = datLastCalcDate; //datFirstDate = datLastCalcDate
            datEndDate = datCalcDate; //datEndDate = datCalcDate

            
            List<InterestModel> rsTInterest = lstTInterest;

            //List<InterestModel> rsTInterest = lstTInterest.Where(p => p.t_type == strTType &&
            //                                                           p.t_int == strTInt &&
            //                                                           p.filestatus == FileStatus.A)
            //                                               .OrderBy(p => p.first_effect_date).ToList();

            //balIntRate = _unitOfWork.Interest.GetInterestRate(CurrentAuth.CoopSystem().coop_id, loanType, loanTypeInt);

            // รหัสดอกเบี้ย หรือ วันที่คำนวณดอกเบี้ยไม่ถูกต้อง 
            // If either the BOF or EOF property is True, there is no current record. 
            // see also: https://msdn.microsoft.com/en-us/library/office/ff821459.aspx
            // if there is no record
            if (!rsTInterest.Any() || strTInt == "0") //If rsTInterest.EOF Or strTInt = "0" Then
            {
                //'MsgBox "รหัสดอกเบี้ย หรือ วันที่คำนวณดอกเบี้ยไม่ถูกต้อง โปรดตรวจสอบข้อมูล"
                //'dblRate = dblInterestRate
                //'dblCalcIntAmt = dblBalance * dblInterestRate * Intdays / (gsDaysInYear * 100)

                switch (strIntCalcMethod) //Select Case strIntCalcMethod
                {
                    // 'M' single rate คำนวณดอกเบี้ยรายเดือน และอัตราดอกเบี้ยจากสัญญาคือ dblInterestRate
                    case "M": // Case "M"
                        IntMonths = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Month, datFirstDate, datEndDate) - 1; //IntMonths = DateDiff(DateInterval.Month, datFirstDate, datEndDate) - 1
                        //'If Day(datFirstDate) > 15 Then
                        //'If Val(Format(datFirstDate, "DD")) > 15 Then
                        if (datFirstDate.Day > 15) //If datFirstDate.Day > 15 Then
                        {
                            IntMonths = IntMonths + 0.5; //IntMonths = IntMonths + 0.5
                        }
                        else //Else
                        {
                            IntMonths = IntMonths + 1; //IntMonths = IntMonths + 1
                        } //End If
                        dblCalcIntAmt = dblBalance * dblInterestRate * IntMonths / 1200; //dblCalcIntAmt = dblBalance * dblInterestRate * IntMonths / 1200

                        break;
                    default: // Case Else   ' 'D' single rate คำนวณดอกเบี้ยรายวัน และอัตราดอกเบี้ยจากสัญญาคือ dblInterestRate
                        dblRate = DblLoanIntRate(rsTInterest.FirstOrDefault(), dblBalance); //dblRate = dblLoanIntRate(rsTInterest, dblBalance) 'Edit by Ton! 11/Aug/08
                        Intdays = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate) + 1 ; //Intdays = DateDiff(DateInterval.Day, datFirstDate, datEndDate)
                        dblCalcIntAmt = dblBalance * dblInterestRate * Intdays / (daysInYear * 100); //dblCalcIntAmt = dblBalance * dblInterestRate * Intdays / (gsDaysInYear * 100)
                        break;
                } //End Select
            }
            else //Else --> at least one record
            {
                //Do Until rsTInterest.EOF
                //    If IsDBNull(rsTInterest.Fields("last_effect_date").Value) Then
                //        'If IsDBNull(rsTInterest.Fields("last_effect_date").Value) Then
                //        Exit Do
                //    Else
                //        If rsTInterest.Fields("last_effect_date").Value >= datFirstDate Then
                //            Exit Do
                //        End If
                //        rsTInterest.MoveNext()
                //    End If
                //Loop

                // Find t_Interest which 'last_effect_date' == null       
         
                 bool isSingleRate = false; 
                 var tMultiRate = new List<InterestModel>();
                InterestModel tSingleRate = rsTInterest.FirstOrDefault(p => p.last_effect_date == null);
                // Check for if Single Rate                
                if (tSingleRate == null)
                {
                    // Null of 'last_effect_date' Record was not found
                    tMultiRate = rsTInterest.Where(p => p.last_effect_date.Value >= datFirstDate)
                                   .OrderBy(p => p.first_effect_date)
                                   .ToList();

                }
                else // Single Rate
                {
                    isSingleRate = true;
                }

                /// Single Rate --> last_effect_date == null
                if (isSingleRate) //If IsDBNull(rsTInterest.Fields("last_effect_date").Value) Then          ' single rate
                {
                    dblRate = DblLoanIntRate(tSingleRate, dblBalance); //dblRate = dblLoanIntRate(rsTInterest, dblBalance)
                    datEndDate = datCalcDate; //datEndDate = datCalcDate
                    Intdays = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate) + 1; //Intdays = DateDiff(DateInterval.Day, datFirstDate, datEndDate)
                    dblCalcIntAmt = dblBalance * dblRate * Intdays / (daysInYear * 100); //dblCalcIntAmt = dblBalance * dblRate * Intdays / (gsDaysInYear * 100)
                }
                /// Case: "last_effect_date" >= datFirstDate
                else //Else         ' multiple rate
                {
                    //Do Until rsTInterest.EOF
                    dblRate = DblLoanIntRate(tMultiRate.FirstOrDefault(), dblBalance); //    dblRate = dblLoanIntRate(rsTInterest, dblBalance)

                    //    ' Edit by Diaw 18NOV2008
                    if (tMultiRate.Where(p => p.last_effect_date.Value.Equals(DBNull.Value)).Count() > 0) // If IsDBNull(rsTInterest.Fields("last_effect_date").Value) Then                         
                    {
                        datEndDate = datCalcDate; // datEndDate = datCalcDate
                        Intdays = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate) + 1; // Intdays = DateDiff(DateInterval.Day, datFirstDate, datEndDate)
                        dblCalcIntAmt = dblCalcIntAmt + (dblBalance * dblRate * Intdays / (daysInYear * 100)); // dblCalcIntAmt = dblCalcIntAmt + (dblBalance * dblRate * Intdays / (gsDaysInYear * 100))
                        // Exit Do    
                    }
                    else if (tMultiRate.Where(p => p.last_effect_date.Value >= datCalcDate).Count() > 0) // ElseIf rsTInterest.Fields("last_effect_date").Value >= datCalcDate Then
                    {
                        datEndDate = datCalcDate; // datEndDate = datCalcDate
                        Intdays = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate) + 1 ; // Intdays = DateDiff(DateInterval.Day, datFirstDate, datEndDate)
                        dblCalcIntAmt = dblCalcIntAmt + (dblBalance * dblRate * Intdays / (daysInYear * 100)); // dblCalcIntAmt = dblCalcIntAmt + (dblBalance * dblRate * Intdays / (gsDaysInYear * 100))
                        // Exit Do
                    }
                    else // Else
                    {
                        datEndDate = tMultiRate.FirstOrDefault().last_effect_date.GetValueOrDefault(); // datEndDate = rsTInterest.Fields("last_effect_date").Value
                        Intdays = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate) + 1; // Intdays = DateDiff(DateInterval.Day, datFirstDate, datEndDate) + 1
                        dblCalcIntAmt = dblCalcIntAmt + (dblBalance * dblRate * Intdays / (daysInYear * 100)); // dblCalcIntAmt = dblCalcIntAmt + (dblBalance * dblRate * Intdays / (gsDaysInYear * 100))

                    } // End If

                    // ' End Edit
                    datFirstDate = tMultiRate.FirstOrDefault().last_effect_date.GetValueOrDefault().AddDays(1); // datFirstDate = DateAdd(DateInterval.Day, 1, rsTInterest.Fields("last_effect_date").Value)
                    // rsTInterest.MoveNext()
                    //Loop
                }//End If
            }//End If

            if (bolRound) //If bolRound Then
            {
                dblCalcIntAmt = Rounded(dblCalcIntAmt); // dblCalcIntAmt = Rounded(dblCalcIntAmt)

            } //End If

            if (bolAdjust) //If bolAdjust Then
            {
                dblCalcIntAmt = DblLoanIntAmtRounded(dblCalcIntAmt, intRoundIntMethod); //dblCalcIntAmt = dblLoanIntAmtRounded(dblCalcIntAmt, intRoundIntMethod)
            } //End If

            returnValue = dblCalcIntAmt; //dblLoanIntAmt = dblCalcIntAmt

            //Exit Function
            //Error_dblLoanIntAmt:
            //MsgErr("Loan", Err.Number, Err.Description, gsWarning)
            //Resume Next
            return returnValue;
        }

        /// <summary>
        /// คำนวณอัตราดอกเบี้ยเงินกู้
        /// จาก: Loan.vb :: dblLoanIntRate
        /// </summary>
        /// <param name="interestRate">อัตราดอกเบี้ยของสัญญาเงินกู้ที่จะนำมาคำนวณ</param>
        /// <param name="dblBalance">ยอดเงินที่จะคำนวณดอกเบี้ย</param>
        /// <returns>ตัวเลข: อัตราดอกเบี้ยเงินกู้</returns>
        public static double DblLoanIntRate(IInteresteRate interestRate, double dblBalance)
        {
            if (interestRate.balance_1   != null && dblBalance <= (double)(interestRate.balance_1 ?? 0))
            {
                if (interestRate.rate_1 != null) return (double)interestRate.rate_1;
            }
            else if (interestRate.balance_2 != null && dblBalance <= (double) (interestRate.balance_2 ?? 0))
            {
                if (interestRate.rate_2 != null) return (double)interestRate.rate_2;
            }
            else if (interestRate.balance_3 != null && dblBalance <= (double)(interestRate.balance_3 ?? 0))
            {
                if (interestRate.rate_3 != null) return (double)interestRate.rate_3;
            }
            else if (interestRate.balance_4 != null && dblBalance <= (double)(interestRate.balance_4 ?? 0))
            {
                if (interestRate.rate_4 != null) return (double)interestRate.rate_4;
            }
            else
            {
                if (interestRate.rate_5 != null) return (double)interestRate.rate_5;
            }

            return 0;
        }

        /// <summary>
        /// จาก: SysApp32.vb
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <summary>
        /// จาก: Loan.vb
        /// การปัดเศษของดอกเบี้ย 0 - ไม่ป้ดเศษ, 25-ป้ดเศษ 25 สตางค์, 50-ป้ดเศษ 50 สตางค์, 100-ป้ดเศษ 100 สตางค์
        /// </summary>
        /// <param name="dblLoanInt"></param>
        /// <param name="intRoundIntMethod"></param>
        /// <returns></returns>
        public static double DblLoanIntAmtRounded(double dblLoanInt, int intRoundIntMethod)
        {
            double functionReturnValue = 0;
            double intRemainder = 0; //Dim intRemainder As Long = 0
            double loanIntValue = 0;
            functionReturnValue = Rounded(dblLoanInt); //dblLoanIntAmtRounded = Rounded(dblLoanInt)
            loanIntValue = Rounded(dblLoanInt); //dblLoanInt = Rounded(dblLoanInt)
            if (intRoundIntMethod > 0) //If intRoundIntMethod > 0 Then
            {
                if ((int)((loanIntValue * 100 - (int)(loanIntValue) * 100 + 0.01)) > 0) //If Int((dblLoanInt * 100 - Int(dblLoanInt) * 100 + 0.01)) > 0 Then
                {
                    intRemainder = Rounded((double)((int)((loanIntValue * 100 - (int)(dblLoanInt) * 100) % intRoundIntMethod))); //intRemainder = Rounded(Int((dblLoanInt * 100 - Int(dblLoanInt) * 100) Mod intRoundIntMethod))
                    if (intRemainder > 0) //If intRemainder > 0 Then
                    {
                        intRemainder = Rounded((double)(((int)((loanIntValue * 100 - (int)(loanIntValue) * 100)) / intRoundIntMethod + 1) * intRoundIntMethod)); //intRemainder = Rounded((Int((dblLoanInt * 100 - Int(dblLoanInt) * 100)) \ intRoundIntMethod + 1) * intRoundIntMethod)
                        loanIntValue = (int)(loanIntValue) + intRemainder / 100.00; //dblLoanInt = Int(dblLoanInt) + intRemainder / 100
                    }
                    else //Else
                    {
                        loanIntValue = (int)(loanIntValue) + (int)((loanIntValue * 100 - (int)(loanIntValue) * 100)) / 100.00; //dblLoanInt = Int(dblLoanInt) + Int((dblLoanInt * 100 - Int(dblLoanInt) * 100)) / 100
                    } //End If
                } //End If
            }
            else //Else
            {
                if ((int)((loanIntValue * 100 - (int)(loanIntValue) * 100)) > System.Math.Abs(intRoundIntMethod)) //If Int((dblLoanInt * 100 - Int(dblLoanInt) * 100)) > System.Math.Abs(intRoundIntMethod) Then
                {
                    loanIntValue = (int)loanIntValue + 1; //dblLoanInt = Int(dblLoanInt) + 1
                }
                else //Else
                {
                    loanIntValue = (int)loanIntValue; //dblLoanInt = Int(dblLoanInt)
                } //End If
            }
            functionReturnValue = loanIntValue; //dblLoanIntAmtRounded = dblLoanInt
            return functionReturnValue;
        }

        public static double RoundSubstract(double dblAmt, long intRoundAmt)
        {
            double functionReturnValue = 0;
            double dblIntAmt = 0;
            dblAmt = Core.Rounded(dblAmt);

            if (intRoundAmt > 0)
            {
                if ((dblAmt * 100 - Microsoft.VisualBasic.Conversion.Int(dblAmt) * 100.00) > 0)
                {
                    double intRemainder = ((dblAmt * 100.00) - Microsoft.VisualBasic.Conversion.Int(dblAmt) * 100.00) % intRoundAmt;
                    dblIntAmt = dblAmt - (intRemainder / 100.00);
                }
                else
                {
                    dblIntAmt = dblAmt;
                }
            }
            else
            {
                dblIntAmt = dblAmt;
            }

            functionReturnValue = dblIntAmt;
            return functionReturnValue;
        }

        /// <summary>
        /// จาก: Loan.vb :: dblLoanPrinciple
        /// 
        ///    '============================
        ///    '   Input parameter
        ///    '============================
        ///    '   strLoan               เพิ่มใหม่
        ///    '   datStartDate
        ///    '   datLastdatCalcDate
        ///    '   dblBalance
        ///    '   intInterestRate             => strTInt
        ///    '   datCalcDate
        ///    '   strIntCalcMethod <=-- where it used
        /// </summary>
        //public static double DblLoanPrinciple(string strLoan, System.DateTime datStartDate, System.DateTime datLastdatCalcDate, double dblBalance, string strTInt, System.DateTime datCalcDate, string strIntCalcMethod)
        public static double DblLoanPrinciple(List<InterestModel> lstTInterest, string strTType, string strTInt, double dblBalance,
                                       DateTime datStartDate, DateTime datLastdatCalcDate, DateTime datCalcDate, DateTime thisMthProcDate)
        {
            double returnValue = 0;

            if (lstTInterest == null || lstTInterest.Count == 0)
            {
                return returnValue;
            }

            /// Validate date to be calculate interest
            if (datStartDate > datCalcDate || datLastdatCalcDate >= datCalcDate) //If datStartDate > datCalcDate Or datLastdatCalcDate >= datCalcDate Then
            {
                return returnValue; //Exit Function
            } //End If

            //Dim rsCoop As ADODB.Recordset
            //Dim rsTInterest As ADODB.Recordset

            double dblCalcIntAmt = 0; //Dim dblCalcIntAmt As Double = 0
            double dblPrinciple = 0; //Dim dblPrinciple As Double = 0
            double intInterestRate = 0; //Dim intInterestRate As Double = 0
            double intInterestBal = 0; //Dim intInterestBal As Double            
            double intIntMonths = 0; //Dim intIntMonths As Double = 0
            double dblAccIntDay = 0; //Dim dblAccIntDay As Double
            long intIntDay = 0; //Dim intIntDay As Long = 0
            DateTime datFirstDate = datLastdatCalcDate; //Dim datFirstDate As Date
            DateTime datEndDate = datCalcDate; //Dim datEndDate As Date

            //SQLStmt = "select * from coop"
            //rsCoop = DataSet(SQLStmt)

            //'SQLStmt = "select * from loan_interest"
            //SQLStmt = "select * from t_interest" 'Edit by Ton! 27/Aug/08
            //SQLStmt = SQLStmt & " where t_int = '" & strTInt & "'"
            //'SQLStmt = SQLStmt & " and t_loan = '" & strLoan & "'"
            //SQLStmt = SQLStmt & " and t_type = '" & strLoan & "'" 'Edit by Ton! 27/Aug/08
            //SQLStmt = SQLStmt & " and filestatus = 'A'"
            //SQLStmt = SQLStmt & " order by first_effect_date;"
            //rsTInterest = DataSet(SQLStmt)
            List<InterestModel> rsTInterest = lstTInterest.Where(p => p.t_type == strTType &&
                                                                       p.t_int == strTInt &&
                                                                       p.filestatus == FileStatus.A)
                                                           .OrderBy(p => p.first_effect_date).ToList();
            bool isSingleRate = false;
            InterestModel tCurrentRate = rsTInterest[0];
            int runningRecord = 0;
            if (rsTInterest != null || rsTInterest.Count > 0) //If Not rsTInterest.EOF Then
            {
                //rsTInterest.MoveFirst()
                foreach (InterestModel item in rsTInterest) //Do Until rsTInterest.EOF
                {
                    if (item.last_effect_date.HasValue == false) //If IsDBNull(rsTInterest.Fields("last_effect_date").Value) Then
                    {
                        isSingleRate = true;
                        tCurrentRate = item;
                        break; //Exit Do
                    }
                    else //Else
                    {
                        if (item.last_effect_date >= datFirstDate) //If rsTInterest.Fields("last_effect_date").Value >= datFirstDate Then
                        {
                            isSingleRate = false;
                            tCurrentRate = item;
                            break;  //Exit Do
                        } //End If
                        //rsTInterest.MoveNext()
                    } //End If
                    runningRecord++;
                }//Loop
            }
            else //Else
            {
                //MsgBox("รหัสดอกเบี้ย หรือ วันที่คำนวณดอกเบี้ยไม่ถูกต้อง โปรดตรวจสอบข้อมูล")
                //Exit Function
            } //End If

            /// Single Rate
            if (isSingleRate) //If IsDBNull(rsTInterest.Fields("last_effect_date").Value) Then                        ' single rate
            {
                intInterestRate = DblLoanIntRate(tCurrentRate, intInterestBal); //intInterestRate = dblLoanIntRate(rsTInterest, intInterestBal)
                datEndDate = (DateTime)(datCalcDate); //datEndDate = CDate(datCalcDate)
                if (thisMthProcDate >= datLastdatCalcDate) //If rsCoop.Fields("this_mth_proc_date").Value >= datLastdatCalcDate Then 'Edit by Ton! 27/Aug/08
                {
                    intIntDay = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate); //intIntDay = DateDiff(DateInterval.Day, datFirstDate, datEndDate)
                }
                else //Else
                {
                    intIntDay = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate); //intIntDay = DateDiff(DateInterval.Day, datFirstDate, datEndDate)
                } //End If
            }
            /// Multi-Rate
            else //Else                                                        ' multiple rate
            {
                /// First Rate
                intInterestRate = DblLoanIntRate(tCurrentRate, intInterestBal); //intInterestRate = dblLoanIntRate(rsTInterest, intInterestBal)                          ' first rate
                if (tCurrentRate.last_effect_date >= (DateTime)(datCalcDate)) //If rsTInterest.Fields("last_effect_date").Value >= CDate(datCalcDate) Then
                {
                    datEndDate = (DateTime)(datCalcDate); //datEndDate = CDate(datCalcDate)
                    intIntDay = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate); //intIntDay = DateDiff(DateInterval.Day, datFirstDate, datEndDate)
                }
                else //Else
                {
                    datEndDate = tCurrentRate.last_effect_date ?? default(DateTime); //datEndDate = rsTInterest.Fields("last_effect_date").Value
                    intIntDay = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate) + 1; //intIntDay = DateDiff(DateInterval.Day, datFirstDate, datEndDate) + 1
                } //End If
            } //End If

            dblAccIntDay = dblAccIntDay + (intInterestRate * intIntDay); //dblAccIntDay = dblAccIntDay + (intInterestRate * intIntDay)

            /// Multi-Rate::Second Rate && more rate...
            //rsTInterest.MoveNext()                                              ' second or more rate
            runningRecord++;
            if (runningRecord == rsTInterest.Count) //If rsTInterest.EOF Then
            {
                goto Final_Loop; //GoTo Final_Loop
            }
            else //Else
            {
                tCurrentRate = rsTInterest[runningRecord];
                if (datCalcDate < tCurrentRate.first_effect_date) //If datCalcDate < rsTInterest.Fields("first_effect_date").Value Then
                {
                    goto Final_Loop; //GoTo Final_Loop   
                }
                else //Else
                {
                    datFirstDate = tCurrentRate.first_effect_date; //datFirstDate = rsTInterest.Fields("first_effect_date").Value
                } //End If
            } //End If

            /// Multi-Rate::Second Rate Loop
            do //Do                                                         ' second rate loop
            {
                intInterestRate = DblLoanIntRate(tCurrentRate, intInterestBal); //intInterestRate = dblLoanIntRate(rsTInterest, intInterestBal)
                if (tCurrentRate.last_effect_date.HasValue == false) //If IsDBNull(rsTInterest.Fields("last_effect_date").Value) Then                  ' only second rate
                {
                    datEndDate = datCalcDate; //datEndDate = CDate(datCalcDate)
                    intIntDay = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate); //intIntDay = DateDiff(DateInterval.Day, datFirstDate, datEndDate)
                    dblAccIntDay = dblAccIntDay + (intInterestRate * intIntDay); //dblAccIntDay = dblAccIntDay + (intInterestRate * intIntDay)
                }
                else //Else                                                    ' third rate
                {
                    if (tCurrentRate.last_effect_date >= datCalcDate) //If rsTInterest.Fields("last_effect_date").Value >= CDate(datCalcDate) Then
                    {
                        datEndDate = datCalcDate; //datEndDate = CDate(datCalcDate)
                        intIntDay = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate); //intIntDay = DateDiff(DateInterval.Day, datFirstDate, datEndDate)
                    }
                    else //Else
                    {
                        datEndDate = tCurrentRate.last_effect_date ?? default(DateTime); //datEndDate = rsTInterest.Fields("last_effect_date").Value
                        //'intIntDay = DateDiff(DateInterval.Day, datFirstDate, datEndDate) + 1 ไม่นับวันชำระวันก่อน นับวันที่รับชำระวันนี้  4. ???????????????
                        intIntDay = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate) + 1; //intIntDay = DateDiff(DateInterval.Day, datFirstDate, datEndDate) + 1
                    } //End If

                    //'dblCalcIntAmt = dblCalcIntAmt + (dblBalance * intInterestRate * intIntDay / (gsDaysInYear * 100))
                    //'dblPrinciple = dblPrinciple + (dblBalance * (gsDaysInYear * 100) / (gsDaysInYear * 100) + (intInterestRate * intIntDay))
                    dblAccIntDay = dblAccIntDay + (intInterestRate * intIntDay); //dblAccIntDay = dblAccIntDay + (intInterestRate * intIntDay)
                }//End If


                runningRecord++; //rsTInterest.MoveNext()                
                if (runningRecord <= rsTInterest.Count) //If Not rsTInterest.EOF Then
                {
                    tCurrentRate = rsTInterest[runningRecord];
                    //'If rsTInterest("t_int") <> strTInt Or rsTInterest("t_loan") <> strLoan Or CDate(datCalcDate) < rsTInterest("first_effect_date") Then    
                    if (datCalcDate < tCurrentRate.first_effect_date) //If datCalcDate < rsTInterest.Fields("first_effect_date").Value Then
                    {
                        //Exit Do
                        break;
                    }
                    else //Else
                    {
                        datFirstDate = tCurrentRate.first_effect_date; //datFirstDate = rsTInterest.Fields("first_effect_date").Value
                    }//End If
                } //End If
            } while (runningRecord != rsTInterest.Count); //Loop Until rsTInterest.EOF

            Final_Loop:
            //'dblPrinciple = dblPrinciple + (dblBalance * (gsDaysInYear * 100) / (gsDaysInYear * 100) + (intInterestRate * intIntDay))
            //'dblCalcIntAmt = Rounded(dblPrinciple)
            returnValue = dblAccIntDay; //dblLoanPrinciple = dblAccIntDay

            //    Exit Function
            //Error_dblLoanPrinciple:

            //    MsgErr("Loan", Err.Number, Err.Description, gsWarning)
            //    Resume Next

            return returnValue;
        } //End Function

        //อันเก่า 
        public static int IntInstallMonth(List<t_loan_installModels> listTloan, string strTLoan, string strPrincipleMethod, double dblLoanAmt, double dblIntRate, double dblInstallAmt)
        {
            int intInstallMonth = 0;
            try
            {
                if (strPrincipleMethod == "B")
                {
                    //intInstallMonth = Microsoft.VisualBasic.Financial.NPer((dblIntRate / 100) / 12, -dblInstallAmt, dblLoanAmt, 0, Microsoft.VisualBasic.DueDate.EndOfPeriod) + 10
                    /// +10 ทำไมอ่ะ ... งง
                    intInstallMonth = (int)Microsoft.VisualBasic.Financial.NPer((dblIntRate / 100.00) / 12.00, -dblInstallAmt, dblLoanAmt, 0, DueDate.EndOfPeriod) + 10;

                    return intInstallMonth;
                }
                else
                {
                    if (listTloan.Count > 0)
                    {
                        foreach (t_loan_installModels loanInstall in listTloan)
                        {
                            if (dblLoanAmt <= Convert.ToDouble(loanInstall.balance))
                            {
                                intInstallMonth = loanInstall.install_month;
                                if (dblInstallAmt > 0)
                                {
                                    int intInstall = 0;
                                    if (Convert.ToInt32(dblLoanAmt / dblInstallAmt) == (dblLoanAmt / dblInstallAmt))
                                    {
                                        intInstall = (int)(dblLoanAmt / dblInstallAmt);
                                    }
                                    else
                                    {
                                        intInstall = (int)(dblLoanAmt / dblInstallAmt) + 1;
                                    }
                                    if (intInstall < intInstallMonth)
                                    {
                                        intInstallMonth = intInstall;
                                        return intInstallMonth;
                                    }
                                    else
                                    {
                                        return intInstallMonth;
                                    }
                                }
                                else
                                {
                                    return intInstallMonth;
                                }
                            }
                            else
                            {
                                return intInstallMonth;
                            }
                        }
                        return intInstallMonth;
                    }
                    else
                    {
                        if (dblInstallAmt > 0)
                        {
                            intInstallMonth = Convert.ToInt32(dblLoanAmt / dblInstallAmt);
                            if ((intInstallMonth * dblInstallAmt) < dblLoanAmt)
                            {
                                intInstallMonth = Convert.ToInt32(intInstallMonth) + 1;
                                return intInstallMonth;
                            }
                            else
                            {
                                intInstallMonth = Convert.ToInt32(intInstallMonth);
                                return intInstallMonth;
                            }
                        }
                        else
                        {
                            return intInstallMonth;
                        }
                    }
                }
            }
            catch
            {
                return intInstallMonth;
            }
        }

        public static int IntInstallMonth(List<t_loan_amtModels> listTloan, string strTLoan, string strPrincipleMethod, double dblLoanAmt, double dblIntRate, double dblInstallAmt, double dblInstallbal)
        {
            int intInstallMonth = 0;
            try
            {
                if (strPrincipleMethod == "B")
                {
                    //intInstallMonth = Microsoft.VisualBasic.Financial.NPer((dblIntRate / 100) / 12, -dblInstallAmt, dblLoanAmt, 0, Microsoft.VisualBasic.DueDate.EndOfPeriod) + 10
                    /// +10 ทำไมอ่ะ ... งง
                    intInstallMonth = (int)Microsoft.VisualBasic.Financial.NPer((dblIntRate / 100.00) / 12.00, -dblInstallAmt, dblLoanAmt, 0, DueDate.EndOfPeriod) + 10;

                    return intInstallMonth;
                }
                else
                {
                    if (listTloan.Count > 0)
                    {
                        foreach (t_loan_amtModels loanInstall in listTloan)
                        {
                            if (dblLoanAmt <= Convert.ToDouble(dblInstallbal))
                            {
                                intInstallMonth = loanInstall.install_month;
                                if (dblInstallAmt > 0)
                                {
                                    int intInstall = 0;
                                    if (Convert.ToInt32(dblLoanAmt / dblInstallAmt) == (dblLoanAmt / dblInstallAmt))
                                    {
                                        intInstall = (int)(dblLoanAmt / dblInstallAmt);
                                    }
                                    else
                                    {
                                        intInstall = (int)(dblLoanAmt / dblInstallAmt) + 1;
                                    }
                                    if (intInstall < intInstallMonth)
                                    {
                                        intInstallMonth = intInstall;
                                        return intInstallMonth;
                                    }
                                    else
                                    {
                                        return intInstallMonth;
                                    }
                                }
                                else
                                {
                                    return intInstallMonth;
                                }
                            }
                            else
                            {
                                return intInstallMonth;
                            }
                        }
                        return intInstallMonth;
                    }
                    else
                    {
                        if (dblInstallAmt > 0)
                        {
                            intInstallMonth = Convert.ToInt32(dblLoanAmt / dblInstallAmt);
                            if ((intInstallMonth * dblInstallAmt) < dblLoanAmt)
                            {
                                intInstallMonth = Convert.ToInt32(intInstallMonth) + 1;
                                return intInstallMonth;
                            }
                            else
                            {
                                intInstallMonth = Convert.ToInt32(intInstallMonth);
                                return intInstallMonth;
                            }
                        }
                        else
                        {
                            return intInstallMonth;
                        }
                    }
                }
            }
            catch
            {
                return intInstallMonth;
            }
        }
    }
}
