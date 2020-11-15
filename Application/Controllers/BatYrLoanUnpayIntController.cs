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
    public class BatYrLoanUnpayIntController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BatYrLoanUnpayIntController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //
        // GET: /BatMthDeposit/
        public ActionResult Index()
        {
            var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();
            var model = new BatMthLoanModel
            {
                CoopID = coopData.CoopID,
                CalcDate = coopData.SystemDate,
                CalcDateTH = coopData.SystemDate.Value.AddYears(543).ToString("dd/MM/yyyy")
            };

            return View(model);
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
                CoopID = coopData.CoopID,
                BudgetYear = coopData.BudgetYear,
                CalcDate = coopData.SystemDate,
                CalcDateTH = coopData.SystemDate.Value.AddYears(543).ToString("dd/MM/yyyy")
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
        public JsonResult ProcessBatYrLoanUnpayInt(int CoopID, string CalcDate)
        {
            var userId = AuthorizeHelper.Current.UserAccount().UserID;
            DateTime calcDate = Convert.ToDateTime(CalcDate);

            _unitOfWork.Loan.Sp_BatYrLoanUnpayInt(CoopID, calcDate);
            return Json("ประมวณผลเสร็จแล้ว", JsonRequestBehavior.AllowGet);
        }
    }
}
