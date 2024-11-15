using FluentResults;
using MediatR;

namespace Application.Usecases.Logging.CreateLogCommand;

public sealed record CreateLogCommand : IRequest<Result<CreateLogResponse>>
{
    public string Action { get; init; }
    public string Description { get; init; }
    public string Level { get; init; }
    public string Source { get; init; }
    public string Environment { get; init; }
    public string IpAddress { get; init; }
    public string ApiKeyId { get; init; }
    public Dictionary<string, string>? Metadata { get; init; }
    public string? UserName { get; init; }
    public string? Exception { get; init; }
    public string? StackTrace { get; init; }
}
