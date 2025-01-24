using Domain.Common;
using Domain.Logging.Port;
using FluentResults;
using MediatR;
using Anthropic;
using Anthropic.SDK;
using Anthropic.SDK.Constants;
using Anthropic.SDK.Messaging;
using Error = FluentResults.Error;

namespace Application.Usecases.Logging.ReplayLogCommand;

public class ReplayLogCommandHandler : IRequestHandler<ReplayLogCommand, Result<ReplayLogResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly AnthropicClient _anthropicClient;

    public ReplayLogCommandHandler(
        IUnitOfWork uow,
        AnthropicClient anthropicClient)
    {
        _uow = uow;
        _anthropicClient = anthropicClient;
    }

    public async Task<Result<ReplayLogResponse>> Handle(ReplayLogCommand request, CancellationToken cancellationToken)
    {
        var logRepository = _uow.GetRepository<LogRepositoryPort>();
        var logs = logRepository.GetLogsForReplay(request.RequestId);

        if (!logs.Any())
            return Result.Fail(new Error("No logs found for the specified request ID")
                .WithMetadata("ErrorCode", "LOGS_NOT_FOUND"));

        var analysisPrompts = new List<string>();
        foreach (var log in logs)
        {
            var prompt = $"""
                Analyze this log entry (level: {log.Level}, source: {log.Source}):
                Action: {log.Action}
                Description: {log.Description}
                Metadata: {string.Join(", ", log.Metadata)}
                Exception: {log.Exception ?? "None"}
                Timestamp: {log.CreatedAt:u}
                
                Identify anomalies or root causes. Respond in 3 sentences.
                """;
            analysisPrompts.Add(prompt);
        }

        var messages = new List<Message>
        {
            new Message(RoleType.User, string.Join("\n\n", analysisPrompts))
        };

        var parameters = new MessageParameters
        {
            Messages = messages,
            MaxTokens = 1024,
            Model = AnthropicModels.Claude35Sonnet,
            Stream = false,
            Temperature = 0.7m
        };

        var anthropicResponse = await _anthropicClient.Messages.GetClaudeMessageAsync(parameters, cancellationToken);
        var analysis = anthropicResponse.Message.Content.OfType<TextContent>().First().Text;

        return Result.Ok(new ReplayLogResponse(
            logs: logs,
            analysis: analysis
        ));
    }
} 