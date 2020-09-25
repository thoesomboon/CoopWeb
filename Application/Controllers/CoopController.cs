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
    public class CoopController : BaseController
    {
        // GET: Coop
        private readonly IUnitOfWork _unitOfWork;
        public CoopController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ActionResult ReadCoop()
        {
            var pInfo = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();

            bool result = false;
            string msg = string.Empty;

            /// Account Exist
            if (pInfo != null)
            {
                result = true;
            }
            else
            {
                result = false;
                msg = "Coop Not Found";
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
    }
}