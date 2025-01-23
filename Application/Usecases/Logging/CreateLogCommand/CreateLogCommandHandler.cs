using Application.Common;
using Domain.Account.Port;
using Domain.Common;
using Domain.Logging.Model;
using Domain.Logging.Port;
using FluentResults;
using FluentValidation;

using MediatR;

namespace Application.Usecases.Logging.CreateLogCommand;

public class CreateLogCommandHandler : IRequestHandler<CreateLogCommand, Result<CreateLogResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IValidator<CreateLogCommand> _validator;
    private readonly IMessagingProducer _messagingProducer;
    private readonly IKafkaSettings _kafkaSettings;
    public CreateLogCommandHandler(
            IUnitOfWork uow,
            IValidator<CreateLogCommand> validator,
            IMessagingProducer messagingProducer,
            IKafkaSettings kafkaSettings)
    {
        _uow = uow;
        _validator = validator;
        _messagingProducer = messagingProducer;
        _kafkaSettings = kafkaSettings;
    }

    public async Task<Result<CreateLogResponse>> Handle(CreateLogCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return validation.ToResult<CreateLogResponse>();

        var apiKey = await _uow.GetRepository<ApiKeyRepositoryPort>().FindApiKey(request.ApiKeyId, cancellationToken);
        if (apiKey == null)
            return Result.Fail(new Error("Cannot find API key"));

        var log = new Log(
            action: request.Action,
            description: request.Description,
            level: request.Level,
            source: request.Source,
            environment: request.Environment,
            ipAddress: request.IpAddress,
            metadata: request.Metadata,
            userName: request.UserName,
            apiKeyId: apiKey.Id,
            userId: apiKey.UserId,
            exception: request.Exception,
            stackTrace: request.StackTrace
        );

        await _uow.GetRepository<LogRepositoryPort>().AddAsync(log, cancellationToken);
        await _uow.CompleteAsync(cancellationToken);
        var message = CreateLogMessage.FromLog(log);
        var topic = _kafkaSettings.Topics["LogVectorization"].Name;
        _messagingProducer.SendMessage(topic, "LOG", message);

        return Result.Ok(new CreateLogResponse(
            id: log.Id,
            action: log.Action,
            level: log.Level,
            source: log.Source,
            createdAt: log.CreatedAt
        ));
    }
}
