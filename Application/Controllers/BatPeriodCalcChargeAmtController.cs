﻿using System;
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
    public class BatPeriodCalcChargeAmtController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BatPeriodCalcChargeAmtController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //
        // GET: /BatPeriodCalcChargeAmt/
        public ActionResult Index()
        {
            //ViewBag.DepositTypeList = new SelectList(GetDepositTypeList(), "DepositTypeID", "DepositTypeName");
            return View();
        }
        //public List<DepositType> GetDepositTypeList()
        //{
        //    CoopWebEntities db = new CoopWebEntities();
        //    //List<DepositType> DepositTypes = db.DepositType.ToList();
        //    List<DepositType> DepositTypes = db.DepositType.Where(d => d.Filestatus == "A" && (d.TypeOfDeposit == "SAV" || d.TypeOfDeposit == "SPC")).ToList();
        //    return DepositTypes;
        //}
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
                CalcDate = coopData.SystemDate
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
        public JsonResult ProcessBatTrfMilk2Deposit(int coopId, DateTime calcDate)
        {
            var userId = AuthorizeHelper.Current.UserAccount().UserID;
            var workId = "A001";
            _unitOfWork.Loan.Sp_BatPeriodCalcChargeAmt(coopId, calcDate, userId, workId);
            //@CoopID, @DepTypeID, @CalcDate, @UserID, @BranchID, @ProgramName, @WorkStationId
            return Json("ประมวณผลเสร็จแล้ว", JsonRequestBehavior.AllowGet);
        }
    }
}
