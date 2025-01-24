
namespace Presentation.API.Logging;

public static partial class LoggingEndpoints
{
    public static void MapLoggingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/logging")
            .WithTags("Logging")
            .RequireAuthorization();

        group.MapPost("/", CreateLogHandler);
        group.MapGet("/replay", ReplayLogHandler);

        
        var alertGroup = group.MapGroup("/alerts");
        alertGroup.MapPost("/", CreateAlertHandler);

    }
}
