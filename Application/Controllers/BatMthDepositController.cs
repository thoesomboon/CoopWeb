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
    public class BatMthDepositController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BatMthDepositController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //
        // GET: /BatMthDeposit/
        public ActionResult Index()
        {
            ViewBag.DepositTypeList = new SelectList(GetDepositTypeList(), "DepositTypeID", "DepositTypeName");
            return View();
        }
        public List<DepositType> GetDepositTypeList()
        {
            CoopWebEntities db = new CoopWebEntities();
            //List<DepositType> DepositTypes = db.DepositType.ToList();
            List<DepositType> DepositTypes = db.DepositType.Where(d => d.Filestatus == "A").ToList();
            return DepositTypes;
        }
        [Authorization]
        public ActionResult GetParam(BatMthDepositModel batModel)
        {
            if (batModel == null)
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new BatMthDepositModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            bool result = false;
            string msg = string.Empty;

            var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();

            var model = new BatMthDepositModel
            {
                //DepositTypeID = batModel.DepositTypeID,
                CoopID = coopData.CoopID,
                BudgetYear = coopData.BudgetYear,
                Period = (int)coopData.AccountPeriod,

                //StartDate = Library.DateLib.FirstDateOfMonth(Convert.ToString(AuthorizeHelper.Current.CoopControls().SystemDate)),
                //EndDate = Library.DateLib.FirstDateOfMonth(Convert.ToString(AuthorizeHelper.Current.CoopControls().SystemDate)),
                //StartDate = stDate,
                //EndDate = enDate,

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
        public JsonResult ProcessBatMthDepositBal(int coopId, string DepositTypeID, string budgetYear, int period)
        {
            var userId = AuthorizeHelper.Current.UserAccount().UserID;
            //_unitOfWork.MonthBalanceDeposit.BatMthDepositBal(coopId, dTypeID, (DateTime)sDate, (DateTime)eDate, userId, budgetYear, period);
            _unitOfWork.MonthBalanceDeposit.sp_BatMthDepositBal(coopId, DepositTypeID, userId, budgetYear, period);
            //_unitOfWork.MonthBalanceDeposit.sp_BatMthDepositBal(coopId, userId, budgetYear, period);
            return Json("ประมวณผลเสร็จแล้ว", JsonRequestBehavior.AllowGet);
        }
    }}
