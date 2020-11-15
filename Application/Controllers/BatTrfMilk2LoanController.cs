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
    public class BatTrfMilk2LoanController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BatTrfMilk2LoanController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //
        // GET: /BatTrfMilk2Loan/
        public ActionResult Index()
        {
            var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();
            var model = new BatTrfMilk2LoanModel
            {
                CoopID = coopData.CoopID,
                CalcDate = coopData.SystemDate,
                CalcDateTH = coopData.SystemDate.Value.AddYears(543).ToString("dd/MM/yyyy")
                //ViewBag.strNextDate = AuthorizeHelper.Current.CoopSystem().next_system_date.Value.AddYears(543).ToString("dd/MM/yyyy");
                //UserId = AuthorizeHelper.Current.UserAccount().UserID
                //WorkStationId = AuthorizeHelper.Current.CoopGETMachine().WorkStationID
            };
            return View(model);
        }
        [Authorization]
        public JsonResult ProcessBatTrfMilk2Loan(int CoopId, string cDateTH)
        {
            var userId = AuthorizeHelper.Current.UserAccount().UserID;
            var workId = "A001";
            DateTime calcDate = Convert.ToDateTime(cDateTH);
            _unitOfWork.Loan.sp_BatTrfMilk2Loan(CoopId, calcDate, userId, workId);
            return Json("** ประมวณผลเสร็จแล้ว **", JsonRequestBehavior.AllowGet);
        }
    }
}
