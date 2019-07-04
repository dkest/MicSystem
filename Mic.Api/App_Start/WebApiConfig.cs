using Mic.Api.Filter;
using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace Mic.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            string name = Assembly.GetExecutingAssembly().GetName().Name;
            GlobalConfiguration.Configuration.EnableSwagger("docs/{apiVersion}", (conf) => {
                conf.SingleApiVersion("v2", name);
                conf.IncludeXmlComments(string.Format("{0}bin\\{1}.xml", AppDomain.CurrentDomain.BaseDirectory, name));
                conf.DescribeAllEnumsAsStrings();
                conf.OperationFilter<SwaggerOperatorFilter>();
            }).EnableSwaggerUi("help/{*assetPath}", (conf) => {
                conf.DisableValidator();
            });
        }
    }
}
