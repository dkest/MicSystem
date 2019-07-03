using System.Web.Http;
using WebActivatorEx;
using Mic.Api;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Mic.Api
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {

                        c.SingleApiVersion("v1", "Mic.ApiÔÚÏßÎÄµµ");


                    })
                .EnableSwaggerUi(c =>
                    {

                    });
        }
    }
}
