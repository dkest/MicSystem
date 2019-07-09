using Mic.Web.Common.Attribute;
using System.Web;
using System.Web.Mvc;

namespace Mic.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomAuthorizeAttribute());
            //filters.Add(new CustomActionFilterAttribute());
            //filters.Add(new GlobalHandleErrorAttribute());
        }
    }
}
