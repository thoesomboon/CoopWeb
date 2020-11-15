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

//using Microsoft.Ajax.Utilities;
//using System.Globalization;
//using System.Web.UI;

namespace Coop.Controllers
{
    public class OtxDepositController : Controller
    {
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;
        public OtxDepositController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorization]
        public ActionResult Index()
        {
            ViewBag.DepositTypeList = new SelectList(GetDepositTypeList(), "DepositTypeID", "DepositTypeName");
            ViewBag.TxnCodeList = new SelectList(GetTxnCodeList(), "TTxnCode", "Descript");
            return View();
        }
        public List<DepositType> GetDepositTypeList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<DepositType> DepositTypes = db.DepositType.Where(d => d.Filestatus == "A").ToList();
            return DepositTypes;
        }
        public List<TxnCode> GetTxnCodeList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<TxnCode> TxnCodes = db.TxnCode.Where(p => p.Filestatus == FileStatus.A && p.ProgramName == Otx.DEP).ToList();
            //List<TxnCode> TxnCodes = db.TTxnCode.Where(p => p.TxnType == TxnType.DEP && p.Filestatus == FileStatus.A && p.ProgramName == Otx.DEP).ToList();
            return TxnCodes;
        }
        public ActionResult TransferInOtxDeposit(string AccNo)
        {
            /// check if exist
            if (string.IsNullOrWhiteSpace(AccNo))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            //var mID = AccNo;
            /// get Bank Account No.
            /// 
            //var pInfo = _unitOfWork.Deposit.ReadDetail(pCode).FirstOrDefault();
            var depositData = _unitOfWork.Deposit.ReadOtxDeposit(AccNo);

            bool result = false;

            string msg = string.Empty;

            /// Account Exist
            if (depositData != null && !string.IsNullOrWhiteSpace(depositData.AccountNo))
            {
                result = true;
                //Read Coop
                var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();
                depositData.TxnDate = coopData.SystemDate;

                var daysInYear = AuthorizeHelper.Current.CoopControls().DaysINYear;

                //Read DepositType
                var depositTypeData = _unitOfWork.DepositType.ReadDetail(depositData.DepositTypeID).FirstOrDefault();
                depositData.TypeOfDeposit = depositTypeData.TypeOfDeposit;
                depositData.CalcIntType = depositTypeData.CalcIntType;
                depositData.CalcIntRate = depositTypeData.CalcIntRate;

                depositData.MonthMaxWithdrawAmt = depositTypeData.MonthMaxWithdrawAmt;
                depositData.MonthMaxWithdrawTimes = depositTypeData.MonthMaxWithdrawTimes;
                depositData.MaxChargeAmt = depositTypeData.MaxChargeAmt;
                depositData.MinChargeAmt = depositTypeData.MinChargeAmt;
                depositData.WithdrawChargePercent = depositTypeData.WithdrawChargePercent;
                depositData.MinDepAmt = depositTypeData.MinDepAmt;
                depositData.MaxDepAmt = depositTypeData.MaxDepAmt;
                depositData.MinWithdrawAmt = depositTypeData.MinWithdrawAmt;
                depositData.MaxWithdrawAmt = depositTypeData.MaxWithdrawAmt;
                depositData.MinLedgerBal = depositTypeData.MinLedgerBal;

                List<InterestModel> lstInterest = _unitOfWork.Interest.ReadDetail().Where(p => p.Type == depositData.DepositTypeID &&
                                                                                                p.TInt == depositData.IntType &&
                                                                                                p.Filestatus == "A")
                                                                                    .OrderBy(p => p.FirstEffectDate)
                                                                                    .ToList();

                double dblIntCalc = Coop.Library.Deposit.DepositIntAmt(lstInterest, (DateTime)depositData.LastCalcInt,
                            (double)depositData.AvailBal, depositData.DepositTypeID, depositData.IntType, (DateTime)coopData.SystemDate,
                            depositTypeData.CalcIntType, (double)depositData.AvailBal, (int)coopData.DaysINYear);

                depositData.IntAmt = (Decimal)dblIntCalc;
            }
            else
            {
                result = false;
                msg = "Deposit Not Found";
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
            object[] retObj = new object[] { oResult, depositData };

            return Json(retObj, "application/json", JsonRequestBehavior.AllowGet);
        }
        public ActionResult CalcIntOtxDeposit(string AccNo, string BkDate)
        {
            /// check if exist
            if (string.IsNullOrWhiteSpace(AccNo))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            //var mID = AccNo;
            /// get Bank Account No.
            /// 
            //var pInfo = _unitOfWork.Deposit.ReadDetail(pCode).FirstOrDefault();
            var depositData = _unitOfWork.Deposit.ReadOtxDeposit(AccNo);

            bool result = false;

            string msg = string.Empty;

            /// Account Exist
            if (depositData != null && !string.IsNullOrWhiteSpace(depositData.AccountNo))
            {
                result = true;
                //var coopData = _unitOfWork.CoopControl.ReadCoop().FirstOrDefault();
                var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();
                var daysInYear = AuthorizeHelper.Current.CoopControls().DaysINYear;

                var depositTypeData = _unitOfWork.DepositType.ReadDetail(depositData.DepositTypeID).FirstOrDefault();

                List<InterestModel> lstInterest = _unitOfWork.Interest.ReadDetail().Where(p => p.Type == depositData.DepositTypeID &&
                                                                                               p.TInt == depositData.IntType &&
                                                                                               p.Filestatus == "A")
                                                                                    .OrderBy(p => p.FirstEffectDate)
                                                                                    .ToList();

                //var BackDate = Coop.Library.DateLib.DateTHDateTimeCE(BkDate);
                var BackDate = coopData.SystemDate;
                var CalcIntDate = coopData.SystemDate;
                if (BackDate != CalcIntDate)
                {
                    CalcIntDate = BackDate;
                }

                double dblIntCalc = Coop.Library.Deposit.DepositIntAmt(lstInterest, (DateTime)depositData.LastCalcInt,
                            (double)depositData.AvailBal, depositData.DepositTypeID, depositData.IntType, (DateTime)CalcIntDate,
                            depositTypeData.CalcIntType, (double)depositData.AvailBal, (int)coopData.DaysINYear);
                //var accInt = depositData.AccInt + dblIntCalc;
                depositData.IntAmt = (Decimal)dblIntCalc;
            }
            else
            {
                result = false;
                msg = "Deposit Not Found";
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
            object[] retObj = new object[] { oResult, depositData };

            return Json(retObj, "application/json", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult TransferOutOtxDeposit(OtxDepositModel otxDepModel)
        {
            if (otxDepModel == null || otxDepModel.Equals(new OtxDepositModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new OtxDepositModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            //var coopId = AuthorizeHelper.Current.CoopSystem().coop_id;
            //var systemDate = AuthorizeHelper.Current.CoopSystem().system_date;
            OtxDepositModel model = otxDepModel;
            var AccNo = model.AccountNo;

            bool result = false;
            string msg = string.Empty;

            var dep = _unitOfWork.Deposit.ReadDetail(AccNo).FirstOrDefault();
            // Account Exist => Transaction process start
            if (dep != null && !string.IsNullOrWhiteSpace(dep.AccountNo))
            {
                var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();
                DateTime sysDate = (DateTime)coopData.SystemDate;
                int txnSeq = 1;
                var ttlfdeposit = _unitOfWork.TtlfDeposit.ReadBySeqDesc(sysDate).FirstOrDefault();
                if (ttlfdeposit != null)
                {
                    txnSeq = ttlfdeposit.TxnSeq + 1;
                }
                var uNoBook = _unitOfWork.NoBook.ReadBySeqDesc(AccNo).FirstOrDefault();
                int iSeq = 1;
                if (uNoBook != null)
                {
                    iSeq = uNoBook.Seq + 1;
                };
                decimal BFBal = (decimal)model.LedgerBal;
                decimal Amt = (decimal)model.Amt;
                decimal Cr = (decimal)model.Credit;
                decimal Dr = (decimal)model.Debit;
                decimal CFBal = (decimal)model.CFLedgerBal;
                decimal AcInt = (decimal)model.AccInt;
                // ปิดบัญชี ต้อง Log NoBook และ TtlfDeposit ของดอกเบี้ยก่อน
                if (model.CDCode == "D" && model.OCFlag == "C" && model.AccInt > 0)
                {
                    BFBal = (decimal)model.LedgerBal;
                    Amt = (decimal)model.AccInt;
                    Cr = (decimal)model.AccInt;
                    Dr = 0;
                    CFBal = BFBal + Cr;
                    AcInt = 0;
                    // ปิดบัญชี ต้อง Log NoBook ของดอกเบี้ย
                    NoBookModel nbIntModel = new NoBookModel
                    {
                        Filestatus = "A",
                        CoopID = model.CoopID,
                        AccountNo = AccNo,
                        Seq = iSeq,
                        TxnDate = sysDate,
                        BackDate = DateLib.DateInCE(model.BackDateTH),
                        TTxnCode = "180",
                        AbbCode = "INT",
                        ItemNo = model.ItemNo,
                        CDCode = "C",
                        TxnAmt = Amt,
                        CfLedgerBal = CFBal,
                        ChequeAmt = 0,
                        Tax = 0,
                        CreatedBy = AuthorizeHelper.Current.UserAccount().UserID,
                        CreatedDate = DateTime.Now,
                        ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
                        ModifiedDate = DateTime.Now,
                    };
                    _unitOfWork.NoBook.Create(nbIntModel);
                    // ปิดบัญชี ต้อง Log TtlfDeposit ของดอกเบี้ย
                    TtlfDepositModel tModel = new TtlfDepositModel
                    {
                        CoopID = model.CoopID,
                        TxnDate = sysDate,
                        TxnSeq = txnSeq,
                        TxnTime = DateTime.Now,
                        //WorkstationID = model.WorkstationID,
                        //BranchId = model.BranchId,
                        UserID = AuthorizeHelper.Current.UserAccount().UserID,
                        //BudgetYear = AuthorizeHelper.Current.CoopControls().BudgetYear,
                        OriginalProcess = "OtxDeposit",
                        Filestatus = "A",
                        MemberID = model.MemberID,
                        DepositTypeID = model.DepositTypeID,
                        AccountNo = model.AccountNo,
                        BackDate = DateLib.DateInCE(model.TxnDateTH),
                        BFLedgerBal = BFBal,
                        Credit = Cr,
                        Debit = Dr,
                        CFLedgerBal = CFBal,
                        //Fee = model.Fee,
                        AccInt = AcInt,
                        ChargeAmt = 0,
                        IntDueAmt = 0,
                        //Tax = model.Tax,
                        ItemNo = model.ItemNo,
                        BookFlag = model.BookFlag,
                        ReferenceNo = model.ReferenceNo,
                        BudgetYear = AuthorizeHelper.Current.CoopControls().BudgetYear,
                        Type = model.DepositTypeID,
                        TTxnCode = "180",
                        CDCode = "C",
                        OCFlag = "C",
                        InstrumentType = model.InstrumentType
                    };
                    _unitOfWork.TtlfDeposit.LogTtlfDeposit(tModel);
                    BFBal = CFBal;
                    Amt = CFBal;
                    Cr = 0;
                    Dr = Amt;
                    CFBal = 0;
                    AcInt = 0;
                    txnSeq = txnSeq + 1;
                    iSeq = iSeq + 1;
                }
                // Log NoBook
                //_unitOfWork.NoBook.LogNoBook(FileStatus.A, otxDepModel.CoopID, otxDepModel.AccountNo, iSeq, DateLib.DateInCE(otxDepModel.TxnDateTH),
                //    DateLib.DateInCE(otxDepModel.TxnDateTH), otxDepModel.TTxnCode, otxDepModel.AbbCode, iTem, otxDepModel.CDCode,
                //    (decimal)otxDepModel.Amt, (decimal)otxDepModel.Amt, (decimal)otxDepModel.CFLedgerBal);
                NoBookModel nbModel = new NoBookModel
                {
                    Filestatus = "A",
                    CoopID = model.CoopID,
                    AccountNo = AccNo,
                    Seq = iSeq,
                    TxnDate = sysDate,
                    BackDate = DateLib.DateInCE(model.BackDateTH),
                    TTxnCode = model.TTxnCode,
                    ItemNo = model.ItemNo,
                    CDCode = model.CDCode,
                    TxnAmt = Amt,
                    CfLedgerBal = CFBal,
                    ChequeAmt = 0,
                    Tax = 0,
                    CreatedBy = AuthorizeHelper.Current.UserAccount().UserID,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
                    ModifiedDate = DateTime.Now,
                };
                _unitOfWork.NoBook.Create(nbModel);

                //_unitOfWork.TtlfDeposit.LogTtlfDeposit(otxDepModel.CoopID, DateLib.DateInCE(otxDepModel.TxnDateTH), txnSeq, "OtxDeposit", FileStatus.A, otxDepModel.MemberID,
                //    otxDepModel.DepositTypeID, AccNo, DateLib.DateInCE(otxDepModel.BackDateTH), (decimal)otxDepModel.LedgerBal, (decimal)otxDepModel.Credit, 
                //    (decimal)otxDepModel.Debit, (decimal)otxDepModel.CFLedgerBal, (decimal)otxDepModel.AccInt, (decimal)otxDepModel.ChargeAmt, (int)otxDepModel.ItemNo,
                //    otxDepModel.BookFlag, otxDepModel.ReferenceNo,otxDepModel.TTxnCode, otxDepModel.CDCode, otxDepModel.OCFlag, otxDepModel.InstrumentType);

                // Log TtlfDeposit
                TtlfDepositModel ttlfDepModel = new TtlfDepositModel
                {
                    CoopID = model.CoopID,
                    TxnDate = sysDate,
                    TxnSeq = txnSeq,
                    TxnTime = DateTime.Now,
                    //WorkstationID = model.WorkstationID,
                    //BranchId = model.BranchId,
                    OriginalProcess = "OtxDeposit",
                    Filestatus = "A",
                    MemberID = model.MemberID,
                    DepositTypeID = model.DepositTypeID,
                    AccountNo = model.AccountNo,
                    BackDate = DateLib.DateInCE(model.BackDateTH),
                    BFLedgerBal = BFBal,
                    Debit = Dr,
                    Credit = Cr,
                    CFLedgerBal = CFBal,
                    //Fee = model.Fee,
                    AccInt = AcInt,
                    ChargeAmt = model.ChargeAmt,
                    IntDueAmt = 0,
                    //Tax = model.Tax,
                    ItemNo = model.ItemNo,
                    BookFlag = model.BookFlag,
                    ReferenceNo = model.ReferenceNo,
                    BudgetYear = AuthorizeHelper.Current.CoopControls().BudgetYear,
                    Type = model.DepositTypeID,
                    TTxnCode = model.TTxnCode,
                    CDCode = model.CDCode,
                    OCFlag = model.OCFlag,
                    InstrumentType = model.InstrumentType
                };
                _unitOfWork.TtlfDeposit.LogTtlfDeposit(ttlfDepModel);

                dep.AccountNo = model.AccountNo;
                dep.Filestatus = model.Filestatus;
                dep.LedgerBal = model.CFLedgerBal;
                dep.AvailBal = model.CFLedgerBal;
                if (string.IsNullOrWhiteSpace(Convert.ToString(model.BackDateTH)))
                {
                    dep.LastCalcInt = DateLib.DateInCE(model.BackDateTH);
                    dep.LastContact = DateLib.DateInCE(model.BackDateTH);
                };
                dep.AccInt = model.AccInt;
                if (model.TypeOfDeposit == "SPC" && model.CDCode == "D")
                {
                    dep.MonthWithdrawAmt += model.Debit;
                    dep.MonthWithdrawTimes += 1;
                };
                if (model.CDCode == "C")
                {
                    if (model.TTxnCode == "21")     // Checque clearing
                    {
                        dep.Amt2 += model.Credit;
                    };
                    if (model.TTxnCode == "22")     // Checque late
                    {
                        dep.Amt3 += model.Credit;
                    };
                };
                using (TransactionScope tranScope = new TransactionScope())
                {
                    try
                    {
                        _unitOfWork.Deposit.Update(dep);
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