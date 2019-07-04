//using System.Web.Http;
//using WebActivatorEx;
//using Mic.Api;
//using Swashbuckle.Application;
//using System.Linq;

//[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

//namespace Mic.Api
//{
//    public class SwaggerConfig
//    {
//        public static void Register()
//        {
//            var thisAssembly = typeof(SwaggerConfig).Assembly;
//            GlobalConfiguration.Configuration
//                .EnableSwagger("docs/{apiVersion}/swagger",c =>
//                    {

//                        c.SingleApiVersion("v1", "Mic System API");
//                        c.IncludeXmlComments(GetXmlCommentsPath());
//                        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());


//                    })
//                .EnableSwaggerUi("api/{*assetPath}", c =>
//                    {

//                    });
//        }
//        private static string GetXmlCommentsPath()
//        {
//            return System.AppDomain.CurrentDomain.BaseDirectory + @"\bin\Mic.Api.xml";
//        }
//    }
//}
