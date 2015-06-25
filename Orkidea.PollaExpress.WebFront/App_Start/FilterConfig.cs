using System.Web;
using System.Web.Mvc;

namespace Orkidea.PollaExpress.WebFront
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
