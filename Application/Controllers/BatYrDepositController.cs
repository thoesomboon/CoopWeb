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
    public class BatYrDepositController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BatYrDepositController(IUnitOfWork unitOfWork)
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
        public ActionResult GetParam(BatYrDepositModel batModel)
        {
            if (batModel == null)
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new BatYrDepositModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            bool result = false;
            string msg = string.Empty;

            var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();

            var model = new BatYrDepositModel
            {
                //DepositTypeID = batModel.DepositTypeID,
                CoopID = coopData.CoopID,
                BudgetYear = coopData.BudgetYear,
                Period1 = (int)coopData.AccountPeriod,
                Period2 = (int)coopData.AccountPeriod,

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
        public JsonResult ProcessBatYrDepositBal(int coopId, string DepositTypeID, string budgetYear, int period1, int period2)
        {
            var userId = AuthorizeHelper.Current.UserAccount().UserID;
            //_unitOfWork.MonthBalanceDeposit.BatMthDepositBal(coopId, dTypeID, (DateTime)sDate, (DateTime)eDate, userId, budgetYear, period);
            _unitOfWork.YearBalanceDeposit.sp_BatYrDepositBal(coopId, DepositTypeID, userId, budgetYear, period1, period2);
            //_unitOfWork.MonthBalanceDeposit.sp_BatMthDepositBal(coopId, userId, budgetYear, period);
            return Json("** ประมวณผลเสร็จแล้ว **", JsonRequestBehavior.AllowGet);
        }
    }}
