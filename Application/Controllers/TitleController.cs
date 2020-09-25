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
    public class TitleController : BaseController
    {
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;
        public TitleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorization]
        //[HttpPost]
        public ActionResult Index()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<Title> titles = db.Title.Where(t => t.IsActive).ToList();
            return View(titles);
        }
        public JsonResult GetTitleList()
        {
            CoopWebEntities db = new CoopWebEntities();
            var result = (from t in db.Title
                          select new TitleModel
                          {
                              TitleID = t.TitleID,
                              TitleName = t.TitleName,
                              TitleNameEng = t.TitleNameEng,
                              Gender = t.Gender,
                              IsActive = t.IsActive
                          }).ToList();
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
    }
}