using System.Net;
using FluentResults;

namespace Presentation.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = new ApiResponse<object> { IsSuccess = false };
        if (exception.Source == "FluentResults")
        {
            
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            response = ApiResponse<object>.FromException(exception);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.Errors = new List<ApiError>
            {
                new()
                {
                    Message = "An internal server error occurred",
                    ErrorCode = "INTERNAL_SERVER_ERROR"
                }
            };
        }

        return context.Response.WriteAsJsonAsync(response);
    }
}