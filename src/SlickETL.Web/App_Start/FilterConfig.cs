using System.Web;
using System.Web.Mvc;

namespace SlickETL.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            GlobalFilters.Filters.Add(new RequireHttpsAttribute());
        }
    }
}
