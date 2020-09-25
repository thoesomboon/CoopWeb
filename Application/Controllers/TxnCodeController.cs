using Coop.Entities;
using Coop.Infrastructure.ActionFilters;
using Coop.Infrastructure.Helpers;
using Coop.Library;
using Coop.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace Coop.Controllers
{
    public class TxnCodeController : BaseController
    {
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;
        public TxnCodeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorization]
        //[HttpPost]
        public ActionResult Index()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<TxnCode> TxnCodes = db.TxnCode.Where(t => t.Filestatus == FileStatus.A).ToList();
            return View(TxnCodes);
        }
        //public JsonResult GetTxnCodeList()
        //{
        //    CoopWebEntities db = new CoopWebEntities();
        //    var result = (from t in db.TTxnCode
        //                  select new TxnCodeModel
        //                  {
        //                      Filestatus = t.Filestatus,
        //                      CoopID = t.CoopID,
        //                      TxnType = t.TxnType,
        //                      TTxnCode = t.TTxnCode,
        //                      Descript = t.Descript
        //                  }).ToList();
        //    return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult TransferInTxnCodeDEP(string tCode)
        {
            /// check if exist
            if (string.IsNullOrWhiteSpace(tCode))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            //var progName = "OtxDeposit";
            var txndata = _unitOfWork.TxnCode.ReadDetailByType("DEP", tCode).FirstOrDefault();

            bool result = false;
            string msg = string.Empty;

            /// TTxnCode Exist
            //if (pInfo != null && !string.IsNullOrWhiteSpace(pInfo.TTxnCode))
            if (txndata != null)
            {
                result = true;
            }
            else
            {
                result = false;
                msg = "TxnCode Not Found";
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
            object[] retObj = new object[] { oResult, txndata };

            return Json(retObj, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult TransferInTxnCodeDEPOpen(string txnCode, string progName)
        {
            /// check if exist
            if (string.IsNullOrWhiteSpace(txnCode))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            //var progName = "OtxDeposit";
            var txndata = _unitOfWork.TxnCode.ReadDetail(txnCode, progName).FirstOrDefault();

            bool result = false;
            string msg = string.Empty;

            /// TTxnCode Exist
            //if (pInfo != null && !string.IsNullOrWhiteSpace(pInfo.TTxnCode))
            if (txndata != null)
            {
                result = true;
            }
            else
            {
                result = false;
                msg = "TxnCode Not Found";
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
            object[] retObj = new object[] { oResult, txndata };

            return Json(retObj, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult TransferInTxnCode(string tType, string tCode)
        {
            /// check if exist
            if (string.IsNullOrWhiteSpace(tCode))
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            //var progName = "OtxDeposit";
            var txndata = _unitOfWork.TxnCode.ReadDetailByType(tType, tCode).FirstOrDefault();

            bool result = false;
            string msg = string.Empty;

            /// TTxnCode Exist
            //if (pInfo != null && !string.IsNullOrWhiteSpace(pInfo.TTxnCode))
            if (txndata != null)
            {
                result = true;
            }
            else
            {
                result = false;
                msg = "TxnCode Not Found";
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
            object[] retObj = new object[] { oResult, txndata };

            return Json(retObj, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}