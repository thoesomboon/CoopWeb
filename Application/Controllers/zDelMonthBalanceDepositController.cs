using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using Coop.Library;

namespace Coop.Controllers
{
    public class MonthBalanceDepositController : BaseController
    {
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;

        //[Authorization]
        public MonthBalanceDepositController(IUnitOfWork unitOfWork)
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
            List<DepositType> DepositTypes = db.DepositType.ToList();
            return DepositTypes;
        }
        public ActionResult GetParam(BatMthDepositModel bModel)
        {
            var m = Convert.ToDateTime(AuthorizeHelper.Current.CoopControls().SystemDate).Month.ToString();

            var model = new BatMthDepositModel
            {
                //DepositType = db.DepositType.ToList(),
                CoopID = AuthorizeHelper.Current.CoopControls().CoopID,
                BudgetYear = AuthorizeHelper.Current.CoopControls().BudgetYear,
                //MonthNo = Convert.ToInt32(AuthorizeHelper.Current.CoopControls().SystemDate).Month.ToString(),
                //MonthNo = Convert.ToInt32(AuthorizeHelper.Current.CoopControls().SystemDate).Month.ToString(),

                StartDate = Library.DateLib.FirstDateOfMonth(Convert.ToString(AuthorizeHelper.Current.CoopControls().SystemDate)),
                EndDate = Library.DateLib.EndDateOfMonth(Convert.ToString(AuthorizeHelper.Current.CoopControls().SystemDate)),

                UserId = AuthorizeHelper.Current.UserAccount().UserID
            };
            //var DepositType = _unitOfWork.DepositType.Read().Where(p => p.Filestatus == FileStatus.A);
            //model.DepositType = DepositType;

            return View(model);
        }

        [Authorize]
        //public JsonResult ProcessBatMthDepositBal(BatMthDepositModel model)
        //{
        //    //var coopId = AuthorizeHelper.Current.CoopControl().CoopID;
        //    //var userId = AuthorizeHelper.Current.UserLogin().UserName;
        //    var CoopID = AuthorizeHelper.Current.CoopControls().CoopID,

        //    _unitOfWork.MonthBalanceDeposit.stored_BatMthDepositBal(model.CoopID, model.DepositTypeID, model.StartDate, model.EndDate, model.UserId, model.BudgetYear, model.Months);
        //    return Json("ประมวณผลเสร็จแล้ว", JsonRequestBehavior.AllowGet);
        //}
        public JsonResult ProcessBatMthDepositBal(BatMthDepositModel model)
        {
            TransactionResultModel result = _unitOfWork.MonthBalanceDeposit.sp_BatMthDepositBal(model.CoopID, model.DepositTypeID, (DateTime)model.StartDate, (DateTime)model.EndDate, model.UserId, model.BudgetYear, model.Period);
            return Json("ประมวณผลเสร็จแล้ว", JsonRequestBehavior.AllowGet);
        }
    }
}
