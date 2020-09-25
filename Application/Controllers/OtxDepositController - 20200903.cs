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

                NoBookModel nbModel = new NoBookModel
                {
                    Filestatus = "A",
                    CoopID = otxDepModel.CoopID,
                    AccountNo = AccNo,
                    Seq = iSeq,
                    TxnDate = DateLib.DateInCE(otxDepModel.TxnDateTH),
                    BackDate = DateLib.DateInCE(otxDepModel.TxnDateTH),
                    TTxnCode = otxDepModel.TTxnCode,
                    ItemNo = otxDepModel.ItemNo,
                    CDCode = otxDepModel.CDCode,
                    TxnAmt = otxDepModel.Amt,
                    CfLedgerBal = otxDepModel.CFLedgerBal,
                    ChequeAmt = 0,
                    Tax = 0,
                    CreatedBy = AuthorizeHelper.Current.UserAccount().UserID,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
                    ModifiedDate = DateTime.Now,
                };
                _unitOfWork.NoBook.Create(nbModel);

                // Log TtlfDeposit
                TtlfDepositModel tModel = new TtlfDepositModel
                {
                    CoopID = otxDepModel.CoopID,
                    TxnDate = sysDate,
                    TxnSeq = txnSeq,
                    TxnTime = DateTime.Now,
                    //WorkstationID = otxDepModel.WorkstationID,
                    //BranchId = otxDepModel.BranchId,
                    OriginalProcess = "OtxDeposit",
                    Filestatus = "A",
                    MemberID = otxDepModel.MemberID,
                    DepositTypeID = otxDepModel.DepositTypeID,
                    AccountNo = AccNo,
                    BackDate = DateLib.DateInCE(otxDepModel.TxnDateTH),
                    BFLedgerBal = otxDepModel.LedgerBal,
                    Debit = otxDepModel.Debit,
                    Credit = otxDepModel.Credit,
                    CFLedgerBal = otxDepModel.CFLedgerBal,
                    //Fee = otxDepModel.Fee,
                    AccInt = 0,
                    ChargeAmt = 0,
                    IntDueAmt = 0,
                    //Tax = otxDepModel.Tax,
                    ItemNo = otxDepModel.ItemNo,
                    BookFlag = otxDepModel.BookFlag,
                    ReferenceNo = otxDepModel.ReferenceNo,
                    BudgetYear = AuthorizeHelper.Current.CoopControls().BudgetYear,
                    Type = otxDepModel.DepositTypeID,
                    TTxnCode = otxDepModel.TTxnCode,
                    CDCode = otxDepModel.CDCode,
                    OCFlag = otxDepModel.OCFlag,
                    InstrumentType = otxDepModel.InstrumentType
                };
                _unitOfWork.TtlfDeposit.LogTtlfDeposit(tModel);

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
                    dep.MonthWithdrawAmt = dep.MonthWithdrawAmt ?? 0 + model.Debit;
                    dep.MonthWithdrawTimes = dep.MonthWithdrawTimes ?? 0 + 1;
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