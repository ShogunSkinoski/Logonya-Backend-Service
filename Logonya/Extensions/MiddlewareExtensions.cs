using Application.Common;
using Presentation.API.Middleware;
using Presentation.API.Swagger;
using System.Text.Json;

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
            // Get the middleware instance
            var middleware = context.HttpContext.RequestServices
                .GetRequiredService<ApiKeyMiddleware>();

            // Create a new memory stream for capturing the response
            var originalBodyStream = context.HttpContext.Response.Body;
            await using var memoryStream = new MemoryStream();
            context.HttpContext.Response.Body = memoryStream;

            try
            {
                // Create a wrapper to capture the middleware result
                var responseWrapper = new ResponseWrapper();
                RequestDelegate wrappedNext = async (HttpContext httpContext) =>
                {
                    try
                    {
                        var result = await next(context);
                        if (result is IResult iResult)
                        {
                            await iResult.ExecuteAsync(httpContext);
                            responseWrapper.Result = (IResult)result;
                        }
                    }
                    catch (Exception ex)
                    {
                        responseWrapper.Exception = ex;
                    }
                };

                // Execute the middleware
                await middleware.InvokeAsync(context.HttpContext, wrappedNext);

                // Handle unauthorized response
                if (context.HttpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    memoryStream.Position = 0;
                    using var reader = new StreamReader(memoryStream);
                    var unauthorizedResponse = await reader.ReadToEndAsync();
                    return Results.Unauthorized();
                }

                // Handle exceptions
                if (responseWrapper.Exception != null)
                {
                    throw responseWrapper.Exception;
                }

                // Return the captured result if available
                if (responseWrapper.Result != null)
                {
                    return responseWrapper.Result;
                }

                // If no result was captured but we have response content, deserialize and return it
                memoryStream.Position = 0;
                using (var reader = new StreamReader(memoryStream))
                {
                    var responseBody = await reader.ReadToEndAsync();
                    if (!string.IsNullOrEmpty(responseBody))
                    {
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        return Results.Ok(JsonSerializer.Deserialize<object>(responseBody, options));
                    }
                }

                // Copy the response to the original stream
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(originalBodyStream);

                return Results.Ok();
            }
            finally
            {
                context.HttpContext.Response.Body = originalBodyStream;
            }
        });

        return builder;
    }

    private class ResponseWrapper
    {
        public IResult? Result { get; set; }
        public Exception? Exception { get; set; }
    }
}