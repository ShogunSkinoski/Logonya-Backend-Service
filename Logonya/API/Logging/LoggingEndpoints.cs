
using Presentation.Extensions;

namespace Presentation.API.Logging;

public static partial class LoggingEndpoints
{
    public static RouteGroupBuilder MapLoggingEndpoints(this RouteGroupBuilder builder)
    {
        builder.MapPost("log", CreateLogHandler).RequireApiKey();
        return builder;
    }

}
