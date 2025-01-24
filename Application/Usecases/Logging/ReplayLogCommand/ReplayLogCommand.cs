using FluentResults;
using MediatR;

namespace Application.Usecases.Logging.ReplayLogCommand;

public sealed record ReplayLogCommand : IRequest<Result<ReplayLogResponse>>
{
    public string RequestId { get; init; }
    public Guid UserId { get; init; }
}

