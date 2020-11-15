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
    public class BatMthLoanController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BatMthLoanController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //
        // GET: /BatMthLoan/
        public ActionResult Index()
        {
            ViewBag.LoanTypeList = new SelectList(GetLoanTypeList(), "LoanTypeID", "LoanTypeName");
            return View();
        }
        public List<LoanType> GetLoanTypeList()
        {
            CoopWebEntities db = new CoopWebEntities();
            //List<LoanType> LoanTypes = db.LoanType.ToList();
            List<LoanType> LoanTypes = db.LoanType.Where(d => d.Filestatus == "A").ToList();
            return LoanTypes;
        }
        [Authorization]
        public ActionResult GetParam(BatMthLoanModel batModel)
        {
            if (batModel == null)
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new BatMthLoanModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            bool result = false;
            string msg = string.Empty;

            var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();

            var model = new BatMthLoanModel
            {
                //LoanTypeID = batModel.LoanTypeID,
                CoopID = coopData.CoopID,
                BudgetYear = coopData.BudgetYear,
                Period = (int)coopData.AccountPeriod,
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
        public JsonResult ProcessBatMthLoanBal(int CoopId, string BudgetYear, int Period)
        {
            var userId = AuthorizeHelper.Current.UserAccount().UserID;
            //_unitOfWork.MonthBalanceLoan.BatMthLoanBal(coopId, dTypeID, (DateTime)sDate, (DateTime)eDate, userId, budgetYear, period);
            _unitOfWork.Loan.Sp_BatMthLoanBal(CoopId, userId, BudgetYear, Period);
            //_unitOfWork.MonthBalanceLoan.sp_BatMthLoanBal(coopId, userId, budgetYear, period);
            return Json("ประมวณผลเสร็จแล้ว", JsonRequestBehavior.AllowGet);
        }
    }}
