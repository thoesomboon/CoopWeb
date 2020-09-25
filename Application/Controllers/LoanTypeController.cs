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
    public class LoanTypeController : Controller
    {
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;
        public LoanTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: LoanType
        [Authorization]
        // GET: LoanType
        public ActionResult ReadLoanType(string lonTypeID)
        {
            var pInfo = _unitOfWork.LoanType.ReadDetail(lonTypeID).FirstOrDefault();

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
                msg = "LoanType Not Found";
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
        public ActionResult UpdateIssueLoanID(string lonTypeID, string lonID)
        {
            if (string.IsNullOrWhiteSpace(lonTypeID))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            bool result = false;
            string msg = string.Empty;

            var uLoanType = _unitOfWork.LoanType.ReadDetail(lonTypeID).FirstOrDefault();

            if (uLoanType != null)
            {
                /// Account Exist => update one

                if (uLoanType != null && !string.IsNullOrWhiteSpace(uLoanType.LoanTypeID))
                {
                    uLoanType.LastLoanID = Int32.Parse(lonID) + 1;

                    uLoanType.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
                    uLoanType.ModifiedDate = DateTime.Now;

                    using (TransactionScope tranScope = new TransactionScope())
                    {
                        try
                        {
                            //_unitOfWork.LoanType.UpdateIssuelonID(uLoanType);

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
            object[] retObj = new object[] { oResult, uLoanType };

            return Json(retObj, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}
