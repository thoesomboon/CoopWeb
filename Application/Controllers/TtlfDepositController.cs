using Coop.Entities;
using Coop.Infrastructure.ActionFilters;
using Coop.Infrastructure.Helpers;
using Coop.Library;
using Coop.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace Coop.Controllers
{
    public class TtlfDepositController : BaseController
    {
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;
        public TtlfDepositController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorization]
        //[HttpPost]

        public ActionResult LogTtlfDepositOpen(OpenDepositModel OpenDepModel)
        {
            if (OpenDepModel == null || OpenDepModel.Equals(new DepositTypeModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new DepositTypeModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            OpenDepositModel model = OpenDepModel;
            var AccNo = model.AccountNo;

            bool result = false;
            string msg = string.Empty;

            var Deposit = _unitOfWork.Deposit.ReadDetail(AccNo).FirstOrDefault();
            if (Deposit != null)
            {
                var sysDate = AuthorizeHelper.Current.CoopControls().SystemDate;
                var txnSeq = 1;
                var ttlfDeposit = _unitOfWork.TtlfDeposit.ReadDetail(AccNo, (DateTime)sysDate).FirstOrDefault();
                if (ttlfDeposit != null)
                {
                    txnSeq = ttlfDeposit.TxnSeq + 1;
                }
                var txnCode = _unitOfWork.TxnCode.ReadDetailByType( "DEP", model.TTxnCode).FirstOrDefault();

                TtlfDepositModel tModel = new TtlfDepositModel
                {
                    CoopID = OpenDepModel.CoopID,
                    TxnDate = DateLib.DateInCE(OpenDepModel.TxnDateTH),
                    TxnSeq = txnSeq,
                    TxnTime = DateTime.Now,
                    //WorkstationID = OpenDepModel.WorkstationID,
                    //BranchId = OpenDepModel.BranchId,
                    OriginalProcess = "OtxDeposit",
                    Filestatus = "A",
                    MemberID = OpenDepModel.MemberID,
                    DepositTypeID = OpenDepModel.DepositTypeID,
                    AccountNo = OpenDepModel.AccountNo,
                    BackDate = DateLib.DateInCE(OpenDepModel.TxnDateTH),
                    BFLedgerBal = 0,
                    Debit = 0,
                    Credit = OpenDepModel.Amt,
                    CFLedgerBal = OpenDepModel.Amt,
                    Fee = OpenDepModel.Fee,
                    AccInt = 0,
                    ChargeAmt = 0,
                    IntDueAmt = 0,
                    //Tax = OpenDepModel.Tax,
                    ItemNo = OpenDepModel.ItemNo,
                    BookFlag = OpenDepModel.BookFlag,
                    ReferenceNo = OpenDepModel.ReferenceNo,
                    BudgetYear = AuthorizeHelper.Current.CoopControls().BudgetYear,
                    Type = OpenDepModel.DepositTypeID,
                    TTxnCode = OpenDepModel.TTxnCode,
                    CDCode = txnCode.CDCode,
                    OCFlag = txnCode.OCFlag,
                    InstrumentType = txnCode.InstrumentType
                };
                using (TransactionScope tranScope = new TransactionScope())
                {
                    try
                    {
                        _unitOfWork.TtlfDeposit.LogTtlfDeposit(tModel);
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
        public ActionResult LogTtlfDepositOtx(OtxDepositModel OtxDepModel)
        {
            if (OtxDepModel == null || OtxDepModel.Equals(new DepositTypeModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new DepositTypeModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            OtxDepositModel model = OtxDepModel;
            var AccNo = model.AccountNo;

            bool result = false;
            string msg = string.Empty;

            var Deposit = _unitOfWork.Deposit.ReadDetail(AccNo).FirstOrDefault();
            if (Deposit != null)
            {
                var sysDate = AuthorizeHelper.Current.CoopControls().SystemDate;
                var txnSeq = 1;
                var ttlfDeposit = _unitOfWork.TtlfDeposit.ReadDetail(AccNo, (DateTime)sysDate).FirstOrDefault();
                if (ttlfDeposit != null) { 
                    txnSeq = ttlfDeposit.TxnSeq + 1; }

                var txnCode = _unitOfWork.TxnCode.ReadDetailByType("DEP", model.TTxnCode).FirstOrDefault();

                TtlfDepositModel tModel = new TtlfDepositModel
                {
                    CoopID = OtxDepModel.CoopID,
                    TxnDate = DateLib.DateInCE(OtxDepModel.TxnDateTH),
                    TxnSeq = txnSeq,
                    TxnTime = DateTime.Now,
                    //WorkstationID = OtxDepModel.WorkstationID,
                    //BranchId = OtxDepModel.BranchId,
                    OriginalProcess = "OtxDeposit",
                    Filestatus = OtxDepModel.Filestatus,
                    MemberID = OtxDepModel.MemberID,
                    DepositTypeID = OtxDepModel.DepositTypeID,
                    AccountNo = OtxDepModel.AccountNo,
                    BackDate = DateLib.DateInCE(OtxDepModel.BackDateTH),
                    BFLedgerBal = OtxDepModel.BFLedgerBal,
                    Debit = OtxDepModel.Debit,
                    Credit = OtxDepModel.Credit,
                    CFLedgerBal = OtxDepModel.CFLedgerBal,
                    //Fee = OtxDepModel.Fee,
                    AccInt = OtxDepModel.AccInt,
                    ChargeAmt = OtxDepModel.ChargeAmt,
                    IntDueAmt = OtxDepModel.IntDueAmt,
                    //Tax = OtxDepModel.Tax,
                    ItemNo = OtxDepModel.ItemNo,
                    BookFlag = OtxDepModel.BookFlag,
                    ReferenceNo = OtxDepModel.ReferenceNo,
                    BudgetYear = AuthorizeHelper.Current.CoopControls().BudgetYear,
                    Type = OtxDepModel.DepositTypeID,
                    TTxnCode = OtxDepModel.TTxnCode,
                    ECFlag = OtxDepModel.ECFlag,
                    //OverrideID = OtxDepModel.OverrideID,
                    ChequeDate = OtxDepModel.ChequeDate,
                    BankID = OtxDepModel.BankID,
                    //ChequeAmt = OtxDepModel.ChequeAmt,
                    ////ClearingFlag = OtxDepModel.ClearingFlag,
                    ////ClearingDate = OtxDepModel.ClearingDate,
                    CDCode = txnCode.CDCode,
                    OCFlag = txnCode.OCFlag,
                    InstrumentType = txnCode.InstrumentType
                };
                using (TransactionScope tranScope = new TransactionScope())
                {
                    try
                    {
                        _unitOfWork.TtlfDeposit.LogTtlfDeposit(tModel);
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