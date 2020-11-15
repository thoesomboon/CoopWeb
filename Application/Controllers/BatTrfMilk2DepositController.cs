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
    public class BatTrfMilk2DepositController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BatTrfMilk2DepositController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //
        // GET: /BatMthDeposit/
        public ActionResult Index()
        {
            var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();
            var model = new BatTrfMilk2DepositModel
            {
                CoopID = coopData.CoopID,
                TxnDate = coopData.SystemDate,
                TxnDateTH = coopData.SystemDate.Value.AddYears(543).ToString("dd/MM/yyyy")
             };

            return View(model);
        }
        //public List<DepositType> GetDepositTypeList()
        //{
        //    CoopWebEntities db = new CoopWebEntities();
        //    //List<DepositType> DepositTypes = db.DepositType.ToList();
        //    List<DepositType> DepositTypes = db.DepositType.Where(d => d.Filestatus == "A" && (d.TypeOfDeposit == "SAV" || d.TypeOfDeposit == "SPC")).ToList();
        //    return DepositTypes;
        //}
        [Authorization]
        public ActionResult GetParam(BatTrfMilk2DepositModel batModel)
        {
            if (batModel == null)
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new BatTrfMilk2DepositModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            bool result = false;
            string msg = string.Empty;

            var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();

            var model = new BatTrfMilk2DepositModel
            {
                CoopID = coopData.CoopID,
                BudgetYear = coopData.BudgetYear,
                TxnDate = coopData.SystemDate,
                TxnDateTH = coopData.SystemDate.Value.AddYears(543).ToString("dd/MM/yyyy"),
                SystemDate = coopData.SystemDate,
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
        public JsonResult ProcessBatTrfMilk2Deposit(int CoopID, string CalcDate)
        {
            var userId = AuthorizeHelper.Current.UserAccount().UserID;
            var workId = "A001";
            var branchID = "B01";
            //var programName = "BatTrfMilk2Deposit";
            DateTime calcDate = Convert.ToDateTime(CalcDate);

            _unitOfWork.Deposit.Sp_BatTrfMilk2Deposit(CoopID, calcDate, branchID, userId, workId);
            //@CoopID, @DepTypeID, @CalcDate, @UserID, @BranchID, @ProgramName, @WorkStationId
            return Json("ประมวณผลเสร็จแล้ว", JsonRequestBehavior.AllowGet);
        }
    }}
