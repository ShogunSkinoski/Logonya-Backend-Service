namespace Presentation.API.Swagger;
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class OpenApiSecurityRequirementAttribute : Attribute
{
    public string SecuritySchemaName { get; }

    public OpenApiSecurityRequirementAttribute(string securitySchemaName)
    {
        SecuritySchemaName = securitySchemaName;
    }
}
