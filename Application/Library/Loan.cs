using System;
using System.Globalization;
using System.Linq;
using SaLPro.Entities;
using System.Collections.Generic;
using SaLPro.Models.POCO;
using SaLPro.Infrastructure.Helpers;

namespace SaLPro.Library
{
    public class Loan
    {
        public double dblInstallAmt(string strPrincipleMethod, double dblInstallAmtChange, double dblLoanAmt, double dblIntRate, int intInstallMonth)
        {
            double functionReturnValue = 0;
            if (strPrincipleMethod == "B")
            {
                //functionReturnValue =Math.Round((Math.Abs(-Pmt((dblIntRate / 100) / 12, intInstallMonth, dblLoanAmt, 0, 1))) + 10;
            }
            else
            {
                functionReturnValue = Math.Round(dblLoanAmt / intInstallMonth);
            }
            if (dblInstallAmtChange > 0)
            {
                if (double.Parse((functionReturnValue / dblInstallAmtChange).ToString()) * dblInstallAmtChange != functionReturnValue)
                {
                    functionReturnValue = (double.Parse((functionReturnValue / dblInstallAmtChange).ToString())) * dblInstallAmtChange + dblInstallAmtChange;
                }
            }
            return functionReturnValue;
        }

        public double dblLoanBal(object strMemberId, string coop_id)
        {
            var s = (from c in _db.loans
                     where c.coop_id == coop_id && c.member_id == strMemberId && c.filestatus == "A"
                     group c by c.member_id into d
                     select new
                     {
                         sum_loan_bal = d.Sum(x => x.loan_bal)
                     });
            return 0;
        }

        //private double Rounded(double Amount)
        //{
        //    double returnValue = 0;
        //    double Amt = 0, AmtReal = 0;//Dim Amt As Double, AmtReal As Double

        //    Amt = (int)(Amount * 1000 + 5); //Amt = Int(Amount * 1000 + 5)
        //    //''Amt = Int(Amount * 1000 + 6) 'Comment Out by Ton! 18/May/09
        //    AmtReal = (int)(Amt / 10); //AmtReal = Int(Amt / 10)
        //    AmtReal = AmtReal / 100; //AmtReal = AmtReal / 100
        //    returnValue = AmtReal; //Rounded = AmtReal

        //    return returnValue;
        //}
        //public double dblLoanIntAmt(string strTType, string strTInt, double dblBalance, DateTime datStartDate, System.DateTime datLastCalcDate, DateTime datCalcDate, string strIntCalcMethod, double dblInterestRate, long intRoundIntMethod, bool bolRound,
        //    bool bolAdjust, string coop_id, int gsDaysInYear)
        //{
        //    double functionReturnValue = 0;
        //    // ERROR: Not supported in C#: OnErrorStatement

        //    //============================
        //    //   Input parameter
        //    // *****************************************
        //    //   strTType              เพิ่มใหม่ 'LON' เพื่อนำมาอ่านตารางดอกเบี้ย
        //    //   strTInt               รหัสดอกเบี้ย เพื่อนำมาอ่านตารางดอกเบี้ย
        //    //   datStartDate          วันที่เริ่มคำนวณดอกเบี้ย
        //    //   datLastCalcDate       วันที่คำนวณดอกเบี้ยครั้งสุดท้าย
        //    //   dblBalance            ยอดเงินที่จะคำนวณดอกเบี้ย
        //    //   datCalcDate           วันที่จะคำนวณดอกเบี้ยถึง
        //    //   strIntCalcMethod      วิธีการคำนวณดอกเบี้ย
        //    //                           'D' single rate คำนวณดอกเบี้ยรายวัน และอัตราดอกเบี้ยจากสัญญาคือ dblInterestRate
        //    //                           'M' single rate คำนวณดอกเบี้ยรายเดือน และอัตราดอกเบี้ยจากสัญญาคือ dblInterestRate
        //    //                           'd' multiple rate คำนวณดอกเบี้ยรายวัน และอัตราดอกเบี้ยจากตารางดอกเบี้ย
        //    //   dblInterestRate       อัตราดอกเบี้ยของสัญญาเงินกู้
        //    //   intRoundIntMethod     การปัดเศษของดอกเบี้ย 0 - ไม่ป้ดเศษ, 25-ป้ดเศษ 25 สตางค์, 50-ป้ดเศษ 50 สตางค์,100-ป้ดเศษ 100 สตางค์
        //    //============================


        //    string SQLStmt = null;
        //    System.DateTime datFirstDate = default(DateTime);
        //    System.DateTime datEndDate = default(DateTime);
        //    double dblCalcIntAmt = 0;
        //    double dblRate = 0;
        //    long Intdays = 0;
        //    double IntMonths = 0;
        //    long intRemainder = 0;

        //    if (datStartDate > datCalcDate | datLastCalcDate >= datCalcDate)
        //    {
        //        return functionReturnValue;
        //    }
        //    dblCalcIntAmt = 0;
        //    datFirstDate = datLastCalcDate;
        //    datEndDate = datCalcDate;


        //    var rsTInterest =
        //        _db.t_interest.Where(p => p.coop_id == coop_id && p.t_type == strTType &&
        //                                  p.t_int == strTInt && p.filestatus == "A")
        //           .OrderByDescending(p => p.first_effect_date)
        //           .FirstOrDefault();

        //    if (rsTInterest != null || strTInt == "0")
        //    {
        //        //MsgBox "รหัสดอกเบี้ย หรือ วันที่คำนวณดอกเบี้ยไม่ถูกต้อง โปรดตรวจสอบข้อมูล"
        //        //dblRate = dblInterestRate
        //        //dblCalcIntAmt = dblBalance * dblInterestRate * Intdays / (gsDaysInYear * 100)
        //        switch (strIntCalcMethod)
        //        {
        //            // 'M' single rate คำนวณดอกเบี้ยรายเดือน และอัตราดอกเบี้ยจากสัญญาคือ dblInterestRate
        //            case "M": 
        //                IntMonths = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Month, datFirstDate, datEndDate) - 1;
        //                //If Day(datFirstDate) > 15 Then
        //                //If Val(Format(datFirstDate, "DD")) > 15 Then
        //                if (datFirstDate.Day > 15)
        //                {
        //                    IntMonths = IntMonths + 0.5;
        //                }
        //                else
        //                {
        //                    IntMonths = IntMonths + 1;
        //                }
        //                dblCalcIntAmt = dblBalance * dblInterestRate * IntMonths / 1200;
        //                break;
        //            default:
        //                // 'D' single rate คำนวณดอกเบี้ยรายวัน และอัตราดอกเบี้ยจากสัญญาคือ dblInterestRate
        //                dblRate = dblLoanIntRate(rsTInterest, dblBalance);
        //                //Edit by Ton! 11/Aug/08
        //                Intdays = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate);
        //                dblCalcIntAmt = dblBalance * dblInterestRate * Intdays / (gsDaysInYear * 100);
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        //while (!(rsTInterest.EOF))
        //        //{
        //        //    if (Information.IsDBNull(rsTInterest.Fields("last_effect_date").Value))
        //        //    {
        //        //        //If IsDBNull(rsTInterest.Fields("last_effect_date").Value) Then
        //        //        break; // TODO: might not be correct. Was : Exit Do
        //        //    }
        //        //    else
        //        //    {
        //        //        if (rsTInterest.Fields("last_effect_date").Value >= datFirstDate)
        //        //        {
        //        //            break; // TODO: might not be correct. Was : Exit Do
        //        //        }
        //        //        rsTInterest.MoveNext();
        //        //    }
        //        //}
        //        // single rate
        //        if (rsTInterest.last_effect_date.HasValue)
        //        {
        //            dblRate = dblLoanIntRate(rsTInterest, dblBalance);
        //            datEndDate = datCalcDate;
        //            Intdays = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate);
        //            dblCalcIntAmt = dblBalance * dblRate * Intdays / (gsDaysInYear * 100);
        //            // multiple rate
        //        }
        //        else
        //        {
        //            //while (!(rsTInterest.EOF))
        //            //{
        //            //    dblRate = dblLoanIntRate(rsTInterest, dblBalance);

        //            //    // Edit by Diaw 18NOV2008 
        //            //    if (Information.IsDBNull(rsTInterest.Fields("last_effect_date").Value))
        //            //    {
        //            //        datEndDate = datCalcDate;
        //            //        Intdays = DateDiff(Simulate.DateInterval.Day, datFirstDate, datEndDate);
        //            //        dblCalcIntAmt = dblCalcIntAmt + (dblBalance * dblRate * Intdays / (gsDaysInYear * 100));
        //            //        break; // TODO: might not be correct. Was : Exit Do
        //            //    }
        //            //    else if (rsTInterest.Fields("last_effect_date").Value >= datCalcDate)
        //            //    {
        //            //        datEndDate = datCalcDate;
        //            //        Intdays = DateDiff(Simulate.DateInterval.Day, datFirstDate, datEndDate);
        //            //        dblCalcIntAmt = dblCalcIntAmt + (dblBalance * dblRate * Intdays / (gsDaysInYear * 100));
        //            //        break; // TODO: might not be correct. Was : Exit Do
        //            //    }
        //            //    else
        //            //    {
        //            //        datEndDate = rsTInterest.Fields("last_effect_date").Value;
        //            //        Intdays = DateDiff(Simulate.DateInterval.Day, datFirstDate, datEndDate) + 1;
        //            //        dblCalcIntAmt = dblCalcIntAmt + (dblBalance * dblRate * Intdays / (gsDaysInYear * 100));
        //            //    }
        //            //    // End Edit
        //            //    datFirstDate = DateAdd(Simulate.DateInterval.Day, 1, rsTInterest.Fields("last_effect_date").Value);
        //            //    rsTInterest.MoveNext();
        //            //}
        //        }
        //    }
        //    if (bolRound)
        //    {
        //        dblCalcIntAmt = Math.Round(dblCalcIntAmt);
        //    }
        //    if (bolAdjust)
        //    {
        //        dblCalcIntAmt = dblLoanIntAmtRounded(dblCalcIntAmt, intRoundIntMethod);

        //    }
        //    functionReturnValue = dblCalcIntAmt;
        //    return functionReturnValue;
        //}

        private double dblLoanIntRate(t_interest rsTInterest, double dblBalance)
        {
            if (rsTInterest.balance_1 != null && dblBalance <= (double)rsTInterest.balance_1)
            {
                if (rsTInterest.rate_1 != null) return (double)rsTInterest.rate_1;
            }
            else if (rsTInterest.balance_2 != null && dblBalance <= (double)rsTInterest.balance_2)
            {
                if (rsTInterest.rate_2 != null) return (double)rsTInterest.rate_2;
            }
            else if (rsTInterest.balance_3 != null && dblBalance <= (double)rsTInterest.balance_3)
            {
                if (rsTInterest.rate_3 != null) return (double)rsTInterest.rate_3;
            }
            else if (rsTInterest.balance_4 != null && dblBalance <= (double)rsTInterest.balance_4)
            {
                if (rsTInterest.rate_4 != null) return (double)rsTInterest.rate_4;
            }
            else
            {
                if (rsTInterest.rate_5 != null) return (double)rsTInterest.rate_5;
            }


            return 0;
        }

        //public double dblLoanIntAmtRounded(double dblLoanInt, long intRoundIntMethod)
        //{
        //    double functionReturnValue = 0;
        //    //LoanJUS (20/Dec/08)
        //    double intRemainder = 0; //Dim intRemainder As Long = 0
        //    functionReturnValue = Math.Round(dblLoanInt); //dblLoanIntAmtRounded = Rounded(dblLoanInt)
        //    dblLoanInt = Math.Round(dblLoanInt); //dblLoanInt = Rounded(dblLoanInt)
        //    if (intRoundIntMethod > 0) //If intRoundIntMethod > 0 Then
        //    {
        //        if ((int)((dblLoanInt * 100 - (int)(dblLoanInt) * 100 + 0.01)) > 0) //If Int((dblLoanInt * 100 - Int(dblLoanInt) * 100 + 0.01)) > 0 Then
        //        {
        //            intRemainder = Math.Round((double)((int)(dblLoanInt * 100 - (int)dblLoanInt * 100) % intRoundIntMethod)); //intRemainder = Rounded(Int((dblLoanInt * 100 - Int(dblLoanInt) * 100) Mod intRoundIntMethod))
        //            if (intRemainder > 0) //If intRemainder > 0 Then
        //            {
        //                intRemainder = Math.Round(((dblLoanInt * 100 - (int)dblLoanInt * 100) / intRoundIntMethod + 1) * intRoundIntMethod); //intRemainder = Rounded((Int((dblLoanInt * 100 - Int(dblLoanInt) * 100)) \ intRoundIntMethod + 1) * intRoundIntMethod)
        //                dblLoanInt = ((int)dblLoanInt) + (intRemainder / 100); //dblLoanInt = Int(dblLoanInt) + intRemainder / 100
        //            }
        //            else //Else
        //            {
        //                dblLoanInt = (int)dblLoanInt + (int)((dblLoanInt * 100 - (int)dblLoanInt * 100)) / 100; //dblLoanInt = Int(dblLoanInt) + Int((dblLoanInt * 100 - Int(dblLoanInt) * 100)) / 100
        //            } //End If
        //        } //End If
        //    }
        //    else //Else
        //    {
        //        if ((int)(dblLoanInt * 100 - (int)dblLoanInt * 100) > Math.Abs(intRoundIntMethod)) //If Int((dblLoanInt * 100 - Int(dblLoanInt) * 100)) > System.Math.Abs(intRoundIntMethod) Then
        //        {
        //            dblLoanInt = dblLoanInt + 1; //dblLoanInt = Int(dblLoanInt) + 1
        //        }
        //        //else //Else
        //        //{
        //        //    dblLoanInt = dblLoanInt; //dblLoanInt = Int(dblLoanInt)
        //        //} //End If
        //    }
        //    functionReturnValue = dblLoanInt;
        //    return functionReturnValue;
        //}


        public double dblLoanPaymentAmt(string strLoanId, string coop_id)
        {
            var model = _db.loan_payment.OrderByDescending(p => p.pay_date)
               .FirstOrDefault(p => p.coop_id == coop_id && p.loan_id == strLoanId && p.filestatus == "A");
            return model != null && model.bal != null ? (double)model.bal : 0;
        }

        public double dblRoundSubstract(double dblAmt, long intRoundAmt)
        {
            double functionReturnValue = 0;
            double dblIntAmt = 0;
            dblAmt = Math.Round(dblAmt);
            if (intRoundAmt > 0)
            {
                if ((dblAmt * 100 - dblAmt * 100) > 0)
                {
                    double intRemainder = (dblAmt * 100 - dblAmt * 100) % intRoundAmt;
                    dblIntAmt = dblAmt - intRemainder / 100;
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


        public DateTime dtFirstInstall(Int16 intGracePeriod, DateTime dtThisMonthProcess)
        {
            return dtThisMonthProcess.AddMonths(intGracePeriod);
        }


        //public int intInstallMonth(string strTLoan, string strPrincipleMethod, double dblLoanAmt, double dblIntRate, double dblInstallAmt,string coop_id)
        //{
        //    int functionReturnValue = 0;
        //    // ERROR: Not supported in C#: OnErrorStatement

        //    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //    // function หาค่า Install amount (เงินงวด) สำหรับสัญญาเงินกู้ที่จ่ายเงินแบบธนาคาร และคำนวณเงินงวดจากสูตรหาค่าเงินปัจจุบัน 
        //    // คือ Nper ใช้สำหรับสัญญาเงินกู้พิเศษ
        //    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //  var rsTLoanInstall = _db.t_loan_install.OrderByDescending(p=>p.balance).FirstOrDefault(p=>p.coop_id == coop_id && p.t_loan == strTLoan && p.filestatus == "A")

        //    if (strPrincipleMethod == "B")
        //    {
        //        //intInstallMonth = NPer((dblIntRate / 100) / 12, -dblInstallAmt, dblLoanAmt, FVal, ENDPERIOD) + 10
        //        functionReturnValue = Microsoft.VisualBasic.Financial.NPer((dblIntRate / 100) / 12, -dblInstallAmt, dblLoanAmt, 0, Microsoft.VisualBasic.DueDate.EndOfPeriod) + 10;
        //    }
        //    else
        //    {

        //        if (rsTLoanInstall != null)
        //        {
        //            while (!(rsTLoanInstall.EOF))
        //            {
        //                if (dblLoanAmt <= InsertDBL(rsTLoanInstall, "balance"))
        //                {
        //                    functionReturnValue = InsertDBL(rsTLoanInstall, "install_month");
        //                    if (dblInstallAmt > 0)
        //                    {
        //                        int intInstall = 0;
        //                        if (dblLoanAmt / dblInstallAmt == (dblLoanAmt / dblInstallAmt))
        //                        {
        //                            intInstall = Conversion.Int(dblLoanAmt / dblInstallAmt);
        //                        }
        //                        else
        //                        {
        //                            intInstall = Conversion.Int(dblLoanAmt / dblInstallAmt) + 1;
        //                        }
        //                        if (intInstall < functionReturnValue)
        //                        {
        //                            functionReturnValue = intInstall;
        //                        }
        //                    }
        //                    break; // TODO: might not be correct. Was : Exit Do
        //                }

        //            }
        //        }
        //        else
        //        {
        //            if (dblInstallAmt > 0)
        //            {
        //                functionReturnValue = dblLoanAmt / dblInstallAmt;
        //                if (intInstallMonth * dblInstallAmt < dblLoanAmt)
        //                {
        //                    functionReturnValue = intInstallMonth + 1;
        //                }
        //                else
        //                {
        //                    functionReturnValue = intInstallMonth;
        //                }
        //            }
        //        }
        //    }
        //    return functionReturnValue;
        //}


        public long intLoanInstall(string strTLoan, double dblLoanAmt, string coop_id)
        {
            long functionReturnValue = 0;
            // ERROR: Not supported in C#: OnErrorStatement

            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            // function หาค่า Install amount (เงินงวด) สำหรับสัญญาเงินกู้ที่จ่ายเงินแบบสหกรณ์ และกำหนดจากวงเงินกู้
            // เช่น สส. ฉฉ. เป็นต้น
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //ADODB.Recordset rsTLoanInstall = default(ADODB.Recordset);
            //functionReturnValue = 0;
            //SQLStmt = "select * from t_loan_install where coop_id = " +
            //          "'" + gsCoopId + "' and t_loan = '" + strTLoan + "' and " +
            //          "filestatus = 'A' order by balance;";
            //rsTLoanInstall = DataSet(SQLStmt);

            //rsTLoanInstall = _db.t_loan_install.OrderByDescending(p=>p.balance).FirstOrDefault(p=>p.coop_id == coop_id && p.t_loan == strTLoan)
            ////    If Not rsTLoanInstall.EOF Then
            //while (!(rsTLoanInstall.EOF))
            //{
            //    if (dblLoanAmt <= InsertDBL(rsTLoanInstall, "balance"))
            //    {
            //        functionReturnValue = InsertDBL(rsTLoanInstall, "install_month");
            //        break; // TODO: might not be correct. Was : Exit Do
            //    }
            //    rsTLoanInstall.MoveNext();
            //}
            return functionReturnValue;
            //    End If

        }


        public string strIssueLoanId(string strTLoan, int intCnt, string coop_id, string BudgetYear)
        {
            string functionReturnValue = null;


            if (strTLoan != "")
            {
                var rsTLoanIssue = _db.t_loan.LastOrDefault(p => p.coop_id == coop_id && p.t_loan1 == strTLoan);

                BudgetYear = BudgetYear.Substring(3, 2);
                if (intCnt > 0)
                {
                    if (rsTLoanIssue != null)
                    {
                        var loanPrefix = rsTLoanIssue.loan_prefix.Substring(0, 1);
                        functionReturnValue = loanPrefix + intCnt.ToString(CultureInfo.InvariantCulture).Trim() +
                                              int.Parse((rsTLoanIssue.last_loan_id + 1).ToString()).ToString("#####") +
                                              BudgetYear;
                    }
                }
                else
                {
                    if (rsTLoanIssue != null)
                        functionReturnValue = rsTLoanIssue.loan_prefix +
                                              intCnt.ToString(CultureInfo.InvariantCulture).Trim() +
                                              int.Parse((rsTLoanIssue.last_loan_id + 1).ToString()).ToString("#####") +
                                              BudgetYear;
                }


                var u = _db.t_loan.FirstOrDefault(p => p.coop_id == coop_id && p.t_loan1 == rsTLoanIssue.t_loan1);
                if (rsTLoanIssue != null)
                {
                    if (u != null) u.last_loan_id = rsTLoanIssue.last_loan_id + 1;
                    _db.SaveChanges();
                }
            }
            return functionReturnValue;
        }


        public string strLoanIdMask(object strLoanId, object strMaskLoanId)
        {
            string functionReturnValue = null;


            //functionReturnValue = String.Left(strLoanId, Strings.InStr(strMaskLoanId, "-") - 1) + "-" +
            //                      Strings.Right(Strings.Left(strLoanId, Strings.InStr(strMaskLoanId, "/") - 2),
            //                                    Strings.InStr(strMaskLoanId, "/") - Strings.InStr(strMaskLoanId, "-") -
            //                                    1) + "/" + Strings.Right(strLoanId, 2);
            return functionReturnValue;
        }



        public double MaxLoanReduce(string memberId, string tLoan, string coopId, long roundIntMethod, DateTime dtCalcDate, ref double monthAmt,
                                    ref double loanBalSpc)
        {
            var loanReduce = from p in _db.t_loan_reduce
                             join l in _db.loans
                             on new { A = p.coop_id, B = p.t_loan } equals new { A = l.coop_id, B = l.t_loan }
                             where p.filestatus == "A" && l.filestatus == "A"
                             && p.coop_id == coopId && p.t_loan == tLoan && l.member_id == memberId
                             select new LoanReduceModel
                             {
                                 loan_reduce_action = p.loan_reduce_action,
                                 loan_bal = (double)(l.loan_bal ?? 0),
                                 t_principle = l.t_principle,
                                 install_amt = (double)(l.install_amt),
                                 t_loan = l.t_loan,
                                 t_int = l.t_int,
                                 start_calc_int = l.start_calc_int ?? DateTime.Now,
                                 last_calc_int = l.last_calc_int ?? DateTime.Now,
                                 t_int_calc = l.t_int_calc,
                                 interest_rate = (double)(l.interest_rate ?? 0),
                                 loan_month_action = p.loan_month_action
                             };


            foreach (var loanr in loanReduce)
            {
                if (loanr.loan_reduce_action == "+")
                {
                    loanBalSpc += loanr.loan_bal;
                    monthAmt += loanr.loan_bal;
                }
                if (loanr.loan_month_action == "+")
                {
                    if (loanr.t_principle == "B")
                    {
                        monthAmt = monthAmt + loanr.install_amt;
                    }
                    else
                    {
                        double intCalc = dblLoanIntAmt(coopId, loanr.t_loan, loanr.t_int, loanr.loan_bal,
                                                            loanr.start_calc_int, loanr.last_calc_int, dtCalcDate,
                                                            loanr.t_int_calc,
                                                            loanr.interest_rate, roundIntMethod, true, true);
                        monthAmt = monthAmt + (loanr.install_amt + intCalc);
                    }
                }
            }
            return monthAmt;
        }



        public double dblLoanIntAmt(string coopId, string strTType, string strTInt, double dblBalance, DateTime datStartDate, DateTime datLastCalcDate,
                          DateTime datCalcDate, string strIntCalcMethod, double dblInterestRate, long intRoundIntMethod, bool bolRound, bool bolAdjust)
        {
            DateTime datFirstDate = datLastCalcDate;
            DateTime datEndDate = datCalcDate;
            double dblCalcIntAmt = 0;
            long intDays = 0;
            double intMonths = 0;
            long intRemainder = 0;
            double dblRate = 0;
            if (datStartDate > datCalcDate || datLastCalcDate >= datCalcDate)
            {
                return 0;
            }

            var rsTInterest =
                _db.t_interest.Where(
                    p =>
                    p.coop_id == coopId && p.t_type == strTType && p.t_int == strTInt && p.filestatus == FileStatus.A)
                   .OrderBy(p => p.first_effect_date);

            if (rsTInterest.Any() || strTInt == "0")
            {
                switch (strIntCalcMethod)
                {
                    case "M":
                        intMonths = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Month, datFirstDate, datEndDate);
                        if (datFirstDate.Day > 15)
                        {
                            intMonths = intMonths + 0.5;
                        }
                        else
                        {
                            intMonths = intMonths + 1;
                        }
                        dblCalcIntAmt = dblBalance * dblInterestRate * intMonths / 1200;
                        break;
                    default:
                        double dayOfYear = AuthorizeHelper.Current.CoopSystem().days_in_year ?? 0;
                        //  dblRate = dblLoanIntRate(rsTInterest, dblBalance);
                        intDays = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate);
                        dblCalcIntAmt = dblBalance * dblInterestRate * intDays / (dayOfYear * 100);
                        break;
                }
            }
            else
            {
                var singleNull = rsTInterest.Where(p => p.last_effect_date == null);
                if (singleNull.Any())
                {
                    dblRate = dblLoanIntRate(singleNull.FirstOrDefault(), dblBalance);
                    datEndDate = datCalcDate;
                    intDays = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate);
                    double dayOfYear = AuthorizeHelper.Current.CoopSystem().days_in_year ?? 0;
                    dblCalcIntAmt = dblBalance * dblRate * intDays / (dayOfYear * 100);
                }
                else
                {
                    var single = rsTInterest.Where(p => p.last_effect_date.Value >= datFirstDate);
                    if (single.Any())
                    {
                        dblRate = dblLoanIntRate(single.FirstOrDefault(), dblBalance);
                        datEndDate = datCalcDate;
                        intDays = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate);
                        double dayOfYear = AuthorizeHelper.Current.CoopSystem().days_in_year ?? 0;
                        dblCalcIntAmt = dblBalance * dblRate * intDays / (dayOfYear * 100);
                    }
                }

                foreach (var tInterest in rsTInterest)
                {
                    double dayOfYear = AuthorizeHelper.Current.CoopSystem().days_in_year ?? 0;
                    if (tInterest.last_effect_date == null)
                    {

                        datEndDate = datCalcDate;
                        intDays = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate);
                        dblCalcIntAmt = dblCalcIntAmt + (dblBalance * dblRate * intDays / (dayOfYear * 100));
                        break;
                    }
                    else if (tInterest.last_effect_date.Value >= datCalcDate)
                    {
                        datEndDate = datCalcDate;
                        intDays = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate);
                        dblCalcIntAmt = dblCalcIntAmt + (dblBalance * dblRate * intDays / (dayOfYear * 100));
                        break;
                    }
                    else
                    {
                        datEndDate = tInterest.last_effect_date.Value;
                        intDays = DateTimeExtension.DateDiff(DateTimeExtension.DateInterval.Day, datFirstDate, datEndDate) + 1;
                        dblCalcIntAmt = dblCalcIntAmt + (dblBalance * dblRate * intDays / (dayOfYear * 100));
                    }
                    if (tInterest.last_effect_date != null) datFirstDate = tInterest.last_effect_date.Value.AddDays(1);
                }
            }
            if (bolRound)
            {
                dblCalcIntAmt = Core.Rounded(dblCalcIntAmt);
            }
            if (bolAdjust)
            {
                dblCalcIntAmt = dblLoanIntAmtRounded(dblCalcIntAmt, intRoundIntMethod);
            }

            return dblCalcIntAmt;
        }

        public double dblLoanIntAmtRounded(double dblLoanInt, long intRoundIntMethod)
        {
            double intRemainder = 0;
            var dblAmtRound = Core.Rounded(dblLoanInt);
            dblLoanInt = Core.Rounded(dblLoanInt);
            if (intRoundIntMethod > 0)
            {
                if ((int)((dblLoanInt * 100 - (int)(dblLoanInt) * 100 + 0.01)) > 0)
                {
                    intRemainder = Core.Rounded((int)((dblLoanInt * 100 - (int)(dblLoanInt) * 100) % intRoundIntMethod));
                    if (intRemainder > 0)
                    {
                        intRemainder =
                            Core.Rounded(((int)(dblLoanInt * 100 - (int)(dblLoanInt) * 100) / intRoundIntMethod + 1) * intRoundIntMethod);
                        dblLoanInt = (int)(dblLoanInt) + intRemainder / 100;

                    }
                    else
                    {
                        dblLoanInt = (int)(dblLoanInt) + (int)((dblLoanInt * 100 - (int)(dblLoanInt) * 100)) / 100;
                    }
                }
            }
            else
            {
                if ((int)(dblLoanInt * 100 - (int)(dblLoanInt) * 100) > Math.Abs(intRoundIntMethod))
                {
                    dblLoanInt = (int)(dblLoanInt) + 1;
                }
                else
                {
                    dblLoanInt = (int)(dblLoanInt);
                }
            }

            return dblLoanInt;
        }


    }
}