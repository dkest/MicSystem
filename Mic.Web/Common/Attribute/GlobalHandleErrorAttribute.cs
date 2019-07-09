using System.Web.Mvc;

namespace Mic.Web.Common.Attribute
{
    /// <summary>
    /// 错误日志（Controller发生异常时会执行这里）
    /// </summary>
    public class GlobalHandleErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            //Exception Error = filterContext.Exception;
            //string controllerName = filterContext.RouteData.Values["controller"].ToString();
            //string actionName = filterContext.RouteData.Values["action"].ToString();
            //FileLoggerHelper.WriteErrorLog(string.Format("捕获到未处理的异常（{0}/{1}）：{2}",
            //    controllerName, actionName, Error.Message));
            //DBLogUtil.LogAsync(new LogEntity()
            //{
            //    Category = ((int)SysLogCategoryEnum.Exception).ToString(),
            //    LogDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            //    RequestHost = filterContext.HttpContext.Request.Url.ToString(),
            //    RequestUrl = string.Format("{0}/{1}", controllerName, actionName),
            //    ResponseText = Error.Message
            //});
            //filterContext.ExceptionHandled = true;
            //filterContext.Result = new RedirectResult("~/Home/Error500");//跳转至错误提示页面
        }
    }
}