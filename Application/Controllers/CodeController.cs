using Coop.Entities;
using Coop.Infrastructure.ActionFilters;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
//using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace Coop.Controllers
{
    public class CodeController : BaseController
    {
        // GET: Coop
        private readonly IUnitOfWork _unitOfWork;
        public CodeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ActionResult GetLoanTypeListPartial()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<LoanType> loanTypeList = db.LoanType.ToList();
            ViewBag.LoanTypeOptions = new SelectList(loanTypeList, "LoanTypeID", "LoanTypeName");
            return PartialView("LoanTypeOptionPartial");
        }
        //<input name = "myInput" id="myInput" onkeypress="return allowOnlyNumber(event);">
        //function allowOnlyNumber(evt)
        //{
        //  var charCode = (evt.which) ? evt.which : event.keyCode
        //  if (charCode > 31 && (charCode< 48 || charCode> 57))
        //    return false;
        //  return true;
        //}
    }
}