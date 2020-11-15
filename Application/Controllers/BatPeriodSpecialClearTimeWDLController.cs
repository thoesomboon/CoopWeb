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
    public class BatPeriodSpecialClearTimeWDLController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BatPeriodSpecialClearTimeWDLController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //
        // GET: /BatMthDeposit/
        public ActionResult Index()
        {
            return View();
        }
        [Authorization]
        public JsonResult ProcessBatPeriodSpecialClearTimeWDL()
        {
            var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();
            var CoopID = coopData.CoopID;
            var userId = AuthorizeHelper.Current.UserAccount().UserID;
            _unitOfWork.Deposit.sp_BatPeriodSpecialClearTimeWDL(CoopID, userId);
            return Json("ประมวณผลเสร็จแล้ว", JsonRequestBehavior.AllowGet);
        }
    }
}
