using Application.Common;
using Domain.Common;
using Domain.Logging.Model;
using Domain.Logging.Port;
using FluentResults;
using FluentValidation;
using MediatR;
using System.Text.Json;

namespace Application.Usecases.Logging.CreateAlertCommand
{
    public class CreateAlertCommandHandler : IRequestHandler<CreateAlertCommand, Result<CreateAlertResponse>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<CreateAlertCommand> _validator;

        public CreateAlertCommandHandler(
            IUnitOfWork uow,
            IValidator<CreateAlertCommand> validator)
        {
            _uow = uow;
            _validator = validator;
        }

        public async Task<Result<CreateAlertResponse>> Handle(CreateAlertCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
                return validation.ToResult<CreateAlertResponse>();

            if (!Guid.TryParse(request.UserId, out var userId))
                return Result.Fail(new Error("Invalid user ID")
                    .WithMetadata("ErrorCode", "INVALID_USER_ID"));

            var alert = new Alert(
                name: request.Name,
                description: request.Description,
                condition: JsonSerializer.Serialize(request.Condition),
                channel: request.Channel,
                target: request.Target,
                userId: userId
            );

            var repository = _uow.GetRepository<AlertRepositoryPort>();
            await repository.AddAsync(alert, cancellationToken);
            await _uow.CompleteAsync(cancellationToken);

            return Result.Ok(new CreateAlertResponse(
                alert.Id,
                alert.Name,
                alert.CreatedAt
            ));
        }
    }
} 