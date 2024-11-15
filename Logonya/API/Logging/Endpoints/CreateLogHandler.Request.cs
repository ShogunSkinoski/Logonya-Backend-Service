namespace Presentation.API.Logging;

public sealed record CreateLogRequest(
    string Action,
    string Description,
    string Level,
    string Source,
    string Environment,
    Dictionary<string, string>? Metadata,
    string? UserName,
    string? Exception,
    string? StackTrace
);
