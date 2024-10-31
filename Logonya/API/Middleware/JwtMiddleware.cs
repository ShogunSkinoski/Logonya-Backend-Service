using Application.Common;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.API.Middleware;

public class JwtMiddleware
    (
        RequestDelegate next,
        IJWTGenerator jwtGenerator
    )
{
    private readonly IJWTGenerator _jwtGenerator = jwtGenerator;
    private readonly RequestDelegate _next = next;
    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if(endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null)
        {
            await _next(context);
            return;
        }
        var token = context.Request.Headers["Authorization"]
                        .FirstOrDefault()?.Split(" ").Last();
        if(token == null)
        {
            await _next(context);
            return;
        }
        var isValid = _jwtGenerator.ValidateToken(token);
        if (!isValid)
        {
            await _next(context);
            return;
        }
        await _next(context);
    }
}
