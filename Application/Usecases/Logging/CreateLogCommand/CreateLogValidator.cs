using Application.Common;
using FluentValidation;

namespace Application.Usecases.Logging.CreateLogCommand;

public class CreateLogValidator : BaseValidator<CreateLogCommand>
{
    public CreateLogValidator()
    {
        // Required fields validation
        ValidateRequired(nameof(CreateLogCommand.Action));
        ValidateRequired(nameof(CreateLogCommand.Description));
        ValidateRequired(nameof(CreateLogCommand.Level));
        ValidateRequired(nameof(CreateLogCommand.Source));
        ValidateRequired(nameof(CreateLogCommand.Environment));
        ValidateRequired(nameof(CreateLogCommand.IpAddress));
        ValidateRequired(nameof(CreateLogCommand.ApiKeyId));

        // Length validations
        RuleFor(x => x.Action)
            .MaximumLength(200)
            .WithMessage("Action must not exceed 200 characters.")
            .WithErrorCode("ACTION_TOO_LONG");

        RuleFor(x => x.Source)
            .MaximumLength(200)
            .WithMessage("Source must not exceed 200 characters.")
            .WithErrorCode("SOURCE_TOO_LONG");

        RuleFor(x => x.Level)
            .MaximumLength(50)
            .WithMessage("Level must not exceed 50 characters.")
            .WithErrorCode("LEVEL_TOO_LONG")
            .Must(BeValidLogLevel)
            .WithMessage("Level must be one of: INFO, WARNING, ERROR, DEBUG, CRITICAL")
            .WithErrorCode("INVALID_LOG_LEVEL");

        RuleFor(x => x.Environment)
            .MaximumLength(50)
            .WithMessage("Environment must not exceed 50 characters.")
            .WithErrorCode("ENVIRONMENT_TOO_LONG");

        //RuleFor(x => x.IpAddress)
        //    .MaximumLength(50)
        //    .WithMessage("IP Address must not exceed 50 characters.")
        //    .WithErrorCode("IP_ADDRESS_TOO_LONG")
        //    .Matches(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$|^([0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}$")
            
        //    .WithMessage("Invalid IP address format.")
        //    .WithErrorCode("INVALID_IP_ADDRESS");

        When(x => x.UserName != null, () =>
        {
            RuleFor(x => x.UserName)
                .MaximumLength(200)
                .WithMessage("Username must not exceed 200 characters.")
                .WithErrorCode("USERNAME_TOO_LONG");
        });
    }

    private static bool BeValidLogLevel(string level)
    {
        var validLevels = new[] { "INFO", "WARNING", "ERROR", "DEBUG", "CRITICAL" };
        return validLevels.Contains(level.ToUpper());
    }
}
