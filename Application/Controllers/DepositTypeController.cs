using Coop.Entities;
//using Coop.Controllers;
using Coop.Infrastructure.ActionFilters;
using Coop.Infrastructure.Helpers;
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
    public class DepositTypeController : Controller
    {
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;
        public DepositTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: DepositType
        [Authorization]
        [HttpPost]
        // GET: DepositType
        public ActionResult TransferInDepositType(DepositTypeModel DepTypeModel)
        {
            if (DepTypeModel == null || DepTypeModel.Equals(new DepositTypeModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new DepositTypeModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            DepositTypeModel model = DepTypeModel;

            /// check Deposit
            if (string.IsNullOrWhiteSpace(model.DepositTypeID))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Invalid Deposit Code" };
                return Json(new object[] { oper, new DepositTypeModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            var depTypeID = model.DepositTypeID;
            var pInfo = _unitOfWork.DepositType.ReadDetail(depTypeID).FirstOrDefault();

            bool result = false;
            string msg = string.Empty;

            /// Account Exist
            if (pInfo != null)
            {
                result = true;
            }
            else
            {
                result = false;
                msg = "DepositType Not Found";
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
            object[] retObj = new object[] { oResult, pInfo };

            return Json(retObj, "application/json", JsonRequestBehavior.AllowGet);
        }
        public ActionResult TransferOutDepositType(DepositTypeModel DepTypeModel)
        {
            if (DepTypeModel == null || DepTypeModel.Equals(new DepositTypeModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new DepositTypeModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            DepositTypeModel model = DepTypeModel;
            var DepTypeID = model.DepositTypeID;

            bool result = false;
            string msg = string.Empty;

            var uDepositType = _unitOfWork.DepositType.ReadDetail(DepTypeID).FirstOrDefault();

            if (uDepositType != null)
            {
                /// Account Exist => update one

                if (uDepositType != null && !string.IsNullOrWhiteSpace(uDepositType.DepositTypeID))
                {
                    uDepositType.Filestatus = model.Filestatus;
                    uDepositType.CoopID = model.CoopID;
                    uDepositType.DepositTypeID = model.DepositTypeID;
                    uDepositType.DepositTypeName = model.DepositTypeName;
                    uDepositType.TypeOfDeposit = model.TypeOfDeposit;
                    uDepositType.MinOpenAmt = model.MinOpenAmt;
                    uDepositType.MaxOpenAmt = model.MaxOpenAmt;
                    uDepositType.ItemStatus = model.ItemStatus;
                    uDepositType.MinDepAmt = model.MinDepAmt;
                    uDepositType.MaxDepAmt = model.MaxDepAmt;
                    uDepositType.MinBalCalcInt = model.MinBalCalcInt;
                    uDepositType.MinWithdrawAmt = model.MinWithdrawAmt;
                    uDepositType.MaxWithdrawAmt = model.MaxWithdrawAmt;
                    uDepositType.MinLedgerBal = model.MinLedgerBal;
                    uDepositType.MonthMaxWithdrawAmt = model.MonthMaxWithdrawAmt;
                    uDepositType.MonthMaxWithdrawTimes = model.MonthMaxWithdrawTimes;
                    uDepositType.WithdrawChargePercent = model.WithdrawChargePercent;
                    uDepositType.MaxChargeAmt = model.MaxChargeAmt;
                    uDepositType.MinChargeAmt = model.MinChargeAmt;
                    uDepositType.LastAccountNo = model.LastAccountNo;
                    uDepositType.LastBookNo = model.LastBookNo;
                    uDepositType.CloseAccountFee = model.CloseAccountFee;
                    uDepositType.MonthIntDue = model.MonthIntDue;
                    uDepositType.CalcIntRate = model.CalcIntRate;
                    uDepositType.CalcIntType = model.CalcIntType;

                    uDepositType.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
                    uDepositType.ModifiedDate = DateTime.Now;

                    using (TransactionScope tranScope = new TransactionScope())
                    {
                        try
                        {
                            _unitOfWork.DepositType.Update(uDepositType);

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
            else
            {
                /// Create new
                DepositTypeModel mModel = new DepositTypeModel
                {
                    Filestatus = model.Filestatus,
                    CoopID = model.CoopID,
                    DepositTypeID = model.DepositTypeID,
                    DepositTypeName = model.DepositTypeName,
                    TypeOfDeposit = model.TypeOfDeposit,
                    MinOpenAmt = model.MinOpenAmt,
                    MaxOpenAmt = model.MaxOpenAmt,
                    ItemStatus = model.ItemStatus,
                    MinDepAmt = model.MinDepAmt,
                    MaxDepAmt = model.MaxDepAmt,
                    MinBalCalcInt = model.MinBalCalcInt,
                    MinWithdrawAmt = model.MinWithdrawAmt,
                    MaxWithdrawAmt = model.MaxWithdrawAmt,
                    MinLedgerBal = model.MinLedgerBal,
                    MonthMaxWithdrawAmt = model.MonthMaxWithdrawAmt,
                    MonthMaxWithdrawTimes = model.MonthMaxWithdrawTimes,
                    WithdrawChargePercent = model.WithdrawChargePercent,
                    MaxChargeAmt = model.MaxChargeAmt,
                    MinChargeAmt = model.MinChargeAmt,
                    LastAccountNo = model.LastAccountNo,
                    LastBookNo = model.LastBookNo,
                    CloseAccountFee = model.CloseAccountFee,
                    MonthIntDue = model.MonthIntDue,
                    CalcIntRate = model.CalcIntRate,
                    CalcIntType = model.CalcIntType,

                    CreatedBy = AuthorizeHelper.Current.UserAccount().UserID,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
                    ModifiedDate = DateTime.Now
                };

                var objCreate = _unitOfWork.DepositType.Create(mModel);
                if (objCreate != null && !objCreate.Equals(new DepositTypeModel()))
                {
                    model.DepositTypeID = objCreate.DepositTypeID;
                    result = true;
                }
                else
                {
                    result = false;
                    msg = "Data Not Found";
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
        public ActionResult UpdateIssueDepositTypeIDX(OpenDepositModel OpenDepTypeModel)
        {
            if (OpenDepTypeModel == null || OpenDepTypeModel.Equals(new DepositTypeModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new DepositTypeModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            OpenDepositModel model = OpenDepTypeModel;
            var id = model.DepositTypeID;

            bool result = false;
            string msg = string.Empty;

            var uDepositType = _unitOfWork.DepositType.ReadDetail(id).FirstOrDefault();

            if (uDepositType != null)
            {
                /// update one
                uDepositType.LastAccountNo = Convert.ToInt32(model.DepositTypeID);
                uDepositType.LastBookNo = Convert.ToInt32(model.BookNo);

                using (TransactionScope tranScope = new TransactionScope())
                {
                    try
                    {
                        _unitOfWork.DepositType.UpdateIssueAccNo(uDepositType);
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

        //public ActionResult UpdateIssueAccNo(string depTypeID, string DepTypeID, string bookNo)
        //{
        //    if (string.IsNullOrWhiteSpace(depTypeID))
        //    {
        //        return Json(null, JsonRequestBehavior.AllowGet);
        //    }

        //    bool result = false;
        //    string msg = string.Empty;

        //    var uDepositType = _unitOfWork.DepositType.ReadDetail(depTypeID).FirstOrDefault();

        //    if (uDepositType != null)
        //    {
        //        /// Account Exist => update one

        //        if (uDepositType != null && !string.IsNullOrWhiteSpace(uDepositType.DepositTypeID))
        //        {
        //            uDepositType.LastAccountNo = (Int32.Parse(DepTypeID) / 10) + 1;
        //            uDepositType.LastBookNo = Int32.Parse(bookNo);

        //            uDepositType.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
        //            uDepositType.ModifiedDate = DateTime.Now;

        //            using (TransactionScope tranScope = new TransactionScope())
        //            {
        //                try
        //                {
        //                    _unitOfWork.DepositType.UpdateIssueAccNo(uDepositType);

        //                    tranScope.Complete();
        //                    result = true;
        //                }
        //                catch (Exception exception)
        //                {
        //                    result = false;
        //                    msg = "Transaction Roll backed due to some exception:" + exception.Message;
        //                }
        //            }
        //        }
        //    }
        //    OperationResult oResult = new OperationResult();
        //    if (result)
        //    {
        //        oResult.Result = result;
        //        oResult.Message = "Successful";
        //    }
        //    else
        //    {
        //        oResult.Result = result;
        //        oResult.Message = msg;
        //    }
        //    object[] retObj = new object[] { oResult, uDepositType };

        //    return Json(retObj, "application/json", JsonRequestBehavior.AllowGet);
        //}
    }
}
