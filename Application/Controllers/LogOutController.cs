using System;
using System.Web.Mvc;
using System.Web.Security;
using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Infrastructure.Extensions;

namespace Coop.Controllers
{
    public class LogOutController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LogOutController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(string cId = "0", string waitTime = "0", string dialingTime = "0", string talkTime = "0", string wrapTime = "0", string page = "")
        {
            var userId = AuthorizeHelper.Current.UserAccount().UserID;
            if (userId > 0)
            {
                _unitOfWork.AccessTransaction.LogOutAccessTransactions(userId);

                if (AuthorizeHelper.Current.UserAccount().AccessTransactionID > 0)
                {
                    AuthorizeHelper.Current.UserAccount().AccessTransactionID = 0;
                }
                AuthorizeHelper.Current.UserAccount().UserID = 0;
                AuthorizeHelper.Current.UserAccount().UserName = "";
                AuthorizeHelper.Current.UserAccount().UserTypeID = 0;
                AuthorizeHelper.Current.UserAccount().UserTypeName = "";
            }
            AuthorizeHelper.Current.ModuleMenulist().Clear();
            AuthorizeHelper.Current.ModuleMenuCategorylist().Clear();
            return View();
        }
        public ActionResult LogoutExpire(string cId = "0", string waitTime = "0", string dialingTime = "0", string talkTime = "0", string wrapTime = "0", string page = "")
        {
            var userId = AuthorizeHelper.Current.UserAccount().UserID;
            if (userId > 0)
            {
                _unitOfWork.AccessTransaction.LogOutAccessTransactions(userId);

                if (AuthorizeHelper.Current.UserAccount().AccessTransactionID > 0)
                {
                    AuthorizeHelper.Current.UserAccount().AccessTransactionID = 0;
                }
                AuthorizeHelper.Current.UserAccount().UserID = 0;
                AuthorizeHelper.Current.UserAccount().UserName = "";
                AuthorizeHelper.Current.UserAccount().UserTypeID = 0;
                AuthorizeHelper.Current.UserAccount().UserTypeName = "";
                FormsAuthentication.SignOut();
            }
            return RedirectToAction("Login", "Account");
        }
    }
}
