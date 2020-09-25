using Coop.Entities;
using Coop.Infrastructure.ActionFilters;
using Coop.Infrastructure.Extensions;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml;

namespace Coop.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private void ApplicationConfig()
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(FilterConfig.ApplicationConfig);
            XmlNode root = doc.DocumentElement;
            XmlNode maxRowSelect = root.SelectSingleNode("MaxRowSelect");

            var currentAppControl = _storage.GetOrAdd("AppControl", () => new CurrentAppControlModel());

            currentAppControl.MaxRowSelect = Convert.ToInt32(maxRowSelect.InnerXml);
        }

        [Authorization]
        public ActionResult Index()
        {
            var UserType = AuthorizeHelper.Current.UserAccount().UserTypeID;

            var currentAppControlModel = _storage.GetOrAdd("AppControl", () => new CurrentAppControlModel());

            ApplicationConfig();

            if (AuthorizeHelper.Current.ModuleMenulist().Count == 0)
            {
                var menu = _unitOfWork.Modules.GetMenu(UserType);

                var currentmodule = _storage.GetOrAdd("ModuleMenulist", () => new List<ModuleModel>());
                currentmodule.AddRange(menu);
            }

            if (AuthorizeHelper.Current.ModuleMenuCategorylist().Count == 0)
            {
                var menuCategory = _unitOfWork.Modules.GetMenuCategory(UserType);

                var currentmoduleCategory = _storage.GetOrAdd("ModuleMenuCategorylist", () => new List<ModuleModel>());
                currentmoduleCategory.AddRange(menuCategory);
            }

            var model = new HomeModels();

            return View(model);
        }

        public ActionResult GetNavbar()
        {
            var model = new HomeModels();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public JsonResult GetHome(string startDate, string endDate)
        {
            var userID = AuthorizeHelper.Current.UserAccount().UserID;
            var cul = AuthorizeHelper.Current.AppControl().ApplicationCulture;
            var model = new HomeModels();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetCulture(string culture, string requestUrl)
        {
            culture = CultureHelper.GetValidCulture(culture);

            var cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture") { HttpOnly = false, Value = culture, Expires = DateTime.Now.AddYears(1) };
            }
            Response.Cookies.Add(cookie);


            if (Url.IsLocalUrl(requestUrl) && requestUrl.Length > 1
                        && !requestUrl.StartsWith("//") && !requestUrl.StartsWith("/\\"))
            {
                return Redirect(requestUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}