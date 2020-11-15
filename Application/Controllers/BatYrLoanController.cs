using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using Coop.Library;
using Coop.Infrastructure.ActionFilters;

namespace Coop.Controllers
{
    public class BatYrLoanController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BatYrLoanController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //
        // GET: /BatYrLoan/
        public ActionResult Index()
        {
            //ViewBag.LoanTypeList = new SelectList(GetLoanTypeList(), "LoanTypeID", "LoanTypeName");
            return View();
        }
        [Authorization]
        public ActionResult GetParam(BatYrLoanModel batModel)
        {
            if (batModel == null)
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new BatYrLoanModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            bool result = false;
            string msg = string.Empty;

            var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();

            var model = new BatYrLoanModel
            {
                //LoanTypeID = batModel.LoanTypeID,
                CoopID = coopData.CoopID,
                BudgetYear = coopData.BudgetYear,
                Period1 = 1,
                Period2 = (int)coopData.AccountPeriod,
                UserId = AuthorizeHelper.Current.UserAccount().UserID
            };
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
        public JsonResult ProcessBatYrLoanBal(int CoopId, string BudgetYear, int Period1, int Period2)
        {
            var userId = AuthorizeHelper.Current.UserAccount().UserID;
            //_unitOfWork.MonthBalanceLoan.BatYrLoanBal(coopId, dTypeID, (DateTime)sDate, (DateTime)eDate, userId, budgetYear, period);
            _unitOfWork.Loan.Sp_BatYrLoanBal(CoopId, userId, BudgetYear, Period1, Period2);
            //_unitOfWork.MonthBalanceLoan.sp_BatYrLoanBal(coopId, userId, budgetYear, period);
            return Json("ประมวณผลเสร็จแล้ว", JsonRequestBehavior.AllowGet);
        }
    }
}
