using Application.Common;

namespace Presentation.API.Middleware;

public class ApiKeyMiddleware : IMiddleware
{
    private const string API_KEY_HEADER = "X-API-KEY";
    private readonly IApiKeyValidator _apiKeyValidator;
    private readonly ILogger<ApiKeyMiddleware> _logger;

    public ApiKeyMiddleware(
        IApiKeyValidator apiKeyValidator,
        ILogger<ApiKeyMiddleware> logger)
    {
        _apiKeyValidator = apiKeyValidator;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {

        if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var extractedApiKey))
        {
            _logger.LogWarning("API Key missing");
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { message = "API Key is missing" });
            return;
        }

        var apiKey = extractedApiKey.ToString();

        if (await _apiKeyValidator.IsValidApiKey(apiKey))
        {
            await next(context);
            return;
        }

        _logger.LogWarning("Invalid API Key: {ApiKey}", apiKey);
        context.Response.StatusCode = 401;
        await context.Response.WriteAsJsonAsync(new { message = "Invalid API Key" });
    }
}
