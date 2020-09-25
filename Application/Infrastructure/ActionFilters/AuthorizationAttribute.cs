using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Coop.Infrastructure.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizationAttribute : AuthorizeAttribute
    {
        //private readonly AuthorizeHelper _authorizationHelper;
        private AuthorizationContext _authorizationContext;
        public string Decription { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            _authorizationContext = filterContext;
            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            if (null == _authorizationContext)
                throw new ArgumentNullException("authorizationContext");

            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            if (Helpers.AuthorizeHelper.Current.UserAccount().UserID == 0)
            {
                return false;
            }
            var operation = string.Format("{0}/{1}",
                                          _authorizationContext.RequestContext.RouteData.Values["Controller"],
                                          _authorizationContext.RequestContext.RouteData.Values["Action"]);
           // return !string.IsNullOrEmpty(operation) && AuthorizeHelper.Current.Authorize(operation);

            return true;
        }

        protected override HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            var flag = AuthorizeCore(httpContext);
            return flag ? HttpValidationStatus.Valid : HttpValidationStatus.IgnoreThisRequest;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //User not sign in
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated) //|| !AuthorizeHelper.Current.ExistSessionId(filterContext.RequestContext.HttpContext.Session.SessionID))
            {
                filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl + "?returnUrl=" +
                                                         filterContext.HttpContext.Server.UrlEncode(filterContext.HttpContext.Request.RawUrl));
            }
            else
            {
                filterContext.Result =
                   new RedirectToRouteResult(new RouteValueDictionary { { "Action", "LoginAuth" }, { "Controller", "Account" } });
            }
            //else
            //{
            //    // User not have permission.
            //    filterContext.Result =
            //        new RedirectToRouteResult(new RouteValueDictionary { { "Action", "AccessDenied" }, { "Controller", "UserAccount" } });
            //}
        }
    }
}