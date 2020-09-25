using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;

namespace Coop.Infrastructure.Extensions
{
    public static class HTMLHelperExtensions
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {

            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
        }

        public static string PageClass(this HtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];
            string current = currentController + "-" + currentAction;
            if (current == "Contact-Index" || current == "CallProcess-CallProcess" || current == "Reconfirm-Index" || current == "Reconfirm-Edit" || current == "PolicyQC-Index"
                || current == "PolicyQC-QCView" || current == "PolicyQC-EditPolicy" || current == "VerifyEDC-DetailEDCAdmin" || current == "VerifyEDC-DetailEDCTsr" || current == "VerifyEDC-DetailUWAdmin" 
                || current == "ListAssignment-Index" || current == "LoadLead-Index" || current == "LoadLead-LoadLead" || current == "LoadLead-EndProcess")
            {
                return "page-header-fixed page-sidebar-closed-hide-logo page-container-bg-solid page-content-white page-sidebar-closed";
            }
            if (currentController.Equals("DepositTransaction", StringComparison.InvariantCultureIgnoreCase) ||
                currentController.Equals("Transaction", StringComparison.InvariantCultureIgnoreCase))
            {
                return "page-header-fixed page-sidebar-closed-hide-logo page-container-bg-solid page-boxed m-page--fluid m--skin- m-content--skin-light m-header--fixed m-header--fixed-mobile m-aside-left--enabled m-aside-left--skin-dark m-aside-left--offcanvas m-footer--push m-aside--offcanvas-default";

            } 
            return "page-header-fixed page-sidebar-closed-hide-logo page-container-bg-solid page-boxed";
            //return "page-header-fixed page-sidebar-closed-hide-logo page-container-bg-solid page-content-white page-boxed ";
        }
        public static string PageNavigationClass(this HtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];
            string current = currentController + "-" + currentAction;
            if (current == "Contact-Index" || current == "CallProcess-CallProcess" || current == "Reconfirm-Index" || current == "Reconfirm-Edit" || 
                current == "PolicyQC-Index" || current == "PolicyQC-QCView" || current == "PolicyQC-EditPolicy" || current == "VerifyEDC-DetailEDCAdmin" 
                || current == "VerifyEDC-DetailEDCTsr" || current == "VerifyEDC-DetailUWAdmin" || current == "ListAssignment-Index" || current == "LoadLead-Index"
                || current == "LoadLead-LoadLead" || current == "LoadLead-EndProcess")
            {
                return "page-sidebar-menu  page-header-fixed page-sidebar-menu-closed";
            }
            //return "page-sidebar-menu  page-header-fixed page-sidebar-menu-hover-submenu page-sidebar-menu-compact ";
            return "page-sidebar-menu  page-header-fixed page-sidebar-menu-compact ";
            //return "page-header-fixed page-sidebar-closed-hide-logo page-container-bg-solid page-content-white";
        }

        public static MvcHtmlString LabelFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> ex,
            Func<object, HelperResult> template)
        {
            var htmlFieldName = ExpressionHelper.GetExpressionText(ex);
            var propertyName = htmlFieldName.Split('.').Last();
            var label = new TagBuilder("label");
            label.Attributes["for"] = TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName));
            label.InnerHtml = string.Format("{0} {1}", propertyName, template(null).ToHtmlString());
            return MvcHtmlString.Create(label.ToString());
        }
    }
}
