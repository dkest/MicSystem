using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace Mic.Api.Filter
{
    public class SwaggerOperatorFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (!string.IsNullOrWhiteSpace(operation.summary) && operation.summary.Contains("[AUTH]"))
            {
                if (operation.parameters == null) operation.parameters = new List<Parameter>();
                operation.parameters.Add(new Parameter
                {
                    name = "access-token",
                    @in = "header",
                    required = true,
                    type = "string"
                });
            }

            if (!string.IsNullOrWhiteSpace(operation.summary) && operation.summary.Contains("[FILE]"))
            {
                operation.consumes.Add("application/form-data");
                if (operation.parameters == null) operation.parameters = new List<Parameter>();
                operation.parameters.Add(new Parameter
                {
                    name = "file",
                    @in = "formData",
                    required = true,
                    type = "file"
                });
            }
        }

    }
}