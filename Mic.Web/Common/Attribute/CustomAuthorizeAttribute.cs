using System.Web;
using System.Web.Mvc;

namespace Mic.Web.Common.Attribute
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        // public new string[] Privileges { get; set; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //TODO: have a look: http://stackoverflow.com/questions/6016892/asp-net-mvc-3-applying-authorizeattribute-to-areas
            var authRequired = !filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);

            //filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals(WebConfigurationManager.AppSettings["AppActionName"])
            if (authRequired)
            {
                HttpContextBase ctx = filterContext.HttpContext;
                //ctx.Request.UserHostAddress
                if (ctx.Session != null)
                {
                    if (ctx.Session.IsNewSession || SessionHelper.GetUser(ctx.Session) == null)
                    {
                        string returnUrl = string.Empty;
                        //returnUrl = ((System.Web.HttpRequestWrapper)ctx.Request).Url.AbsoluteUri;
                        filterContext.Result = new RedirectResult(string.Format("~/Account/Login"));
                    }
                    else
                    {

                    }
                }
            }

        }
    }
}