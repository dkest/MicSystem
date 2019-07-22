using Swashbuckle.Swagger;
using System;
using System.Web.Http.Description;

namespace Mic.Api.Filter
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public partial class HiddenApiAttribute : Attribute { }
    public class HiddenApiFilter : IDocumentFilter
    {

        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            foreach (ApiDescription apiDescription in apiExplorer.ApiDescriptions)
            {
                var _key = "/" + apiDescription.RelativePath.TrimEnd('/');
                // 过滤 swagger 自带的接口
                if (_key.Contains("/api/Swagger") && swaggerDoc.paths.ContainsKey(_key))
                    swaggerDoc.paths.Remove(_key);
            }
        }
    }
}