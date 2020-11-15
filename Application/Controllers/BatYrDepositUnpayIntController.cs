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
    public class BatYrDepositUnpayIntController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BatYrDepositUnpayIntController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //
        // GET: /BatYrDepositUnpayInt/
        public ActionResult Index()
        {
            var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();
            var model = new BatPeriodDepositIntDueModel
            {
                CoopID = coopData.CoopID,
                SystemDate = coopData.SystemDate,
                SystemDateTH = coopData.SystemDate.Value.AddYears(543).ToString("dd/MM/yyyy")
            };

            return View(model);
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
                CoopID = coopData.CoopID,
                BudgetYear = coopData.BudgetYear,
                SystemDate = coopData.SystemDate,
                SystemDateTH = coopData.SystemDate.Value.AddYears(543).ToString("dd/MM/yyyy")
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
        public JsonResult ProcessBatYrDepositUnpayInt(int CoopID, string calcDate)
        {
            var userId = AuthorizeHelper.Current.UserAccount().UserID;
            DateTime CalcDate = Convert.ToDateTime(calcDate);

            _unitOfWork.Deposit.Sp_BatYrDepositUnpayInt(CoopID, CalcDate);
            return Json("ประมวณผลเสร็จแล้ว", JsonRequestBehavior.AllowGet);
        }
    }
}
