using Coop.Entities;
using Coop.Infrastructure.ActionFilters;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace Coop.Controllers
{
    public class ReasonController : BaseController
    {
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;
        public ReasonController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private CoopWebEntities db = new CoopWebEntities();

        [Authorization]
        //[HttpPost]
        public ActionResult Index()
        {
            //CoopWebEntities db = new CoopWebEntities();
            //List<Reason> Reasons = db.Reason.ToList();
            return View(db.Reason.ToList());
        }
        public JsonResult GetReasonList()
        {
            var result = (from t in db.Reason
                          select new ReasonModel
                          {
                              ReasonID = t.ReasonID,
                              ReasonName = t.ReasonName
                          }).ToList();
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
    }
}