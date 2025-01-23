using Domain.Account.Model;
using Domain.Account.Port;
using Domain.Common;
using FluentResults;
using MediatR;

namespace Application.Usecases.Account.CreateApiCommand;

public class CreateApiCommandHandler(IUnitOfWork uow) : IRequestHandler<CreateApiCommand, Result<CreateApiResponse>>
{
    private readonly IUnitOfWork _uow = uow;
    public async Task<Result<CreateApiResponse>> Handle(CreateApiCommand request, CancellationToken cancellationToken)
    {
        var repository = _uow.GetRepository<UserRepositoryPort>();
        if(!Guid.TryParse(request.userId, out var userId))
            return Result.Fail(new Error("Can not parse user id")
                .WithMetadata("ErrorCode", "INVALID_GUID"));

        var user = await repository.GetByIdAsync(userId);
        if (user == null)
            Result.Fail(new Error("Invalid credentials")
                .WithMetadata("ErrorCode", "INVALID_CREDENTIALS"));
        var apiKey = new ApiKey(
            name: request.apiKeyName,
            key: Guid.NewGuid().ToString(),
            description: request.description,
            userId: user.Id
        );
        var apiKeyRepository = _uow.GetRepository<ApiKeyRepositoryPort>();
        await apiKeyRepository.AddAsync(apiKey);
        await _uow.CompleteAsync(cancellationToken);
        return Result.Ok();
    }
}
