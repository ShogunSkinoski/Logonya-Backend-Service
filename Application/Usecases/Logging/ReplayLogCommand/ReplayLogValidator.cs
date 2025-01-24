using FluentValidation;

namespace Application.Usecases.Logging.ReplayLogCommand;

public class ReplayLogValidator : AbstractValidator<ReplayLogCommand>
{
    public ReplayLogValidator()
    {
        RuleFor(x => x.RequestId)
            .NotEmpty()
            .WithMessage("RequestId is required")
            .WithErrorCode("REQUEST_ID_REQUIRED")
            .MaximumLength(100)
            .WithMessage("RequestId must not exceed 100 characters")
            .WithErrorCode("REQUEST_ID_TOO_LONG");
    }
} 