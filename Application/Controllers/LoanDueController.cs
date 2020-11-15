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
using Microsoft.Ajax.Utilities;
using System.IO;

namespace Coop.Controllers
{
    public class LoanDueController : BaseController
    {
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;
        public LoanDueController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorization]
        public ActionResult Index()
        {
            ViewBag.LoanTypeList = new SelectList(GetLoanTypeList(), "LoanTypeID", "LoanTypeName");
            ViewBag.InstallMethodList = new SelectList(GetInstallMethodList(), "InstallMethodID", "InstallMethodName");
            ViewBag.ReasonList = new SelectList(GetReasonList(), "ReasonID", "ReasonName");
            return View();
        }
        public List<LoanType> GetLoanTypeList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<LoanType> loanTypes = db.LoanType.ToList();
            return loanTypes;
        }
        public List<InstallMethod> GetInstallMethodList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<InstallMethod> installMethods = db.InstallMethod.ToList();
            return installMethods;
        }
        public List<Reason> GetReasonList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<Reason> reasons = db.Reason.ToList();
            return reasons;
        }
        [Authorization]
        //[HttpPost]
        public ActionResult TransferInLoanDue(string lonID)
        {
            if (lonID == null)
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new LoanModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            /// get Loan ID
            /// 
            //var pInfo = _unitOfWork.Loan.ReadDetail(pCode).FirstOrDefault();
            var pInfo = _unitOfWork.LoanDue.ReadLoanDueInfo(lonID);

            bool result = false;
            string msg = string.Empty;

            /// Loan Due Exist
            if (pInfo != null && !string.IsNullOrWhiteSpace(pInfo.LoanID))
            {
                result = true;
            }
            else
            {
                result = false;
                msg = "ไม่พบ รายการครบกำหนดสัญญาเงินกู้";
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
            object[] retObj = new object[] { oResult, pInfo };

            return Json(retObj, "application/json", JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetLoadDueList(string lonID)
        //{
        //    CoopWebEntities db = new CoopWebEntities();
        //    List<LoanDueModel> lonDueLists = db.LoanDue.Where(x=>x.LoanID == lonID && x.Filestatus == "A").Select(x=> new LoanDueModel {Seq=x.Seq, DueDate=x.DueDate, LoanDueAmt=x.LoanDueAmt, BFLoanDueAmt = x.BFLoanDueAmt, LoanID=x.LoanID}).ToList();
        //    ViewBag.loanDueList = lonDueLists;
        //    return PartialView("_LoanDue", ViewBag.loanDueLists);
        //}

        //public ActionResult LoanPayment_Read([DataSourceRequest] DataSourceRequest request, string loanId = "", string memberId = "")
        //{
        //    var list = new List<LoanPaymentModels>();
        //    if (string.IsNullOrEmpty(loanId))
        //    {
        //        return Json(list.ToDataSourceResult(request));
        //    }
        //    else
        //    {
        //        list = _unitOfWork.LoanPayment.ReadDetail(loanId).ToList();
        //    }
        //    return Json(list.ToDataSourceResult(request));
        //}
        public JsonResult GridLoadDueList(string LId)
        {
            CoopWebEntities db = new CoopWebEntities();
            var result = (from l in db.LoanDue
                          select new LoanDue
                          {
                              LoanID = l.LoanID,
                              Seq = l.Seq,
                              DueDate = l.DueDate,
                              LoanDueAmt = l.LoanDueAmt
                          }).Where(l => l.LoanID == LId && l.Filestatus =="A").ToList();
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GridLoadDueList()
        {
            CoopWebEntities db = new CoopWebEntities();
            var result = (from l in db.LoanDue
                          select new LoanDueModel
                          {
                              Filestatus = l.Filestatus,
                              LoanID = l.LoanID,
                              Seq = l.Seq,
                              DueDate = l.DueDate,
                              LoanDueAmt = l.LoanDueAmt
                          }).ToList();
            Dictionary<string, object> jsonResult = new Dictionary<string, object>();
            //jsonResult.Add("Html", RenderViewToString("~/Views/LoanDue/Index.cshtml", result));
            jsonResult.Add("Status", "Success");

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
            //return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        protected string RenderViewToString(string viewName, object model)
        {
            if (model != null)
            {
                ViewData.Model = model;
            }
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}