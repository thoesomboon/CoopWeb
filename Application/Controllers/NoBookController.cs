using Coop.Entities;
using Coop.Library;
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

namespace Coop.Controllers
{
    public class NoBookController : BaseController
    {
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;
        public NoBookController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorization]
        public ActionResult Index()
        {
            //ViewBag.DepositTypeList = new SelectList(GetNoBookTypeList(), "NoBookTypeID", "NoBookTypeName");
            //var model = new NoBookModel();
            //return View(model);
            return View();
        }
        [Authorization]
        [HttpPost]
        public ActionResult TransferInNoBook(NoBookModel nModel)
        {
            if (nModel == null || nModel.Equals(new NoBookModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new NoBookModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            NoBookModel model = nModel;

            /// check NoBook
            if (string.IsNullOrWhiteSpace(model.AccountNo))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Invalid NoBook Code" };
                return Json(new object[] { oper, new NoBookModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            /// check if exist
            //var mID = model.AccountNo;
            var pInfo = _unitOfWork.NoBook.ReadByID(model.AccountNo);

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
                msg = "NoBook Not Found";
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
        public ActionResult TransferOutNoBook(NoBookModel nModel)
        {
            if (nModel == null || nModel.Equals(new NoBookModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new NoBookModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            NoBookModel model = nModel;

            bool result = false;
            string msg = string.Empty;

            if (model.AccountNo != null)
            {
                /// Account Exist => update one
                var uNoBook = _unitOfWork.NoBook.ReadBySeq(model.AccountNo, model.Seq).FirstOrDefault();

                if (uNoBook != null && !string.IsNullOrWhiteSpace(uNoBook.AccountNo))
                {
                    uNoBook.AccountNo = model.AccountNo; /// primary key ==> identity
                    uNoBook.Filestatus = model.Filestatus;
                    uNoBook.CoopID = model.CoopID;
                    uNoBook.AccountNo = model.AccountNo;
                    uNoBook.Seq = model.Seq;
                    uNoBook.TxnDate = model.TxnDate;
                    uNoBook.BackDate = model.BackDate;
                    uNoBook.TTxnCode = model.TTxnCode;
                    uNoBook.AbbCode = model.AbbCode;
                    uNoBook.ItemNo = model.ItemNo;
                    uNoBook.CDCode = model.CDCode;
                    uNoBook.TxnAmt = model.TxnAmt;
                    uNoBook.CfLedgerBal = model.CfLedgerBal;
                    uNoBook.ChequeAmt = model.ChequeAmt;
                    uNoBook.Tax = model.Tax;
                    uNoBook.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
                    uNoBook.ModifiedDate = DateTime.Now;

                    using (TransactionScope tranScope = new TransactionScope())
                    {
                        try
                        {
                            _unitOfWork.NoBook.Update(uNoBook);

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
                NoBookModel mModel = new NoBookModel
                {
                    Filestatus = model.Filestatus,
                    CoopID = model.CoopID,
                    AccountNo = model.AccountNo,
                    Seq = model.Seq,
                    TxnDate = model.TxnDate,
                    BackDate = model.BackDate,
                    TTxnCode = model.TTxnCode,
                    AbbCode = model.AbbCode,
                    ItemNo = model.ItemNo,
                    CDCode = model.CDCode,
                    TxnAmt = model.TxnAmt,
                    CfLedgerBal = model.CfLedgerBal,
                    ChequeAmt = model.ChequeAmt,
                    Tax = model.Tax,
                    CreatedBy = AuthorizeHelper.Current.UserAccount().UserID,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
                    ModifiedDate = DateTime.Now,
                };

                var objCreate = _unitOfWork.NoBook.Create(mModel);
                if (objCreate != null && !objCreate.Equals(new NoBookModel()))
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
        public ActionResult LogNoBookOpen(OpenDepositModel nModel)
        {
            if (nModel == null || nModel.Equals(new NoBookModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new NoBookModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            OpenDepositModel model = nModel;

            bool result = false;
            string msg = string.Empty;

            if (model.AccountNo != null)
            {
                /// Account Exist => update one
                var uNoBook = _unitOfWork.NoBook.ReadBySeqDesc(model.AccountNo).FirstOrDefault();
                int iSeq = 1;
                if (uNoBook != null)
                {
                    iSeq = uNoBook.Seq + 1;
                };

                var txnCode = _unitOfWork.TxnCode.ReadDetailByType("DEP", model.TTxnCode).FirstOrDefault();

                //var depType = _unitOfWork.DepositType.ReadDetail(model.DepositTypeID).FirstOrDefault();
                //int iItem = 0;
                //if (depType != null && depType.ItemStatus == true)
                //{
                //    iItem = 1;
                //};
                NoBookModel nbModel = new NoBookModel
                {
                    Filestatus = "A",
                    CoopID = model.CoopID,
                    AccountNo = model.AccountNo,
                    Seq = iSeq,
                    TxnDate = DateLib.DateInCE(model.TxnDateTH),
                    BackDate = DateLib.DateInCE(model.TxnDateTH),
                    TTxnCode = model.TTxnCode,
                    AbbCode = txnCode.AbbCode,
                    ItemNo = model.ItemNo,
                    CDCode = model.CDCode,
                    TxnAmt = model.Amt,
                    CfLedgerBal = model.Amt,
                    ChequeAmt = 0,
                    Tax = 0,
                    CreatedBy = AuthorizeHelper.Current.UserAccount().UserID,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
                    ModifiedDate = DateTime.Now,
                };

                var objCreate = _unitOfWork.NoBook.Create(nbModel);
                if (objCreate != null && !objCreate.Equals(new NoBookModel()))
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
            object[] retObj = new object[] { oResult, nModel };

            return Json(retObj, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}
