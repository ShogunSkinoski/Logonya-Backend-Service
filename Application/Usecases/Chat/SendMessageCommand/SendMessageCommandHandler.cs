using Anthropic;
using Anthropic.SDK;
using Anthropic.SDK.Constants;
using Anthropic.SDK.Messaging;
using Application.Common;
using Domain.Chat.Model;
using Domain.Chat.Port;
using Domain.Common;
using FluentResults;
using FluentValidation;
using MediatR;
using Error = FluentResults.Error;

namespace Application.Usecases.Chat.SendMessageCommand;

public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, Result<SendMessageResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IValidator<SendMessageCommand> _validator;
    private readonly AnthropicClient _anthropicClient;
    private readonly IRAGService _ragService;

    public SendMessageCommandHandler(
        IUnitOfWork uow, 
        IValidator<SendMessageCommand> validator,
        AnthropicClient anthropicClient,
        IRAGService ragService)
    {
        _uow = uow;
        _validator = validator;
        _anthropicClient = anthropicClient;
        _ragService = ragService;
    }

    public async Task<Result<SendMessageResponse>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return validation.ToResult<SendMessageResponse>();

        if (!Guid.TryParse(request.ChatHistoryId, out var chatHistoryId))
            return Result.Fail(new Error("Invalid chat history ID format")
                .WithMetadata("ErrorCode", "INVALID_CHAT_HISTORY_ID"));

        // Search for relevant context
        var context = await _ragService.SearchAsync(request.Content, cancellationToken: cancellationToken);

        var systemMessage = string.IsNullOrEmpty(context) ? null : 
            new SystemMessage($"Use this context to help answer the question: {context}");

        var messages = new List<Message>
        {
            new Message(RoleType.User, request.Content)
        };

        var parameters = new MessageParameters
        {
            Messages = messages,
            System = systemMessage != null ? new List<SystemMessage> { systemMessage } : null,
            MaxTokens = 1024,
            Model = AnthropicModels.Claude35Sonnet,
            Stream = false,
            Temperature = 1.0m
        };

        var anthropicResponse = await _anthropicClient.Messages.GetClaudeMessageAsync(parameters, cancellationToken);

        var chatRepository = _uow.GetRepository<ChatMessageRepositoryPort>();

        // Save user message
        var userMessage = new ChatMessage(
            content: request.Content,
            role: request.Role,
            chatHistoryId: chatHistoryId,
            tokenCount: anthropicResponse.Usage.InputTokens
        );
        await chatRepository.AddAsync(userMessage, cancellationToken);

        // Save assistant response
        var assistantMessage = new ChatMessage(
            content: anthropicResponse.Message.Content.OfType<TextContent>().First().Text,
            role: "assistant",
            chatHistoryId: chatHistoryId,
            tokenCount: anthropicResponse.Usage.OutputTokens
        );
        await chatRepository.AddAsync(assistantMessage, cancellationToken);

        await _uow.CompleteAsync(cancellationToken);

        return Result.Ok(new SendMessageResponse(
            assistantMessage.Id,
            assistantMessage.Content,
            assistantMessage.Role,
            assistantMessage.CreatedAt
        ));
    }
} 