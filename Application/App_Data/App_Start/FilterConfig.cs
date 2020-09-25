using System.Web;
using System.Web.Mvc;

namespace Coop
{
    public class FilterConfig
    {
        public static string FilterPath = "Filters/ApplicationConfig.xml";

        public static string ApplicationConfig = string.Empty;

        public static void RegisterFilters(HttpApplication httpApp)
        {
            ApplicationConfig = httpApp.Server.MapPath("~/" + FilterPath);
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}