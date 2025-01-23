using Application.Common;
using Domain.Account.Port;
using Domain.Chat.Model;
using Domain.Chat.Port;
using Domain.Common;
using FluentResults;
using FluentValidation;
using MediatR;

namespace Application.Usecases.Chat.CreateChatCommand;

public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand, Result<CreateChatResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IValidator<CreateChatCommand> _validator;

    public CreateChatCommandHandler(IUnitOfWork uow, IValidator<CreateChatCommand> validator)
    {
        _uow = uow;
        _validator = validator;
    }

    public async Task<Result<CreateChatResponse>> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return validation.ToResult<CreateChatResponse>();

        if (!Guid.TryParse(request.UserId, out var userId))
            return Result.Fail(new Error("Invalid user ID format")
                .WithMetadata("ErrorCode", "INVALID_USER_ID"));

        var chatRepository = _uow.GetRepository<ChatHistoryRepositoryPort>();
        var chatHistory = new ChatHistory(request.Title, userId);
        
        await chatRepository.AddAsync(chatHistory, cancellationToken);
        await _uow.CompleteAsync(cancellationToken);

        return Result.Ok(new CreateChatResponse(
            chatHistory.Id,
            chatHistory.Title,
            chatHistory.CreatedAt
        ));
    }
} 