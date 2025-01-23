using Application.Common;
using FluentValidation;

namespace Application.Usecases.Chat.SendMessageCommand;

public class SendMessageCommandValidator : BaseValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        ValidateRequired(nameof(SendMessageCommand.ChatHistoryId));
        ValidateRequired(nameof(SendMessageCommand.Content));
        ValidateRequired(nameof(SendMessageCommand.Role));

        RuleFor(x => x.Role)
            .Must(BeValidRole)
            .WithMessage("Role must be one of: system, user, assistant")
            .WithErrorCode("INVALID_ROLE");

        RuleFor(x => x.TokenCount)
            .GreaterThan(0)
            .When(x => x.TokenCount.HasValue)
            .WithMessage("Token count must be greater than 0")
            .WithErrorCode("INVALID_TOKEN_COUNT");
    }

    private static bool BeValidRole(string role)
    {
        var validRoles = new[] { "system", "user", "assistant" };
        return validRoles.Contains(role.ToLower());
    }
} 