using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Coop.Models.POCO;
using Coop.Entities;
using System.Reflection;
using Coop.Infrastructure.Extensions;
using System.Web.Security;
using System.IO;
using System.Xml;
using Coop.Infrastructure.Helpers;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Coop.Resources;

namespace Coop.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        string _licenseMsg = string.Empty;
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            LogOut(); 
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userName = HttpContext.User.Identity.Name;
                if (userName.Split('\\').Length > 1)
                {
                    userName = userName.Split('\\')[1].ToUpper();
                    var usr = _unitOfWork.Users.Read().FirstOrDefault(p => p.UserName.ToUpper() == userName.ToUpper());
                    if (usr == null)
                    {
                        FormsAuthentication.SetAuthCookie(userName.ToString(CultureInfo.InvariantCulture), false);
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(usr.UserName.ToString(CultureInfo.InvariantCulture), false);
                    }
                    return RedirectToLocal("/Account/LoginAuth");
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            AuthorizeHelper.Current.ApplicationVersion().Version = Version();
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            var usr = ValidateUserLogin(model.UserName, model.Password, returnUrl);
            if (ModelState.IsValid)
            {
                InsertLogIn(usr);
                FormsAuthentication.SignOut();
                FormsAuthentication.SetAuthCookie(model.UserName, false);
                return RedirectToLocal("/Home/Index");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult LoginAuth()
        {
            var userName = HttpContext.User.Identity.Name;
            if (userName.Split('\\').Length > 1)
            {
                userName = userName.Split('\\')[1];
            }
            else
            {
                if (userName.Split('\\').Length == 1)
                {
                    userName = userName.Split('\\')[0];
                }

            }
            var user = new LoginModel { UserName = userName, Password = "xxxxxxx" };
            AuthorizeHelper.Current.ApplicationVersion().Version = Version();

            return View(user);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LoginAuth(LoginModel model)
        {
            var usr = ValidateUserLogin(model.UserName, model.Password, "");
            if (ModelState.IsValid)
            {
                InsertLogIn(usr);
                return RedirectToLocal("/Home/Index");
            }
            return View(model);
        }

        public String Version()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var asm = BuildVersion.GetBuildDateTime(Assembly.GetExecutingAssembly());
            var dateformet = asm.Date.ToString("yyyyMMdd", CultureInfo.CurrentCulture);
            var timeformet = asm.TimeOfDay.ToString().Replace(":", "");
            return (string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, dateformet, timeformet));

        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        private Users ValidateUserLogin(string userName, string password, string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
                ModelState.AddModelError("", Resources.Messages.Urlnotformat);
            }
            if (userName != null)
            {              
                
                var usr = _unitOfWork.Users.Read().FirstOrDefault(p => p.UserName.ToUpper() == userName.ToUpper());

                if (usr != null)
                {
                    if (usr.Password != password && !HttpContext.User.Identity.IsAuthenticated)
                    {
                        ModelState.AddModelError("", Resources.Messages.UserNameAndPassIncorrect);
                        return usr;
                    }
                    if (!usr.IsActive)
                    {
                        ModelState.AddModelError("", Resources.Messages.UserisnotActive);
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resources.Messages.UserNameAndPasswordnotonSystem);
                }
                return usr;
            }
            ModelState.AddModelError("", Resources.Messages.UserNameAndPassIncorrect);
            return new Users();
        }

        private void InsertLogIn(Users usr)
        {
            if (usr.UserID > 0)
            {
                var obj = new AccessTransactionModels
                {
                    UserID = usr.UserID,
                    LoginDate = DateTime.Now,
                    LogoutDate = null,
                    MachineName = GetVisitorComputerName().ToUpper(),
                    IPAddress = GetVisitorIpAddress()
                };
                XmlDocument doc = new XmlDocument();
                doc.Load(FilterConfig.ApplicationConfig);
                XmlNode root = doc.DocumentElement;
                var accessTransaction = _unitOfWork.AccessTransaction.Create(obj);
                var currentUserAccount = _storage.GetOrAdd("UserAccount", () => new CurrentUserAccountModel());
                currentUserAccount.UserID = usr.UserID;
                currentUserAccount.UserName = (usr.FirstName ?? "") + " " + (usr.LastName ?? "");
                currentUserAccount.UserTypeID = usr.UserTypeID;
                currentUserAccount.UserTypeName = usr.UserTypes.UserTypeName;
                currentUserAccount.AccessTransactionID = accessTransaction.AccessTransactionID;

                string stringComputerName = "";
                var CoopMachine = _storage.GetOrAdd("CoopGETMachine", () => new CoopMachineModel());
                CoopMachine.BranchID = System.Configuration.ConfigurationManager.AppSettings["BranchId"] ?? "";
                CoopMachine.Server = System.Configuration.ConfigurationManager.AppSettings["Server"] ?? "";
                try
                {

                    if (!string.IsNullOrEmpty(Request.ServerVariables["REMOTE_ADDR"].ToString(CultureInfo.InvariantCulture)))
                    {
                        stringComputerName = Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName.Split('.')[0];
                        obj.MachineName = stringComputerName;
                    }
                    CoopMachine.WorkStationID = stringComputerName;
                }
                catch (Exception)
                {

                    CoopMachine.WorkStationID = "";
                }
            }
        }
        public string GetVisitorComputerName()
        {
            try
            {
                string stringComputerName = "";
                if (!string.IsNullOrEmpty(Request.ServerVariables["REMOTE_ADDR"].ToString(CultureInfo.InvariantCulture)))
                {
                    stringComputerName = Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName.Split('.')[0];
                }
                return stringComputerName;
            }
            catch (Exception)
            {

                return "";
            }
        }

        public string GetVisitorIpAddress()
        {
            try
            {
                string stringIpAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (stringIpAddress == null) //may be the HTTP_X_FORWARDED_FOR is null
                {
                    if (!string.IsNullOrEmpty(Request.ServerVariables["REMOTE_ADDR"].ToString(CultureInfo.InvariantCulture)))
                    {
                        stringIpAddress = Request.ServerVariables["REMOTE_ADDR"];//we can use REMOTE_ADDR
                    }
                    else
                    {
                        stringIpAddress = "";
                    }
                }
                return stringIpAddress;
            }
            catch (Exception)
            {

                return "";
            }
        }
        public ActionResult LogOut(string cId = "0")
        {
            var userId = AuthorizeHelper.Current.UserAccount().UserID;
            var accessTransactionID = AuthorizeHelper.Current.UserAccount().AccessTransactionID;
            if (userId > 0)
            {
                _unitOfWork.AccessTransaction.LogOutAccessTransactions(userId);
                if (Session["AccessTransactionID"] != null && (int)Session["AccessTransactionID"] > 0)
                {
                    _unitOfWork.Save();
                    Session["AccessTransactionID"] = null;
                }

                AuthorizeHelper.Current.UserAccount().UserID = 0;
                AuthorizeHelper.Current.UserAccount().UserName = "";
                AuthorizeHelper.Current.UserAccount().UserTypeID = 0;
                AuthorizeHelper.Current.UserAccount().UserTypeName = "";
                FormsAuthentication.SignOut();
            }
            AuthorizeHelper.Current.ModuleMenulist().Clear();
            AuthorizeHelper.Current.ModuleMenuCategorylist().Clear();
            return null;
        }

        public JsonResult CheckValidUsernameAndPassword(int userID, string userPassword)
        {
            var checkS = _unitOfWork.Users.CheckUserIDAndPass(userID, userPassword);
            return checkS.Any() ? Json(true, JsonRequestBehavior.AllowGet) : Json(Text.UserNameOrPasswordInvalid, JsonRequestBehavior.AllowGet);
        }

        public void ValidatePassword(UserResetModel model)
        {
            var checkS = _unitOfWork.Users.CheckUserIDAndPass(model.UserID, model.Password);
            if (checkS.Any() && model.Password == model.Password)
            {
                ModelState.AddModelError("UserPassword", string.Format(Messages.PassInChang));
            }
        }
    }

}