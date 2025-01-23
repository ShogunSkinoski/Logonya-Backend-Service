using Domain.Common;
using Domain.Logging.Model;
using Domain.Logging.Port;
using FluentResults;
using FluentValidation;
using MediatR;

namespace Application.Usecases.Webhook.CreateWebhookCommand;

public class CreateWebhookCommandHandler : IRequestHandler<CreateWebhookCommand, Result<CreateWebhookResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly IValidator<CreateWebhookCommand> _validator;

    public CreateWebhookCommandHandler(
        IUnitOfWork uow,
        IValidator<CreateWebhookCommand> validator)
    {
        _uow = uow;
        _validator = validator;
    }

    public async Task<Result<CreateWebhookResponse>> Handle(
        CreateWebhookCommand request, 
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return Result.Fail("Invalid webhook request");

        if (!Guid.TryParse(request.UserId, out var userId))
            return Result.Fail("Invalid user ID");

        var repository = _uow.GetRepository<WebhookRepositoryPort>();
        
        // Generate a random secret for webhook signature verification
        var secret = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        
        var webhook = new Domain.Logging.Model.Webhook(
            name: request.Name,
            url: request.Url,
            secret: secret,
            userId: userId,
            events: request.Events
        );

        await repository.AddAsync(webhook, cancellationToken);
        await _uow.CompleteAsync(cancellationToken);

        return Result.Ok(new CreateWebhookResponse(
            webhook.Id,
            webhook.Name,
            webhook.Url,
            webhook.Secret,
            webhook.Events,
            webhook.CreatedAt
        ));
    }
} 