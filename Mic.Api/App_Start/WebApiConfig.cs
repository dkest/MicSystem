using Mic.Api.Filter;
using Swashbuckle.Application;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace Mic.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            string name = Assembly.GetExecutingAssembly().GetName().Name;
            GlobalConfiguration.Configuration.EnableSwagger("docs/{apiVersion}", (conf) => {
                conf.SingleApiVersion("v1", name);
                conf.DocumentFilter<HiddenApiFilter>();
                conf.IncludeXmlComments(string.Format("{0}bin\\{1}.xml", AppDomain.CurrentDomain.BaseDirectory, name));
                conf.DescribeAllEnumsAsStrings();
                conf.OperationFilter<SwaggerOperatorFilter>();
            }).EnableSwaggerUi("help/{*assetPath}", (conf) => {
                conf.DisableValidator();
            });
        }
    }
}
