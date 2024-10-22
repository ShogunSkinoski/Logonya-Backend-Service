using FluentResults;
using MediatR;

namespace Application.Usecases.Account.CreateApiCommand;

public sealed record CreateApiCommand(
        string userId,
        string apiKeyName,
        string? description
    ) : IRequest<Result<CreateApiResponse>>;

