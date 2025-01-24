using FluentValidation;

namespace Application.Usecases.Logging.CreateAlertCommand;

public class CreateAlertCommandValidator : AbstractValidator<CreateAlertCommand>
{
    public CreateAlertCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .WithErrorCode("NAME_REQUIRED")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters")
            .WithErrorCode("NAME_TOO_LONG");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters")
            .WithErrorCode("DESCRIPTION_TOO_LONG");

        RuleFor(x => x.Condition)
            .NotNull()
            .WithMessage("Condition is required")
            .WithErrorCode("CONDITION_REQUIRED");

        RuleFor(x => x.Channel)
            .NotEmpty()
            .WithMessage("Channel is required")
            .WithErrorCode("CHANNEL_REQUIRED")
            .MaximumLength(50)
            .WithMessage("Channel must not exceed 50 characters")
            .WithErrorCode("CHANNEL_TOO_LONG");

        RuleFor(x => x.Target)
            .NotEmpty()
            .WithMessage("Target is required")
            .WithErrorCode("TARGET_REQUIRED")
            .MaximumLength(500)
            .WithMessage("Target must not exceed 500 characters")
            .WithErrorCode("TARGET_TOO_LONG");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required")
            .WithErrorCode("USER_ID_REQUIRED");
    }
} 