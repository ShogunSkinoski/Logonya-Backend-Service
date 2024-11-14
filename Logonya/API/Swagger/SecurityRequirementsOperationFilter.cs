using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Presentation.API.Swagger;

public class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var securityRequirements = context.ApiDescription.ActionDescriptor.EndpointMetadata
            .OfType<OpenApiSecurityRequirementAttribute>()
            .ToList();

        if (!securityRequirements.Any()) return;

        operation.Security = new List<OpenApiSecurityRequirement>();

        foreach (var requirement in securityRequirements)
        {
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = requirement.SecuritySchemaName
                        }
                    },
                    new List<string>()
                }
            });
        }
    }
}