using System;
using Coop.Entities;
using Coop.Infrastructure.ActionFilters;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
//using Coop.Resources;

using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Coop.Library;
using Microsoft.SqlServer.Server;
//using Microsoft.VisualBasic;
//using Coop.Controllers;

//using Microsoft.Ajax.Utilities;
//using System.Globalization;
//using System.Web.UI;

namespace Coop.Controllers
{
    public class OtxLoanController : Controller
    {
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;
        public OtxLoanController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            ViewBag.LoanTypeList = new SelectList(GetLoanTypeList(), "LoanTypeID", "LoanTypeName");
            ViewBag.TxnCodeList = new SelectList(GetTxnCodeList(), "TTxnCode", "Descript");
            return View();
        }
        public List<LoanType> GetLoanTypeList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<LoanType> LoanTypes = db.LoanType.Where(d => d.Filestatus == "A").ToList();
            return LoanTypes;
        }
        public List<TxnCode> GetTxnCodeList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<TxnCode> TxnCodes = db.TxnCode.Where(p => p.Filestatus == FileStatus.A && p.ProgramName == "OtxLoan").ToList();
            //List<TxnCode> TxnCodes = db.TTxnCode.Where(p => p.TxnType == TxnType.DEP && p.Filestatus == FileStatus.A && p.ProgramName == Otx.DEP).ToList();
            return TxnCodes;
        }
        [Authorization]
        public ActionResult TransferInLoan(OtxLoanModel OtxLonModel)
        {
            if (OtxLonModel == null || OtxLonModel.Equals(new OtxLoanModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new OtxLoanModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            OtxLoanModel model = OtxLonModel;

            /// check Loan
            if (string.IsNullOrWhiteSpace(model.LoanID))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Invalid Loan Code" };
                return Json(new object[] { oper, new OtxLoanModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            /// check if exist
            var LID = model.LoanID;

            /// get Bank Account No.
            /// 
            //var lonData = _unitOfWork.Loan.ReadDetail(pCode).FirstOrDefault();
            var lonData = _unitOfWork.Loan.ReadLoanInfo(LID);
            bool result = false;
            string msg = string.Empty;

            /// Account Exist
            if (lonData != null && !string.IsNullOrWhiteSpace(lonData.LoanID))
            {
                result = true;
                var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();
                DateTime sysDate = (DateTime)coopData.SystemDate;
                //DateTime sysDate = AuthorizeHelper.Current.CoopControls().SystemDate ?? DateTime.Now;
                DateTime firstDate = (DateTime)lonData.LastCalcInt;
                long Days = NoDays(firstDate, sysDate);
                double IntCalc = (double)lonData.LoanBal * (double)lonData.IntRate * Days / 36500;
                int IntCalcI = (int)(IntCalc * 100) + 50;
                IntCalc = IntCalcI / 100;

                double ChargeCalc = 0;
                var lonTypeData = _unitOfWork.LoanType.ReadDetail(lonData.LoanTypeID).FirstOrDefault();
                if (lonTypeData != null && lonData.UnpayPrinciple != null && lonTypeData.ChargeRate != null)
                {
                    DateTime chargeDate = (DateTime)lonData.LastCalcCharge;
                    Days = NoDays(chargeDate, sysDate);
                    ChargeCalc = (double)lonData.UnpayPrinciple * (double)lonTypeData.ChargeRate * Days / 36500;
                    int ChargeCalcI = (int)(ChargeCalc * 100) + 50;
                    ChargeCalc = ChargeCalcI / 100;
                }
                //var BFBal = lonData.BFBal ?? 0;
                model.Filestatus = lonData.Filestatus;
                model.CoopID = lonData.CoopID;
                model.LoanID = lonData.LoanID;
                model.LoanTypeID = lonData.LoanTypeID;
                model.LoanTypeName = lonData.LoanTypeName;
                model.MemberID = lonData.MemberID;
                model.Name = lonData.Name;
                model.LoanDate = lonData.LoanDate;
                model.LastContact = lonData.LastContact;
                model.LoanAmt = lonData.LoanAmt;
                model.LastCalcInt = lonData.LastCalcInt;
                model.LastCalcCharge = lonData.LastCalcCharge;

                model.BFBal = lonData.BFBal ?? 0;
                model.LoanBal = lonData.LoanBal;
                model.UnpayPrinciple = lonData.UnpayPrinciple ?? 0;

                model.IntRate = lonData.IntRate;

                model.BFInt = lonData.BFInt ?? 0;
                model.BFUnpayInt = lonData.BFUnpayInt ?? 0;
                model.UnpayInt = lonData.UnpayInt + (decimal)IntCalc;
                model.IntCalc = (decimal)IntCalc;
                model.CFUnpayInt = lonData.UnpayInt + (decimal)IntCalc;

                model.BFCharge = lonData.BFCharge ?? 0;
                model.BFUnpayCharge = lonData.BFUnpayCharge ?? 0;
                model.UnpayCharge = lonData.UnpayCharge + (decimal)ChargeCalc;
                model.ChargeCalc = lonData.UnpayCharge + (decimal)ChargeCalc;

                model.TxnDate = sysDate;
                //model.BFDiscInt = lonData.BFDiscInt;
                //model.BFUnpayDiscInt = lonData.BFUnpayDiscInt;
                //model.DiscInt = lonData.DiscInt;
                //model.TtlfLoan.DiscIntCalc = 0sysDate
            }
            else
            {
                result = false;
                msg = "Loan Not Found";
            }
            OperationResult oResult = new OperationResult();
            if (result)
            {
                oResult.Result = result;
                oResult.Message = "Successful";
            }
            else
            {
                oResult.Result = result;
                oResult.Message = msg;
            }
            object[] retObj = new object[] { oResult, model };

            return Json(retObj, "application/json", JsonRequestBehavior.AllowGet);
        }
        int NoDays(DateTime d1, DateTime d2)
        {
            TimeSpan span = d2.Subtract(d1);
            return (int)span.TotalDays;
        }
        //int xLoanTxnSeq()
        //{
        //    var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();
        //    DateTime sysDate = (DateTime)coopData.SystemDate;
        //    int txnSeq = 1;
        //    var ttlfLoan = _unitOfWork.TtlfLoan.ReadBySeqDesc().Where(t => t.TxnDate == sysDate).OrderByDescending(t => t.TxnSeq);
        //    if (ttlfLoan != null)
        //    {
        //        txnSeq = ttlfLoan.TxnSeq + 1;
        //    }
        //    return (int)txnSeq;
        //}
        [Authorization]
        [HttpPost]
        public ActionResult TransferOutOtxLoan(OtxLoanModel otxLonModel)
        {
            if (otxLonModel == null || otxLonModel.Equals(new OtxLoanModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new OtxLoanModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();
            DateTime sysDate = (DateTime)coopData.SystemDate;
            int BookNo = (int)coopData.LastReceiptBookNo;
            int RunNo = (int)coopData.LastReceiptRunNo;
            if (RunNo >= 9999)
            {
                BookNo = BookNo + 1;
                RunNo = 1;
            }
            else
            {
                RunNo = RunNo + 1;
            }

            coopData.LastReceiptBookNo = BookNo;
            coopData.LastReceiptRunNo = RunNo;
            _unitOfWork.CoopControl.UpdateReceiptNo(coopData);

            //var cData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();
            //DateTime sysDate = (DateTime)cData.SystemDate;
            int tSeq = 1;
            //var ttlfLoan = _unitOfWork.TtlfLoan.ReadBySeqDesc(sysDate).OrderByDescending(t => t.TxnSeq);
            var ttlfLoan = _unitOfWork.TtlfLoan.ReadBySeqDesc(sysDate).FirstOrDefault();
            if (ttlfLoan != null)
            {
                tSeq = ttlfLoan.TxnSeq + 1;
            }            
            
            OtxLoanModel model = otxLonModel;
            var LonID = model.LoanID;

            bool result = false;
            string msg = string.Empty;

            var lon = _unitOfWork.Loan.ReadDetail(LonID).FirstOrDefault();
            if (lon != null)
            {
                /// Account Exist => update one
                // Log TtlfLoan
                TtlfLoanModel tModel = new TtlfLoanModel
                {
                    CoopID = otxLonModel.CoopID,
                    TxnDate = sysDate,
                    TxnSeq = tSeq,
                    TxnTime = DateTime.Now,
                    BackDate = DateLib.DateInCE(otxLonModel.TxnDateTH),
                    Filestatus = otxLonModel.Filestatus,
                    UserID = AuthorizeHelper.Current.UserAccount().UserID,
                    //WorkstationID = AuthorizeHelper.Current.CoopControls().WorkStationID,
                    //BranchID = AuthorizeHelper.Current.CoopControls().BranchID,
                    //BudgetYear = AuthorizeHelper.Current.CoopControls().BudgetYear,
                    OriginalProcess = "OtxLoan",
                    MemberID = otxLonModel.MemberID,
                    LoanID = otxLonModel.LoanID,
                    BFBal = otxLonModel.LoanBal,
                    Amt1 = otxLonModel.Amt1,
                    CFBal = otxLonModel.CFLoanBal,

                    BFInt = otxLonModel.BFInt,
                    IntCalc = otxLonModel.IntCalc,
                    IntAmt = otxLonModel.IntAmt,
                    UnpayInt = otxLonModel.UnpayInt,

                    BFCharge = otxLonModel.BFCharge,
                    ChargeCalc = otxLonModel.ChargeCalc,
                    ChargeAmt = otxLonModel.ChargeAmt,
                    UnpayCharge = otxLonModel.UnpayCharge,

                    PrincipleAmt = otxLonModel.PrincipleAmt,
                    CFUnpayPrinciple = otxLonModel.CFUnpayPrinciple,
                    TTxnCode = otxLonModel.TTxnCode,
                    CDCode = otxLonModel.CDCode,

                    RcptBookNo = Utility.FillZero((int)BookNo, 3),
                    RcptRunNo = Utility.FillZero((int)RunNo, 4)
                };
                _unitOfWork.TtlfLoan.LogTtlfLoan(tModel);

                // update loan
                if (lon != null && !string.IsNullOrWhiteSpace(lon.LoanID))
                {
                    //Update Loan
                    lon.LoanID = model.LoanID;
                    lon.Filestatus = model.Filestatus;
                    lon.LoanBal = model.CFLoanBal;

                    lon.LastCalcInt = DateLib.DateInCE(model.TxnDateTH);
                    lon.LastCalcCharge = DateLib.DateInCE(model.TxnDateTH);
                    lon.LastContact = DateLib.DateInCE(model.TxnDateTH);
                    // คำนวณดอกเบี้ย
                    lon.BFInt = model.BFInt;
                    lon.UnpayInt = model.CFUnpayInt;
                    lon.AccInt = lon.AccInt + model.IntAmt;
                    lon.YTDAccInt = lon.YTDAccInt + model.IntAmt;
                    // คำนวณค่าปรับ
                    lon.BFCharge = lon.BFCharge;
                    lon.UnpayCharge = model.CFUnpayCharge;
                    lon.UnpayPrinciple = model.CFUnpayPrinciple;
                    using (TransactionScope tranScope = new TransactionScope())
                    {
                        try
                        {
                            _unitOfWork.Loan.UpdateOtxLoan(lon);
                            tranScope.Complete();
                            result = true;
                        }
                        catch (Exception exception)
                        {
                            result = false;
                            msg = "Transaction Roll backed due to some exception:" + exception.Message;
                        }
                    }
                }
            }
            OperationResult oResult = new OperationResult();
            if (result)
            {
                oResult.Result = result;
                oResult.Message = "Successful";
            }
            else
            {
                oResult.Result = result;
                oResult.Message = msg;
            }
            object[] retObj = new object[] { oResult, model };

            return Json(retObj, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}