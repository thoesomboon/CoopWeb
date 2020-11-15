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
    public class BatDayCloseController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BatDayCloseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //
        // GET: /BatMthcoposit/
        public ActionResult Index()
        {
            var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();
            var model = new BatDayCloseModel
            {
                CoopID = coopData.CoopID,
                //SystemDate = coopData.SystemDate,
                SystemDateTH = coopData.SystemDate.Value.AddYears(543).ToString("dd/MM/yyyy"),
                //NextSystemDate = coopData.NextSystemDate,
                NextSystemDateTH = coopData.NextSystemDate.Value.AddYears(543).ToString("dd/MM/yyyy")
            };
            
            return View(model);
        }
        //public List<copositType> GetcopositTypeList()
        //{
        //    CoopWebEntities db = new CoopWebEntities();
        //    //List<copositType> copositTypes = db.copositType.ToList();
        //    List<copositType> copositTypes = db.copositType.Where(d => d.Filestatus == "A" && (d.TypeOfcoposit == "SAV" || d.TypeOfcoposit == "SPC")).ToList();
        //    return copositTypes;
        //}
        [Authorization]
        public ActionResult GetParam(BatDayCloseModel batModel)
        {
            if (batModel == null)
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new BatDayCloseModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            bool result = false;
            string msg = string.Empty;

            var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();
            DateTime NextDate = DateLib.WorkingDay(coopData.NextSystemDate.Value.AddYears(543).ToString("dd/MM/yyyy"));

            var model = new BatDayCloseModel
            {
                CoopID = coopData.CoopID,
                BudgetYear = coopData.BudgetYear,
                //SystemDate = coopData.SystemDate,
                SystemDateTH = coopData.SystemDate.Value.AddYears(543).ToString("dd/MM/yyyy"),
                //NextSystemDate = coopData.NextSystemDate,
                NextSystemDateTH = coopData.NextSystemDate.Value.AddYears(543).ToString("dd/MM/yyyy"),

                NextWorkingDayTH = NextDate.AddYears(543).ToString("dd/MM/yyyy"),

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
        [HttpPost]
        public ActionResult TransferOutBatDayClose(BatDayCloseModel dayCloseModel)
        {
            //ProcessBatTrfMilk2Deposit(int CoopID, string CalcDate)
            if (dayCloseModel == null || dayCloseModel.Equals(new BatDayCloseModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new BatDayCloseModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            //var coopId = AuthorizeHelper.Current.CoopSystem().coop_id;
            //var systemDate = AuthorizeHelper.Current.CoopSystem().system_date;
            BatDayCloseModel model = dayCloseModel;
            var copID = model.CoopID;

            bool result = false;
            string msg = string.Empty;

            var cop = _unitOfWork.CoopControl.ReadDetail(copID).FirstOrDefault();
            if (cop != null)
            {
                cop.PrevSystemDate = DateLib.DateInCE(model.SystemDateTH);
                cop.SystemDate = DateLib.DateInCE(model.NextSystemDateTH);
                cop.NextSystemDate = DateLib.DateInCE(model.NextWorkingDayTH);

                _unitOfWork.CoopControl.Update(cop);
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
            object[] retObj = new object[] { oResult, model };

            return Json(retObj, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}
