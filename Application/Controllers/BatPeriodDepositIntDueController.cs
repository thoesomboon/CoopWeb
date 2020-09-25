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
    public class BatPeriodDepositIntDueController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BatPeriodDepositIntDueController(IUnitOfWork unitOfWork)
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
            List<DepositType> DepositTypes = db.DepositType.Where(d => d.Filestatus == "A" && (d.TypeOfDeposit == "SAV" || d.TypeOfDeposit == "SPC")).ToList();
            return DepositTypes;
        }
        [Authorization]
        public ActionResult GetParam(BatPeriodDepositIntDueModel batModel)
        {
            if (batModel == null)
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new BatPeriodDepositIntDueModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            bool result = false;
            string msg = string.Empty;

            var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();

            var model = new BatPeriodDepositIntDueModel
            {
                //DepositTypeID = batModel.DepositTypeID,
                CoopID = coopData.CoopID,
                BudgetYear = coopData.BudgetYear,
                DueDate = coopData.SystemDate,
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
        public JsonResult ProcessBatPeriodDepositIntDueBal(int coopId, string DepositTypeID, string budgetYear, DateTime calcDate)
        {
            var userId = AuthorizeHelper.Current.UserAccount().UserID;
            var workId = "X";
            var branchID = "X";
            var programName = "BatPeriodSpecialIntDue";
            if (DepositTypeID == "SAV")
                branchID = "BatPeriodSavingIntDue";

            _unitOfWork.Deposit.sp_BatPeriodDepositIntDue(coopId, DepositTypeID, calcDate, userId, branchID, programName, workId);
            //@CoopID, @DepTypeID, @CalcDate, @UserID, @BranchID, @ProgramName, @WorkStationId
            return Json("ประมวณผลเสร็จแล้ว", JsonRequestBehavior.AllowGet);
        }
    }}
