using Coop.Entities;
using Coop.Infrastructure.ActionFilters;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Coop.Library;

namespace Coop.Controllers
{
    public class DepositController : Controller
    {
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;
        public DepositController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            ViewBag.DepositTypeList = new SelectList(GetDepositTypeList(), "DepositTypeID", "DepositTypeName");
            return View();
        }
        public List<DepositType> GetDepositTypeList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<DepositType> DepositTypes = db.DepositType.ToList();
            return DepositTypes;
        }
        public ActionResult TransferInDeposit(DepositModel DepModel)
        {
            if (DepModel == null || DepModel.Equals(new DepositModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new DepositModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            DepositModel model = DepModel;

            /// check Deposit
            if (string.IsNullOrWhiteSpace(model.AccountNo))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Invalid Deposit Code" };
                return Json(new object[] { oper, new DepositModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            /// check if exist
            var mID = model.AccountNo;

            /// get Bank Account No.
            /// 
            //var depositData = _unitOfWork.Deposit.ReadDetail(pCode).FirstOrDefault();
            var depositData = _unitOfWork.Deposit.ReadDeposit(mID);

            bool result = false;
            string msg = string.Empty;

            /// Account Exist
            if (depositData != null && !string.IsNullOrWhiteSpace(depositData.AccountNo))
            {
                result = true;
            }
            else
            {
                result = false;
                msg = "ไม่พบบัญชีเงินฝาก : " + model.AccountNo;
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
        [Authorization]
        public ActionResult TransferOutDeposit(DepositModel DepModel)
        {
            if (DepModel == null || DepModel.Equals(new DepositModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new DepositModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            DepositModel model = DepModel;
            var AccNo = model.AccountNo;

            bool result = false;
            string msg = string.Empty;

            var uDeposit = _unitOfWork.Deposit.ReadDetail(AccNo).FirstOrDefault();

            if (uDeposit != null)
            {
                /// Account Exist => update one

                if (uDeposit != null && !string.IsNullOrWhiteSpace(uDeposit.AccountNo))
                {
                    uDeposit.Filestatus = model.Filestatus;
                    uDeposit.CoopID = model.CoopID;
                    uDeposit.AccountNo = model.AccountNo;
                    uDeposit.DepositTypeID = model.DepositTypeID;
                    uDeposit.MemberID = model.MemberID;
                    uDeposit.AccountName = model.AccountName;
                    uDeposit.BookNo = model.BookNo;

                    //uDeposit.OpenDate = Coop.Library.Deposit.DateMDY(model.OpenDate);

                    uDeposit.OpenDate = DateLib.DateInCE(model.OpenDateTH);
                    uDeposit.LastContact = DateLib.DateInCE(model.LastContactTH);
                    uDeposit.LastCalcInt = DateLib.DateInCE(model.LastCalcIntTH);

                    uDeposit.IntType = model.IntType;
                    uDeposit.BFLedgerBal = model.BFLedgerBal;
                    uDeposit.LedgerBal = model.LedgerBal;
                    uDeposit.AvailBal = model.AvailBal;
                    uDeposit.BookBal = model.BookBal;
                    uDeposit.AccInt = model.AccInt;
                    uDeposit.LastLedgerLine = model.LastLedgerLine;
                    uDeposit.LastBookLine = model.LastBookLine;
                    uDeposit.BookPage = model.BookPage;
                    uDeposit.HoldTypeID = model.HoldTypeID;
                    uDeposit.HoldAmt = model.HoldAmt;
                    uDeposit.IntDueAmt = model.IntDueAmt;
                    uDeposit.BudgetYear = model.BudgetYear;
                    uDeposit.UnpayInt = model.UnpayInt;
                    uDeposit.BookSeq = model.BookSeq;
                    uDeposit.MonthDepAmt = model.MonthDepAmt;
                    uDeposit.MonthDepositDate = model.MonthDepositDate;
                    uDeposit.MonthWithdrawAmt = model.MonthWithdrawAmt;
                    uDeposit.MonthWithdrawTimes = model.MonthWithdrawTimes;
                    uDeposit.Amt1 = model.Amt1;
                    uDeposit.Amt2 = model.Amt2;
                    uDeposit.Amt3 = model.Amt3;

                    //var MDY = Coop.Library.Deposit.DateMDY((DateTime)model.OpenDate);
                    ////uDeposit.OpenDate = Convert.ToDateTime(MDY);
                    ////uDeposit.OpenDate = MDY;
                    ////uDeposit.LastContact = model.LastContact;
                    ////uDeposit.LastCalcInt = model.LastCalcInt;

                    uDeposit.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
                    uDeposit.ModifiedDate = DateTime.Now;

                    using (TransactionScope tranScope = new TransactionScope())
                    {
                        try
                        {
                            _unitOfWork.Deposit.Update(uDeposit);

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
                DepositModel mModel = new DepositModel
                {
                    Filestatus = model.Filestatus,
                    CoopID = model.CoopID,
                    AccountNo = model.AccountNo,
                    DepositTypeID = model.DepositTypeID,
                    MemberID = model.MemberID,
                    AccountName = model.AccountName,
                    BookNo = model.BookNo,
                    OpenDate = model.OpenDate,
                    LastContact = model.LastContact,
                    LastCalcInt = model.LastCalcInt,
                    IntType = model.IntType,
                    BFLedgerBal = model.BFLedgerBal,
                    LedgerBal = model.LedgerBal,
                    AvailBal = model.AvailBal,
                    BookBal = model.BookBal,
                    AccInt = model.AccInt,
                    //LastLedgerLine = model.LastLedgerLine,
                    LastBookLine = model.LastBookLine,
                    BookPage = model.BookPage,
                    HoldTypeID = model.HoldTypeID,
                    HoldAmt = model.HoldAmt,
                    //IntDueAmt = model.IntDueAmt,
                    BudgetYear = model.BudgetYear,
                    //UnpayInt = model.UnpayInt,
                    BookSeq = model.BookSeq,
                    MonthDepAmt = model.MonthDepAmt,
                    MonthDepositDate = model.MonthDepositDate,
                    MonthWithdrawAmt = model.MonthWithdrawAmt,
                    MonthWithdrawTimes = model.MonthWithdrawTimes,
                    Amt1 = model.Amt1,
                    Amt2 = model.Amt2,
                    Amt3 = model.Amt3,

                    CreatedBy = AuthorizeHelper.Current.UserAccount().UserID,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
                    ModifiedDate = DateTime.Now
                };

                var objCreate = _unitOfWork.Deposit.Create(mModel);
                if (objCreate != null && !objCreate.Equals(new DepositModel()))
                {
                    model.AccountNo = objCreate.AccountNo;
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

        //public DateTime DateInCE(string DateIN)
        //{
        //    if (string.IsNullOrEmpty(DateIN) || DateIN.Length < 8)
        //    {
        //        return AuthorizeHelper.Current.CoopControls().SystemDate ?? DateTime.Now;
        //    }
        //    var year = Convert.ToInt32((DateIN.Split('/')[2])) - 543;
        //    var month = Convert.ToInt32(DateIN.Split('/')[1]);
        //    var day = Convert.ToInt32(DateIN.Split('/')[0]);
        //    var date = new DateTime(year, month, day);
        //    return date;
        //}
    }
}
