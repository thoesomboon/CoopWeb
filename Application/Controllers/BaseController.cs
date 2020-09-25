using Coop.Infrastructure.ActionFilters;
using Coop.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Coop.Controllers
{
    public class BaseController : AsyncController
    {
        // GET: Base
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Is it View ?
            var view = filterContext.Result as ViewResultBase;
            if (view == null) // if not exit
                return;

            var cultureName = Thread.CurrentThread.CurrentCulture.Name;
            // e.g. "en-US" // filterContext.HttpContext.Request.UserLanguages[0]; // needs validation return "en-us" as default            

            // Is it default culture? exit
            if (cultureName == CultureHelper.GetDefaultCulture())
                return;

            // Are views implemented separately for this culture?  if not exit
            var isSeparatelyView =
                filterContext.ActionDescriptor.GetCustomAttributes(typeof(SeparatelyCultureViewAttribute), true).Any();
            var viewImplemented = isSeparatelyView || CultureHelper.IsViewSeparate(cultureName);

            if (viewImplemented == false)
                return;

            var viewName = view.ViewName;

            int i;

            if (string.IsNullOrEmpty(viewName))
                viewName = filterContext.RouteData.Values["action"] + "." + cultureName; // Index.en-US
            else if ((i = viewName.IndexOf('.')) > 0)
            {
                // contains . like "Index.cshtml"                
                viewName = viewName.Substring(0, i + 1) + cultureName + viewName.Substring(i);
            }
            else
                viewName += "." + cultureName; // e.g. "Index" ==> "Index.en-Us"

            view.ViewName = viewName;

            filterContext.Controller.ViewBag._culture = "." + cultureName;

            base.OnActionExecuted(filterContext);
        }

        protected override void ExecuteCore()
        {
            var cultureCookie = Request.Cookies["_culture"];
            var cultureName = cultureCookie != null ? cultureCookie.Value : "en-US";

            // Validate culture name
            cultureName = CultureHelper.GetValidCulture(cultureName);

            // Modify current thread's culture
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.Calendar = new GregorianCalendar();
            AuthorizeHelper.Current.AppControl().ApplicationCulture = cultureName == "en-US";
            base.ExecuteCore();
           }
        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }
    }
}
