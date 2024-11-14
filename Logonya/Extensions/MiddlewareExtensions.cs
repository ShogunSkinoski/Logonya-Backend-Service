using Application.Common;
using Presentation.API.Middleware;
using Presentation.API.Swagger;

namespace Presentation.Extensions;

public static class MiddlewareExtensions
{
    public static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddScoped<IApiKeyValidator, ApiKeyValidator>();
        services.AddScoped<ApiKeyMiddleware>();
        return services;
    }
    public static IEndpointConventionBuilder RequireApiKey(
       this IEndpointConventionBuilder builder)
    {
        builder.WithMetadata(new OpenApiSecurityRequirementAttribute("ApiKey"));

        builder.AddEndpointFilter(async (context, next) =>
        {
            var middleware = context.HttpContext.RequestServices
                .GetRequiredService<ApiKeyMiddleware>();

            var originalBodyStream = context.HttpContext.Response.Body;
            using var memoryStream = new MemoryStream();
            context.HttpContext.Response.Body = memoryStream;

            RequestDelegate nextDelegate = async (HttpContext httpContext) =>
            {
                var result = await next(context);
                return;
            };

            await middleware.InvokeAsync(context.HttpContext, nextDelegate);

            memoryStream.Position = 0;
            await memoryStream.CopyToAsync(originalBodyStream);

            return context.HttpContext.Response.StatusCode == StatusCodes.Status401Unauthorized
                ? Results.Unauthorized()
                : await next(context);
        });

        return builder;
    }
}
