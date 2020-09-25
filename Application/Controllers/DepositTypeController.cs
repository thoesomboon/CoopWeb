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
        // GET: DepositType
        public ActionResult ReadDepositType(string depTypeID)
        {
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
        public ActionResult UpdateIssueAccountNoX(OpenDepositModel OpenDepModel)
        {
            if (OpenDepModel == null || OpenDepModel.Equals(new DepositTypeModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new DepositTypeModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            OpenDepositModel model = OpenDepModel;
            var id = model.DepositTypeID;

            bool result = false;
            string msg = string.Empty;

            var uDepositType = _unitOfWork.DepositType.ReadDetail(id).FirstOrDefault();

            if (uDepositType != null)
            {
                /// update one
                uDepositType.LastAccountNo = Convert.ToInt32(model.AccountNo);
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

        public ActionResult UpdateIssueAccountNo(string depTypeID, string accNo, string bookNo)
        {
            if (string.IsNullOrWhiteSpace(depTypeID))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            bool result = false;
            string msg = string.Empty;

            var uDepositType = _unitOfWork.DepositType.ReadDetail(depTypeID).FirstOrDefault();

            if (uDepositType != null)
            {
                /// Account Exist => update one

                if (uDepositType != null && !string.IsNullOrWhiteSpace(uDepositType.DepositTypeID))
                {
                    uDepositType.LastAccountNo = (Int32.Parse(accNo) / 10) + 1;
                    uDepositType.LastBookNo = Int32.Parse(bookNo);

                    uDepositType.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
                    uDepositType.ModifiedDate = DateTime.Now;

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
            object[] retObj = new object[] { oResult, uDepositType };

            return Json(retObj, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}
