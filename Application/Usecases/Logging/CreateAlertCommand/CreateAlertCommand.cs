using FluentResults;
using MediatR;

namespace Application.Usecases.Logging.CreateAlertCommand
{
    public sealed record CreateAlertCommand : IRequest<Result<CreateAlertResponse>>
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public AlertCondition Condition { get; init; }
        public string Channel { get; init; }
        public string Target { get; init; }
        public string UserId { get; init; }
    }
} 